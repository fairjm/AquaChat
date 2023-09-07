using AquaChat.Pages;

namespace AquaChat;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
        Routing.RegisterRoute(nameof(ChatMessagePage), typeof(ChatMessagePage));
        Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
        Routing.RegisterRoute(nameof(EmbeddingTestPage), typeof(EmbeddingTestPage));
    }
}
