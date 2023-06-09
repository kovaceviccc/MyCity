using Entities.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models;

public class BasicEventModel
{
    public required string Id { get; set; }
    public required EventTypeEnum EventType { get; set; }
    public required bool Responded { get; set; }
}
