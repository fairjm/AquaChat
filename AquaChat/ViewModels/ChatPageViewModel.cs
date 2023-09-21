using System.Collections.ObjectModel;
using System.Diagnostics;
using AquaChat.Messages;
using AquaChat.Models;
using AquaChat.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AquaChat.ViewModels;

public partial class ChatPageViewModel : ObservableObject
{
    private readonly ChatService _chatService;

    [ObservableProperty]
    private ObservableCollection<Chat>? _chats;

    public ChatPageViewModel(ChatService chatService)
    {
        _chatService = chatService;

        WeakReferenceMessenger.Default.Register<ChatDeletionMessage>(this, async (sender, e) =>
        {
            await RemoveChat(e.ChatId);
        });

    }

    public async Task Refresh()
    {
        var chats = await _chatService.ListChatsAsync();
        Chats = new ObservableCollection<Chat>(chats);
        Debug.WriteLine($"Refresh: {Chats}");
    }

    [RelayCommand]
    public async Task AddNewTask()
    {
        var newChat = await _chatService.NewChat();
        Chats?.Insert(0, newChat);
    }

    private async Task RemoveChat(long chatId)
    {
        await _chatService.DeleteChat(chatId);
        await Refresh();
    }
}