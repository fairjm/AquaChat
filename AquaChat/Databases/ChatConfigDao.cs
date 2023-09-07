using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquaChat.Models;

namespace AquaChat.Databases;

public class ChatConfigDao
{

    private readonly SQLiteAsyncConnection _connection;

    public ChatConfigDao(SQLiteAsyncConnection connection)
    {
        _connection = connection;
    }

    public async Task<ChatConfig?> GetByChatId(long chatId)
    {
        return await _connection.Table<ChatConfig>()
            .Where(e => e.ChatId == chatId)
            .FirstAsync();
    }

    public async Task SaveOrReplaceConfig(ChatConfig chatConfig)
    {
        await _connection.InsertOrReplaceAsync(chatConfig);
    }

    public async Task DeleteByChatId(long chatId)
    {
        var chatConfig = await GetByChatId(chatId);
        if (chatConfig is null)
        {
            return;
        }
        await _connection.DeleteAsync(chatConfig.Id);
    }

}