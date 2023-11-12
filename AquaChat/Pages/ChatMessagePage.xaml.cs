using System.Diagnostics;
using AquaChat.Messages;
using AquaChat.Models;
using AquaChat.Pages.Popups;
using AquaChat.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace AquaChat.Pages;

[QueryProperty(nameof(CurrentChat), "Chat")]
public partial class ChatMessagePage : ContentPage
{
    
    private ChatMessageState _chatMessageState;
    private ChatService _chatService;
    public Chat CurrentChat { get; set ; }

    public ChatMessagePage(ChatMessageState state, ChatService chatService)
    {
        InitializeComponent();
        Debug.WriteLine($"ChatMessagePage init.");
        _chatMessageState = state;
        _chatService = chatService;

        WeakReferenceMessenger.Default.Register<ChatConfigUpdateMessage>(this, async (sender, e) =>
        {
            if (e.Config is not null)
            {
                await _chatService.SaveChatConfig(e.Config);
            }
        });
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

    private async void SettingBarOnClick(object? sender, EventArgs e)
    {
        var chatConfig = await _chatService.GetChatConfig(CurrentChat.Id);
        var editorPopup = new ChatConfigEditorPopup(chatConfig);
        
        await this.ShowPopupAsync(editorPopup);
    }
}