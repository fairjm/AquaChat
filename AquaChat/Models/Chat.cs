using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace AquaChat.Models;

public partial class Chat : ObservableObject
{
    [ObservableProperty]
    [property: PrimaryKey]
    [property: AutoIncrement]
    private long _id;

    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private string? _lastMessage;

    [ObservableProperty] 
    private DateTime? _created;

}