using MAUI_Library.Models.Enums;

namespace DesktopUI.Models;

public class BasicEventDisplayModel
{
    public required string Id { get; set; }
    public required EventTypeEnum EventType { get; set; }
    public required DateTime DateCreated{ get; set; }
    public required string EventTitle { get; set; }
    public required string EventDescription { get; set; }
    public required bool Responded { get; set; }
    //public PersonelDisplayModel PersonelResponded { get; set; }
}
