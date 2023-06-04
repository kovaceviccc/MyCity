using MAUI_Library.Models.OutgoingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Library.API.Interfaces;

public interface IAuthEndpoint
{
    Task<bool> Register(RegisterRequestDto request);
    Task<bool> ChangePassword(ChangePasswordRequestDto request);
}
