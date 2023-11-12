using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquaChat.Models;

namespace AquaChat.Messages;

class ChatConfigUpdateMessage
{
    public ChatConfig? Config { get; set; }
}