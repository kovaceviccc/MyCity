using MAUI_Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Library.Models.Incoming;

public class BasicEventModel
{
    public string Id { get; set; }
    public EventTypeEnum EventType{ get; set; }
    public bool Responded { get; set; }
}
