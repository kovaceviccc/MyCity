using MAUI_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Library.API.Interfaces;

public interface IApiHelper
{
    Task<bool> Authenticate(string username, string password);
    Task<bool> RefreshTokens();
    Task<bool> GetLoggedInUserInfo();
    HttpClient ApiClient { get; }

}
