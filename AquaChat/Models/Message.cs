using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace AquaChat.Models;

public partial class Message : ObservableObject
{
    public const int TypeAi = 0;
    public const int TypeHuman = 1;
    public const int TypeSystem = 2;

    [ObservableProperty]
    [property: PrimaryKey]
    [property: AutoIncrement]
    private long _id;

    [ObservableProperty]
    private long _chatId;

    [ObservableProperty] private int _messageType;

    [ObservableProperty]
    private string? _content;

    [ObservableProperty]
    private string? _referenceContent;

    [ObservableProperty]
    private string? _extraContent;

    [ObservableProperty]
    private DateTime? _created;
}