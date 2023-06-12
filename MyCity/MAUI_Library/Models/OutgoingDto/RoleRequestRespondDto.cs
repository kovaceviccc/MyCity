using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Library.Models.OutgoingDto;

public class RoleRequestRespondDto
{
    public required string RequestId { get; set; }
    public required string UserId { get; set; }
    public required string RoleId { get; set; }
    public required bool Approved { get; set; }
}
