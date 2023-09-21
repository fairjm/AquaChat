using AquaChat.Databases;
using AquaChat.Models;
using AquaChat.Utils;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SemanticFunctions;

namespace AquaChat.Services;

public class ChatService
{
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
        return await _chatDao.CreateNewItem();
    }

    public async Task<List<Chat>> ListChatsAsync()
    {
        return await _chatDao.ListChatsAsync();
    }

    public async Task<List<Message>> ListChatMessages(long chatId)
    {
        return await _messageDao.ListMessageByChatIdAsync(chatId);
    }

    public async Task DeleteChat(long chatId)
    {
        await _chatDao.DeleteAsync(chatId);
        await _chatConfigDao.DeleteByChatId(chatId);
        await _messageDao.DeleteMessagesByChatId(chatId);

    }

    public async Task<ChatConfig> GetChatConfig(long chatId)
    {
        var chatConfig = await _chatConfigDao.GetByChatId(chatId);
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
                Temperature = chatConfig.Temperature ?? 0,
                FrequencyPenalty = chatConfig.FrequencyPenalty ?? 0,
                PresencePenalty = chatConfig.PresencePenalty ?? 0,
                //StopSequences = new List<string>() {"ChatBot:", "User: "}

            }
        };

        var listLastN = await _messageDao.ListLastN(chatId, chatConfig.LastMessageNum ?? 10);
        string histories;
        if (listLastN.Count == 0)
        {
            histories = "";
        }
        else
        {
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

        var semanticFunction = kernel.CreateSemanticFunction(skPrompt, promptConfig);
        var newContext = kernel.CreateNewContext();
        newContext.Variables["userInput"] = userInput;
        newContext.Variables["systemMessage"] = chatConfig.SystemMessage ?? "";
        newContext.Variables["history"] = histories;
        // TODO add context info
        newContext.Variables["context"] = "";
        var invokeAsync = await semanticFunction.InvokeAsync(newContext);

        Message message = new Message
        {
            ChatId = chatId,
            MessageType = Message.TypeAi,
            Content = invokeAsync.Result,
            ReferenceContent = null,
            ExtraContent = null,
            Created = DateTime.Now
        };
        Message humanMessage = new Message
        {
            ChatId = chatId,
            MessageType = Message.TypeHuman,
            Content = userInput,
            ReferenceContent = null,
            ExtraContent = null,
            Created = DateTime.Now
        };

        await _messageDao.SaveNewMessage(humanMessage);
        return await _messageDao.SaveNewMessage(message);
    }
}