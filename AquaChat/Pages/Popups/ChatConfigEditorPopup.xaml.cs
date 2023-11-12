using System.Diagnostics;
using AquaChat.Messages;
using AquaChat.Models;
using AquaChat.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace AquaChat.Pages.Popups;

public partial class ChatConfigEditorPopup : Popup
{

    private ChatConfig _chatConfig;

    public ChatConfigEditorPopup(ChatConfig chatConfig)
    {
        InitializeComponent();
        _chatConfig = chatConfig;
        this.BindingContext = _chatConfig;
    }

    private async void CancelButtonOnClicked(object? sender, EventArgs e)
    {
        await this.CloseAsync();
    }

    private async void SaveButtonOnClicked(object? sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new ChatConfigUpdateMessage()
        {
            Config = _chatConfig
        });
        await this.CloseAsync();
    }



}