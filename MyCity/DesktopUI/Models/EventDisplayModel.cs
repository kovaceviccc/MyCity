using MAUI_Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopUI.Models;

public class EventDisplayModel
{
    public required string Id { get; set; }
    public required EventTypeEnum EventType{ get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required bool Responded { get; set; }
}
