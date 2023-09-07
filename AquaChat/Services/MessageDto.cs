using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaChat.Services;

public class MessageDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public int Type { get; set; }

    public DateTime? Created { get; set; }
}