using MAUI_Library.Models.Enums;

namespace DesktopUI.Models;

public class BasicEventDisplayModel
{
    public required string Id { get; set; }
    public EventTypeEnum EventType { get; set; }
    public bool Responded { get; set; }
}
