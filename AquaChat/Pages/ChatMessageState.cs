using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquaChat.Models;

namespace AquaChat.Pages;

/**
 * this class is used to pass data to blazor web view
 */
public class ChatMessageState
{
    public Chat? CurrentChat { get; set; }
}