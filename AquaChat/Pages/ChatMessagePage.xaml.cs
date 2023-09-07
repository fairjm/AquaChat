using System.Diagnostics;
using AquaChat.Models;
using AquaChat.Services;

namespace AquaChat.Pages;

[QueryProperty(nameof(CurrentChat), "Chat")]
public partial class ChatMessagePage : ContentPage
{
    
    private ChatMessageState _chatMessageState;
    public Chat CurrentChat { get; set ; }

    public ChatMessagePage(ChatMessageState state)
    {
        InitializeComponent();
        Debug.WriteLine($"ChatMessagePage init.");
        _chatMessageState = state;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        _chatMessageState.CurrentChat = CurrentChat;
        Debug.WriteLine($"ChatMessagePage OnAppearing. chatId:{CurrentChat.Id}");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Debug.WriteLine("ChatMessagePage OnDisappearing");
    }
}