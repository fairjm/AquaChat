using System.ComponentModel;
using System.Diagnostics;
using AquaChat.Messages;
using AquaChat.Models;
using AquaChat.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace AquaChat.Pages.Popups;

public partial class ChatTitleEditorPopup : Popup, INotifyPropertyChanged
{

    private long _chatId;
    private string _title;

    public ChatTitleEditorPopup(long chatId, string currentTitle)
    {
        InitializeComponent();
        Debug.WriteLine($"currentTitle - {currentTitle}");
        ChatTitle = currentTitle;
        _chatId = chatId;
        BindingContext = this;
    }

    public string ChatTitle
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }


    private async void CancelButtonOnClicked(object? sender, EventArgs e)
    {
        await this.CloseAsync();
    }

    private async void SaveButtonOnClicked(object? sender, EventArgs e)
    {
        Debug.WriteLine($"title: {ChatTitle}");
        WeakReferenceMessenger.Default.Send(new ChatUpdateTitleMessage()
        {
            ChatId = _chatId,
            Title = _title
        });
        await this.CloseAsync();
    }
}