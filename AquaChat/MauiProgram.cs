using System.Diagnostics;
using AquaChat.Databases;
using AquaChat.Models;
using AquaChat.Pages;
using AquaChat.Services;
using AquaChat.Utils;
using AquaChat.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SQLite;

namespace AquaChat;

public static class MauiProgram
{
    public static MauiApp CreateMauiAppAsync()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .RegisterServices()
            .RegisterViews()
            .RegisterViewModels()
            .RegisterDateBases()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Logging.AddDebug();
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        var mauiApp = builder.Build();
        var appConfigService = mauiApp.Services.GetService<SemanticKernelConfigService>();
        appConfigService!.RefreshConfig();

        return mauiApp;
    }

    public static MauiAppBuilder RegisterDateBases(this MauiAppBuilder mauiAppBuilder)
    {
        Debug.WriteLine("db file:{}", Constants.DatabasePath);
        SQLiteAsyncConnection connection = new(Constants.DatabasePath, Constants.Flags);
        mauiAppBuilder.Services.AddSingleton(connection);
        mauiAppBuilder.Services.AddSingleton<ChatDao>();
        mauiAppBuilder.Services.AddSingleton<ChatConfigDao>();
        mauiAppBuilder.Services.AddSingleton<MessageDao>();

        connection.CreateTableAsync<Chat>();
        connection.CreateTableAsync<ChatConfig>();
        connection.CreateTableAsync<Message>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<SemanticKernelConfigService>();
        mauiAppBuilder.Services.AddSingleton<ChatService>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<SettingPage>();
        mauiAppBuilder.Services.AddSingleton<MainPage>();
        mauiAppBuilder.Services.AddSingleton<ChatPage>();
        mauiAppBuilder.Services.AddSingleton<ChatMessageState>();

        mauiAppBuilder.Services.AddTransient<ChatMessagePage>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<SettingPageViewModel>();
        mauiAppBuilder.Services.AddSingleton<ChatPageViewModel>();
        return mauiAppBuilder;
    }
}
