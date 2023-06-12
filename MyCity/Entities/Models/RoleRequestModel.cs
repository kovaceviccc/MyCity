using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models;

public class RoleRequestModel
{
    public required string Id { get; set; }
    public required string RoleName  { get; set; }
    public required DateTime DateCreated {  get; set; }
    public required BasicUserModel User { get; set; }
}
