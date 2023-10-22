using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaChat.Messages;

class ChatUpdateTitleMessage
{
    public long ChatId { get; set; }
    public string Title { get; set; } = string.Empty;
}