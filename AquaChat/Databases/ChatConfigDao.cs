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
        await _connection.ExecuteAsync("delete from chat_config where chat_id=?", chatId);
    }

}