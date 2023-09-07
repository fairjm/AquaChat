using AquaChat.Models;
using AquaChat.Utils;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SemanticFunctions;

namespace AquaChat.Services;

public class ChatService
{
    public async Message ChatResponse(long chatId, string userInput)
    {
        var kernel = SemanticKernelHolder.GetKernel();

        if (kernel == null)
        {
            throw new InvalidOperationException("check openai settings");
        }


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
                MaxTokens = 2000,
                Temperature = 0.7,
                TopP = 0.5,
            }
        };

        var semanticFunction = kernel.CreateSemanticFunction(skPrompt, promptConfig);
        var newContext = kernel.CreateNewContext();
        newContext.Variables["userInput"] = userInput;
        newContext.Variables["systemMessage"] = "";
        newContext.Variables["context"] = "";
        semanticFunction.InvokeAsync()
    }
}