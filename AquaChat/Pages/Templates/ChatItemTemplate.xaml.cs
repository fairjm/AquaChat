using System;
using System.Diagnostics;
using AquaChat.Messages;
using AquaChat.Models;
using AquaChat.Pages.Popups;
using AquaChat.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Maui.Views;

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
        var dictionary = new Dictionary<string, object>()
        {
            { "Chat", item }
        };
        await Shell.Current.GoToAsync(nameof(ChatMessagePage), dictionary);
    }

    private void OnDeleteSwipeItemInvoked(object? sender, EventArgs e)
    {
        var chat = (sender as SwipeItem)?.BindingContext as Chat;
        Debug.WriteLine($"delete chat:{chat}");
    }

    private async void OnEditTapped(object? sender, TappedEventArgs e)
    {
        var item = (Chat?)((sender as Label)?.BindingContext);
        if (item is null)
        {
            Debug.WriteLine($"item is null");
            return;
        }

        var editorPopup = new ChatTitleEditorPopup(item.Id, item.Title ?? "");
        await Shell.Current.CurrentPage.ShowPopupAsync(editorPopup);
    }

    private async void OnTrashTapped(object? sender, TappedEventArgs e)
    {
        var item = (Chat?)((sender as Label)?.BindingContext);
        if (item is null)
        {
            Debug.WriteLine($"item is null");
            return;
        }

        var deleted = await Application.Current.MainPage.DisplayAlert("Delete the Conversation?",
            $"Do you want to delete the conversation: {item.Title}",
            "Delete",
            "Cancel");

        if (deleted)
        {
            WeakReferenceMessenger.Default.Send(new ChatDeletionMessage()
            {
                ChatId = item.Id
            });
        }
    }
}