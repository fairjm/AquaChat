using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AquaChat.Models;

namespace AquaChat.Databases;

public class MessageDao
{

    private readonly SQLiteAsyncConnection _connection;

    public MessageDao(SQLiteAsyncConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<Message>> ListMessageByChatIdAsync(long chatId)
    {
        return await _connection.Table<Message>()
            .Where(e => e.ChatId == chatId)
            .OrderBy(e => e.Id)
            .ToListAsync();
    }

    public async Task<List<Message>> ListLastN(long chatId, int num)
    {
        return await _connection.Table<Message>()
            .Where(e => e.ChatId == chatId)
            .OrderByDescending(e => e.Id)
            .Take(num)
            .ToListAsync();
    }

    public async Task<Message> SaveNewMessage(Message message)
    {
        var id = await _connection.InsertAsync(message);
        return await _connection.GetAsync<Message>(id);
    }

    public async Task DeleteMessagesByChatId(long chatId)
    {
        await _connection.ExecuteAsync("delete from message where chat_id=?", chatId);
    }

}