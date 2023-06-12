using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopUI.Models;

public class RoleRequestDisplayModel
{
    public string Id { get; set; }
    public string RoleName { get; set; }
    public DateTime DateCreated { get; set; }
    public string UserId { get; set; }
    public string UserUserName { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public string UserEmail { get; set; }
}
