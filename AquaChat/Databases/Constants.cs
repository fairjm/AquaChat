namespace AquaChat.Databases;

public class Constants
{
    public const string DatabaseFilename = "AquaChat.db3";

    public const string MemoryDatabaseFilename = "AquaChat_memory.db3";

    public const SQLite.SQLiteOpenFlags Flags =
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

    public static string MemoryDatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, MemoryDatabaseFilename);
}