using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Library.Models;

public class LoggedInUserModel : ILoggedInUserModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    public void ResetUserModel()
    {
        UserName = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
    }
}
