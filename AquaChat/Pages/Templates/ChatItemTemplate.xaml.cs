using AquaChat.Models;
using CommunityToolkit.Maui.Alerts;

namespace AquaChat.Pages.Templates;

public partial class ChatItemTemplate : ContentView
{
    public ChatItemTemplate()
    {
        InitializeComponent();
    }

    private async void OnItemTapped(object? sender, TappedEventArgs e)
    {
        var item = (Chat?)((sender as Grid)?.BindingContext);
        if (item is null)
        {
            return;
        }
        await Toast.Make("id:" + item.Id).Show();
        var dictionary = new Dictionary<string, object>()
        {
            { "Chat", item }
        };
        await Shell.Current.GoToAsync(nameof(ChatMessagePage), dictionary);
    }
}