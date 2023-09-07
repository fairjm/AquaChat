
using System.Diagnostics;
using AquaChat.Models;
using AquaChat.ViewModels;
using CommunityToolkit.Maui.Alerts;

namespace AquaChat.Pages;

public partial class ChatPage : ContentPage
{
    private ChatPageViewModel _vm;
    public ChatPage(ChatPageViewModel vm)
    {
        vm.Refresh();
        Debug.WriteLine("ChatPage init");
        InitializeComponent();
        this._vm = vm;
        this.BindingContext = _vm;
    }

}