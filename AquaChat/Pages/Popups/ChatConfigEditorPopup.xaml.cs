using System.Diagnostics;
using AquaChat.Models;
using AquaChat.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;

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





}