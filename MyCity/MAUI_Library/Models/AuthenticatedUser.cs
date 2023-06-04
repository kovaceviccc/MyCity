using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Library.Models;

public class AuthenticatedUser
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
