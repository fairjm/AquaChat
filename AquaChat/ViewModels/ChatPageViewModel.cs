using System.Collections.ObjectModel;
using System.Diagnostics;
using AquaChat.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AquaChat.ViewModels;

public partial class ChatPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Chat>? _chats;

    public void Refresh()
    {
        Chats = new ObservableCollection<Chat>()
        {
            new Chat()
            {
                Id = 1,
                Created = DateTime.Now,
                LastMessage = "hello",
                Title = "hello"
            },
            new Chat()
            {
                Id = 2,
                Created = DateTime.Now,
                LastMessage = "hello",
                Title = "hello"
            },
            new Chat()
            {
                Id = 3,
                Created = DateTime.Now,
                LastMessage = "hello",
                Title = "hello"
            }, new Chat()
            {
                Id = 4,
                Created = DateTime.Now,
                LastMessage = "hello",
                Title = "hello"
            }, new Chat()
            {
                Id = 5,
                Created = DateTime.Now,
                LastMessage = "hello",
                Title = "hello"
            }
        };
        Debug.WriteLine($"Refresh: {Chats}");
    }

}