using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquaChat.Models;
using AquaChat.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AquaChat.ViewModels;
public partial class SettingPageViewModel : ObservableObject
{
    [ObservableProperty] private string? _openAiPrefix;

    [ObservableProperty] private string? _secret;

    [ObservableProperty] private string? _model;

    private readonly SemanticKernelConfigService _semanticKernelConfigService;

    public SettingPageViewModel(SemanticKernelConfigService semanticKernelConfigService)
    {
        _semanticKernelConfigService = semanticKernelConfigService;

    }

    [RelayCommand]
    public void Refresh()
    {
        var appConfig = _semanticKernelConfigService.GetConfig();
        OpenAiPrefix = appConfig.OpenAiPrefix;
        Secret = appConfig.Secret;
        Model = appConfig.Model;
    }

    [RelayCommand]
    public void SaveConfig()
    {
        var config = new AppConfig
        {
            Model = Model,
            OpenAiPrefix = OpenAiPrefix,
            Secret = Secret
        };
        _semanticKernelConfigService.UpdateConfig(config);
    }


}