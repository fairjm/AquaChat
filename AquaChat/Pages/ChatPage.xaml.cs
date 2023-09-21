
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
        Debug.WriteLine("ChatPage init");
        InitializeComponent();
        this._vm = vm;
        this.BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.Refresh();
    }
}