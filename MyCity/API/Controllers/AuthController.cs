using API.Extensions;
using API.Models;
using API.Models.DTOs;
using Entities.DbSet;
using Entities.Domain.Enums;
using Entities.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;
    private readonly IUserRepository _userRepository;

    public AuthController(
        IAuthRepository authRepository,
        IUserRepository userRepository)
    {
        _authRepository = authRepository;
        _userRepository = userRepository;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto request)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("Invalid payload");
        }

        var user = await _authRepository.FindByEmailAsync(request.Email);

        if(user is not  null)
        {
            return StatusCode(StatusCodes.Status400BadRequest,new AuthResult
            {
                Result = false,
                Errors = new List<string>()
                {
                    "User with this email already exists"
                }
            });
        }

        User newUser = request.Map();

        var isCreated = await _authRepository.RegisterUser(newUser, request.Password);

        if(!isCreated)
        {
            return StatusCode(StatusCodes.Status400BadRequest,"Failed to create user");
        }

        (string jwtToken, string refreshToken) = await _authRepository.GenerateTokens(newUser);

        if(jwtToken is null || refreshToken is null) 
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Failed to create user");
        }

        return StatusCode(StatusCodes.Status201Created,new AuthResult
        {
            Result = true,
            Token = jwtToken,
            RefreshToken = refreshToken
        });
    }

    //[HttpPost]
    //[Route("AddRoles")]
    //public async Task<IActionResult> AddRoles(string id)
    //{

    //    await _authRepository.CreateRole(RolesEnum.User.ToString());
    //    await _authRepository.CreateRole(RolesEnum.Admin.ToString());
    //    await _authRepository.CreateRole(RolesEnum.AuthorizedPersonel.ToString());

    //    var res = await _userRepository.AddToRoles(id);

    //    if (!res) return StatusCode(StatusCodes.Status400BadRequest, "Failed");

    //    return StatusCode(StatusCodes.Status201Created, "Success");
    //}

    [HttpPost]
    [Route("CreateRole")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var result = await _authRepository.CreateRole(roleName);

        if(result is true)
        {
            return Ok($"Role {roleName} successfully added");
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add role");
        }

    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody]UserLoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResult
            {
                Errors = new List<string>
                {
                    "Invalid payload"
                },
                Result = false
            });
        }

        var existingUser = await _authRepository.FindByEmailAsync(request.Email);

        if (existingUser is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest,new AuthResult
            {
                Result = false,
                Errors = new List<string>{
                    "Invalid payload"
                },
            });
        }

        var isCorrect = await _authRepository.ValidatePassword(existingUser, request.Password);

        if (!isCorrect)
        {
            return BadRequest(new AuthResult
            {
                Result = false,
                Errors = new List<string> { "Invalid credentials" }
            });
        }

        (string jwtToken, string refreshToken) = await _authRepository.GenerateTokens(existingUser);

        return Ok(new AuthResult()
        {
            Result = true,
            Token = jwtToken,
            RefreshToken = refreshToken
        });
    }

    [HttpPost]
    [Route("ChangePassword")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordRequestDto request)
    {
        if (!ModelState.IsValid) 
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Invalid payload");
        }

        string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;

        var user = await _authRepository.FindByEmailAsync(userId);

        if (user is null) return StatusCode(StatusCodes.Status401Unauthorized);

        var success = await _userRepository.ChangePassword(user, request.OldPassword, request.NewPassword);

        if(!success)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Invalid password");
        }

        return StatusCode(StatusCodes.Status202Accepted);
    }

    [HttpGet]
    [Route("GetUserInfo")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public async Task<IActionResult> GetUserInfo()
    {
        string userEmail = HttpContext.User.Claims.FirstOrDefault(c=> c.Type == ClaimTypes.NameIdentifier)?.Value!;

        var user = await _authRepository.FindByEmailAsync(userEmail);

        if(user is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        return StatusCode(StatusCodes.Status202Accepted,new BasicUserInfoDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.DisplayName,
            Email = user.Email!
        });
    }

    [HttpPost]
    [Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody]TokenRequest tokenRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResult
            {
                Errors = new List<string> { "Invalil parameters" },
                Result = false
            });
        }

        var validated = await _authRepository.ValidateTokens(tokenRequest.Token, tokenRequest.RefreshToken);

        if(!validated)
        {
            return Ok(new AuthResult
            {
                Errors = new List<string>
                {
                    "Invalid tokens"
                },
                Result = false
            });
        }

        (string jwtToken, string refreshToken) = await _authRepository.GenerateTokens(tokenRequest.RefreshToken);
         
        if (jwtToken.IsNullOrEmpty() || refreshToken.IsNullOrEmpty())
        {
            return StatusCode(StatusCodes.Status400BadRequest,new AuthResult
            {
                Errors = new List<string>
                {
                    "Invalid tokens"
                },
                Result = false
            });
        }

        return StatusCode(StatusCodes.Status201Created,new AuthResult
        {
            Token = jwtToken,
            RefreshToken = refreshToken,
        });
    }

    [HttpGet]
    [Route("Admin/Roles/GetAll")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllRoles()
    {
        try
        {
            var roles = await _authRepository.GetAllRoles();
            return StatusCode(StatusCodes.Status200OK,roles);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            //log error
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Route("Admin/Roles/GetAllRequired")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllRequiredRole()
    {

        try
        {
            string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!;

            if (userId is null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            var roles = await _authRepository.GetAllRequredRoles(userId);
            return StatusCode(StatusCodes.Status200OK, roles);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            //log error
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }
    }

    [HttpPost]
    [Route("Admin/Roles/SubmitRoleRequest")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> SubmitRequest([FromBody]string roleId)
    {
        try
        {
            string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!;

            (var res, string error) =await _authRepository.SubmitRoleRequest(roleId, userId);

            if(!res) return StatusCode(StatusCodes.Status400BadRequest, error);

            return StatusCode(StatusCodes.Status201Created, error);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            return StatusCode(StatusCodes.Status400BadRequest, error);
        }
    }
}
