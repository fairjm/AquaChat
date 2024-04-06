using AquaChat.Databases;
using AquaChat.Models;
using AquaChat.Utils;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SemanticFunctions;

namespace AquaChat.Services;

public class ChatService
{
    private static readonly int LastMessageLen = 150;

    private readonly ChatDao _chatDao;
    private readonly ChatConfigDao _chatConfigDao;
    private readonly MessageDao _messageDao;

    public ChatService(ChatDao chatDao, ChatConfigDao chatConfigDao, MessageDao messageDao)
    {
        _chatDao = chatDao;
        _chatConfigDao = chatConfigDao;
        _messageDao = messageDao;
    }

    public async Task<Chat> NewChat()
    {
        return await _chatDao.CreateNewItem().ConfigureAwait(false);
    }

    public async Task<List<Chat>> ListChatsAsync()
    {
        return await _chatDao.ListChatsAsync().ConfigureAwait(false);
    }

    public async Task<List<Message>> ListChatMessages(long chatId)
    {
        return await _messageDao.ListMessageByChatIdAsync(chatId).ConfigureAwait(false);
    }

    public async Task DeleteChat(long chatId)
    {
        await _chatDao.DeleteAsync(chatId).ConfigureAwait(false);
        await _chatConfigDao.DeleteByChatId(chatId);
        await _messageDao.DeleteMessagesByChatId(chatId);
    }

    public async Task DeleteMessage(long messageId)
    {
        await _messageDao.DeleteMessagesById(messageId).ConfigureAwait(false);
    }

    public async Task<ChatConfig> GetChatConfig(long chatId)
    {
        var chatConfig = await _chatConfigDao.GetByChatId(chatId).ConfigureAwait(false);
        if (chatConfig is not null)
        {
            return chatConfig;
        }
        chatConfig = new ChatConfig
        {
            ChatId = chatId,
            SystemMessage = "",
            LastMessageNum = 10,
            MaxTokens = 2000,
            Temperature = 0.0,
            PresencePenalty = 0.0,
            FrequencyPenalty = 0.0,
            Created = DateTime.Now,
        };
        await _chatConfigDao.SaveOrReplaceConfig(chatConfig);
        return chatConfig;
    }

    public async Task<ChatConfig> SaveChatConfig(ChatConfig chatConfig)
    {
        await _chatConfigDao.SaveOrReplaceConfig(chatConfig).ConfigureAwait(false);
        return chatConfig;
    }

    public async Task<Message> SaveHumanMessage(long chatId, string userInput)
    {
        var humanMessage = new Message
        {
            ChatId = chatId,
            MessageType = Message.TypeHuman,
            Content = userInput,
            ReferenceContent = null,
            ExtraContent = null,
            Created = DateTime.Now
        };

        await _chatDao.Update(chatId, e =>
        {
            e.LastMessage = userInput.Length > LastMessageLen ? (userInput[0..LastMessageLen] + "...") : userInput;
        }).ConfigureAwait(false);

        return await _messageDao.SaveNewMessage(humanMessage);
    }

    public async Task<Message> ChatResponse(long chatId, string userInput)
    {
        var chatConfig = await GetChatConfig(chatId);

        var kernel = SemanticKernelHolder.GetKernel() ?? throw new InvalidOperationException("check openai settings");

        const string skPrompt = @"
ChatBot can have a conversation with you about any topic.
It can give explicit instructions or say 'I don't know' if it does not have an answer.
{{$systemMessage}}

[CONTEXT]
{{$context}}
[END CONTEXT]

{{$history}}
User: {{$userInput}}
ChatBot:";

        var promptConfig = new PromptTemplateConfig
        {
            Completion =
            {
                MaxTokens = chatConfig.MaxTokens,
                Temperature = Math.Round(chatConfig.Temperature ?? 0, 2),
                FrequencyPenalty = Math.Round(chatConfig.FrequencyPenalty ?? 0, 2),
                PresencePenalty = Math.Round(chatConfig.PresencePenalty ?? 0, 2),
                //StopSequences = new List<string>() {"ChatBot:", "User: "}

            }
        };

        var listLastN = await _messageDao.ListLastN(chatId, chatConfig.LastMessageNum ?? 10).ConfigureAwait(false);
        string histories;
        if (listLastN.Count == 0)
        {
            histories = "";
        }
        else
        {
            var lastMessage = listLastN[0];
            // emmmm...we saved the human input before... no need to add it to history :(
            if ((lastMessage?.Content?.Equals(userInput) ?? false) && lastMessage.MessageType == Message.TypeHuman)
            {
                listLastN.RemoveAt(0);
            }
            histories = listLastN
                .AsEnumerable()
                .Reverse()
                .Select(m =>
                {
                    return m.MessageType switch
                    {
                        Message.TypeAi => $"ChatBot: {m.Content}",
                        Message.TypeHuman => $"User: {m.Content}",
                        _ => ""
                    };
                })
                .Where(e => e != "")
                .Aggregate("", (current, next) => current + "\n" + next);
        }

        if (listLastN.Count == 0)
        {
            string title;
            if (userInput.Length <= 10)
            {
                title = userInput;
            }
            else
            {
                title = await GenerateNewTitle(userInput);
            }
            await UpdateChatTitle(chatId, title);

        }

        var semanticFunction = kernel.CreateSemanticFunction(skPrompt, promptConfig);
        var newContext = kernel.CreateNewContext();
        newContext.Variables["userInput"] = userInput;
        newContext.Variables["systemMessage"] = chatConfig.SystemMessage ?? "";
        newContext.Variables["history"] = histories;
        // TODO add context info
        newContext.Variables["context"] = "";
        var invokeAsync = await semanticFunction.InvokeAsync(newContext);

        var invokeAsyncResult = invokeAsync.Result;
        Message message = new Message
        {
            ChatId = chatId,
            MessageType = Message.TypeAi,
            Content = invokeAsyncResult,
            ReferenceContent = null,
            ExtraContent = null,
            Created = DateTime.Now
        };
        await _chatDao.Update(chatId, e =>
        {
            e.LastMessage = invokeAsyncResult.Length > LastMessageLen ? (invokeAsyncResult[..LastMessageLen] + "...") : invokeAsyncResult;
        });
        return await _messageDao.SaveNewMessage(message);
    }

    public Task UpdateChatTitle(long chatId, string title)
    {
        return _chatDao.Update(chatId, e =>
        {
            e.Title = title;
        });
    }

    public async Task<string> GenerateNewTitle(string userInput)
    {
        var kernel = SemanticKernelHolder.GetKernel() ?? throw new InvalidOperationException("check openai settings");

        const string skPrompt = @"
generate a brief title no more than 20 words based on the user input: {{$userInput}}

===
examples:
user input: tell me how to make a nice dinner 
the title:Homemade Dinner Ideas

user input: 如何在steam中查看指定语言的好评/差评数量
the result: Steam游戏好评差评语言统计
===

you must use the same user input language.
you should just return the title itself only.
";

        var semanticFunction = kernel.CreateSemanticFunction(skPrompt);
        var newContext = kernel.CreateNewContext();
        newContext.Variables["userInput"] = userInput;
        var invokeAsync = await semanticFunction.InvokeAsync(newContext).ConfigureAwait(false);
        return invokeAsync.Result;
    }
}