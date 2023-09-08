using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AquaChat.Models;

namespace AquaChat.Databases;

public class ChatDao
{

    private readonly SQLiteAsyncConnection _connection;

    public ChatDao(SQLiteAsyncConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<Chat>> ListChatsAsync()
    {
        return await _connection.Table<Chat>()
            .OrderByDescending(e => e.Id)
            .ToListAsync();
    }

    public async Task<Chat> CreateNewItem()
    {
        Chat chat = new Chat
        {
            Title = "No Title",
            LastMessage = "",
            Created = DateTime.Now
        };
        var id = await _connection.InsertAsync(chat);
        return await _connection.GetAsync<Chat>(id);
    }

    public async Task DeleteAsync(long id)
    {
        await _connection.DeleteAsync<Chat>(id);
    }

    public async Task Update(long id, Action<Chat> updateAction)
    {
        
        var chat = await _connection.GetAsync<Chat>(id);
        if (chat is null)
        {
            return;
        }
        updateAction.Invoke(chat);
        await _connection.UpdateAsync(chat);
    }

}