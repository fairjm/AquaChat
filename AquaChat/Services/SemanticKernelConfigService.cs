using AquaChat.Models;
using AquaChat.Utils;

namespace AquaChat.Services;

public class SemanticKernelConfigService
{
    public static readonly string KeyPrefix = "KEY_PREFIX";
    public static readonly string KeySecret = "KEY_SECRET";
    public static readonly string KeyModel = "KEY_MODEL";

    public static readonly string DefaultPrefix = "https://api.openai.com";
    public static readonly string DefaultModel = "gpt-3.5-turbo-16k";

    public void RefreshConfig()
    {
        UpdateConfig(GetConfig());
    }

    public AppConfig GetConfig()
    {
        var prefix = Preferences.Default.Get(KeyPrefix, "https://api.openai.com");
        var secret = Preferences.Default.Get(KeySecret, "");
        var model = Preferences.Default.Get(KeyModel, DefaultModel);
        return new AppConfig
        {
            OpenAiPrefix = string.IsNullOrWhiteSpace(prefix) ? DefaultPrefix : prefix,
            Model = string.IsNullOrWhiteSpace(secret) ? DefaultModel : model,
            Secret = string.IsNullOrWhiteSpace(secret) ? null : secret,
        };
    }

    public void UpdateConfig(AppConfig appConfig)
    {
        var appConfigOpenAiPrefix = appConfig.OpenAiPrefix ?? DefaultPrefix;
        Preferences.Default.Set(KeyPrefix, appConfigOpenAiPrefix);
        var appConfigSecret = appConfig.Secret ?? "";
        Preferences.Default.Set(KeySecret, appConfigSecret);
        var appConfigModel = appConfig.Model ?? DefaultModel;
        Preferences.Default.Set(KeyModel, appConfigModel);

        if (!string.IsNullOrWhiteSpace(appConfigSecret))
        {
            SemanticKernelHolder.Init(appConfigOpenAiPrefix, appConfigSecret, appConfigModel);
        }
    }
}