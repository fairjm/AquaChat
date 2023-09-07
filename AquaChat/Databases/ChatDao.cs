using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        await _connection.DeleteAsync(id);
    }

    public async Task UpdateTitle(long id, string title)
    {
        var chat = await _connection.GetAsync<Chat>(id);
        if (chat is null)
        {
            return;
        }
        chat.Title = title;
        await _connection.UpdateAsync(chat);
    }

    public async Task UpdateLastMessage(long id, string lastMessage)
    {
        var chat = await _connection.GetAsync<Chat>(id);
        if (chat is null)
        {
            return;
        }
        chat.LastMessage = lastMessage;
        await _connection.UpdateAsync(chat);
    }
}