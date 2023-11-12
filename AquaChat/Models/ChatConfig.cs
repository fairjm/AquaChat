using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace AquaChat.Models;

public partial class ChatConfig : ObservableObject
{
    [ObservableProperty]
    [property: PrimaryKey]
    [property: AutoIncrement]
    private long _id;

    [property: Unique]
    public long ChatId { get; set; }

    [ObservableProperty]
    private string? _systemMessage;

    [ObservableProperty]
    private int? _lastMessageNum;

    [ObservableProperty]
    private int? _maxTokens;

    [ObservableProperty]
    private double? _temperature;

    [ObservableProperty]
    private double? _presencePenalty;

    [ObservableProperty]
    private double? _frequencyPenalty;

    [ObservableProperty] 
    private DateTime? _created;

}