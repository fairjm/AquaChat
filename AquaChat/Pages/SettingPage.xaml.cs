using AquaChat.Services;
using AquaChat.ViewModels;
using System.Windows.Input;

namespace AquaChat.Pages;

public partial class SettingPage : ContentPage
{
    private readonly SettingPageViewModel _vm;

    public SettingPage(SettingPageViewModel viewModel)
    {
        InitializeComponent();
        _vm = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        _vm.Refresh();
    }

    private async void Embedding_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(EmbeddingTestPage));
    }
}