using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace AquaChat.Models;

public partial class Message : ObservableObject
{
    public static readonly int TypeAi = 0;
    public static readonly int TypeHuman = 1;
    public static readonly int TypeSystem = 2;

    [ObservableProperty]
    [property: PrimaryKey]
    [property: AutoIncrement]
    private int _id;

    [ObservableProperty]
    private int _chatId;

    [ObservableProperty] private int _messageType;

    [ObservableProperty]
    private string? _content;

    [ObservableProperty]
    private string? _referenceContent;

    [ObservableProperty]
    private DateTime? _created;
}