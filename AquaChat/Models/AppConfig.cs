namespace AquaChat.Models;

public struct AppConfig
{
    public string? OpenAiPrefix { get; set; }
    public string? Secret { get; set; }

    public string? Model { get; set; }
}