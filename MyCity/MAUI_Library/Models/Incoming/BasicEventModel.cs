using MAUI_Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Library.Models.Incoming;

public class BasicEventModel
{
    public required string Id { get; set; }
    public required EventTypeEnum EventType { get; set; }
    public required DateTime DateCreated { get; set; }
    public required string EventTitle { get; set; }
    public required string EventDescription { get; set; }
    public required bool Responded { get; set; }
    //public PersonelModel PersonelResponded { get; set; }
}
