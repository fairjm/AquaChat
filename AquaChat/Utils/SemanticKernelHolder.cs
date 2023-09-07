using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquaChat.Databases;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Memory.Sqlite;
using Microsoft.SemanticKernel.Memory;

namespace AquaChat.Utils;
public class ProxyOpenAiHandler : HttpClientHandler
{
    public string Prefix { get; set; }

    public ProxyOpenAiHandler(string prefix)
    {
        Prefix = prefix;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri != null && request.RequestUri.Host.Equals("api.openai.com", StringComparison.OrdinalIgnoreCase))
        {
            request.RequestUri = new Uri($"{Prefix}{request.RequestUri.PathAndQuery}");
        }
        return base.SendAsync(request, cancellationToken);
    }
}


public static class SemanticKernelHolder
{

    private static IKernel? _iKernel;

    private static ProxyOpenAiHandler? _proxyOpenAiHandler;

    private static IMemoryStore? _memoryStore;

    public static IMemoryStore? GetMemoryStore()
    {
        return _memoryStore;
    }

    public static IKernel? GetKernel()
    {
        return _iKernel;
    }

    public static IKernel Init(string proxy, string secret, string model)
    {
        _proxyOpenAiHandler = new ProxyOpenAiHandler(proxy);
        var builder = new KernelBuilder();

        builder.WithOpenAIChatCompletionService(model,
                 secret,
                 alsoAsTextCompletion: true,
                 httpClient: new HttpClient(_proxyOpenAiHandler));
        builder.WithOpenAITextEmbeddingGenerationService("text-embedding-ada-002", secret);
        _memoryStore = SqliteMemoryStore.ConnectAsync(Constants.MemoryDatabasePath).Result;
        builder.WithMemoryStorage(_memoryStore);
        _iKernel = builder.Build();

        return _iKernel;
    }

}