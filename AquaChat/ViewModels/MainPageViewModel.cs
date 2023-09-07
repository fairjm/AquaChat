using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquaChat.Utils;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.SemanticKernel;

namespace AquaChat.ViewModels;

internal partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _inputText;
    [ObservableProperty]
    private string? _outputText;

    [ObservableProperty]
    private bool _pending;

    [RelayCommand]
    public async Task SummaryTask()
    {

        if (InputText is null || (InputText.Trim().Length == 0))
        {
            await Toast.Make("input is empty").Show(); 
            return;
        }

        var kernel = SemanticKernelHolder.GetKernel();
        if (kernel is null)
        {
            await Toast.Make("please config setting first").Show();
            return;
        }

        Pending = true;

        var prompt = @"{{$input}} 

One line TLDR with the fewest words.";

        var summarize = kernel.CreateSemanticFunction(prompt, maxTokens: 100);
        var skContext = await summarize.InvokeAsync(InputText);
        Pending = false;
        OutputText = skContext.Result;
    }
}