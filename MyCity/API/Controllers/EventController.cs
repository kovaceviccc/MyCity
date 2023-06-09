using API.Extensions;
using API.Models.DTOs;
using Entities.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventRepository _eventRepository;
    private readonly IAuthRepository _authRepository;

    public EventController(
        IEventRepository eventRepository,
        IAuthRepository authRepository)
    {
        _eventRepository = eventRepository;
        _authRepository = authRepository;
    }


    [HttpGet]
    [Route("GetAll/{timeSpanString}")]
    public async Task<IActionResult> GetAll([FromRoute]string timeSpanString)
    {
        
        var success = TimeSpan.TryParse(timeSpanString, out TimeSpan timeSpan);

        if(!success)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }    
        var events = await _eventRepository.GetAllEventsAsync(timeSpan);

        var result = events.Map();

        return result is null ? StatusCode(StatusCodes.Status204NoContent) : StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost]
    [Route("Create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public async Task<IActionResult> Create([FromBody] EventDto eventDto)
    {
        string authHeader = HttpContext.Request.Headers["Authorization"]!;
        string jwtToken = authHeader.Replace("Bearer ", "");
        string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;

        var user = await _authRepository.FindByEmailAsync(userId);

        if(user is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        var eventToAdd = Mapper.Map(eventDto, user);
        var result = await _eventRepository.AddEvent(eventToAdd);

        if(!result)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet]
    [Route("{eventId}")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetById([FromRoute]string eventId)
    {
        var result = await _eventRepository.GetById(eventId);

        if(result is null) return StatusCode(StatusCodes.Status400BadRequest);

        var eventModel = result.Map();

        return StatusCode(StatusCodes.Status200OK, eventModel);
    }

    [HttpGet]
    [Route("Comments/{eventId}")]
    public async Task<IActionResult> GetAllComments(string eventId)
    {
        var comments = await _eventRepository.GetAllComments(eventId);

        var result = comments.Map();

        if (result is null) return StatusCode(StatusCodes.Status204NoContent);

        return StatusCode(StatusCodes.Status200OK, result);
    }


    [HttpGet]
    [Route("Admin/GetAll/{timeSpanString}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
    public async Task<IActionResult> GetAllEvents(string timeSpanString)
    {
        var success = TimeSpan.TryParse(timeSpanString, out TimeSpan timeSpan);

        if (!success)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        try
        {
            var result = await _eventRepository.GetAllEventsAsync();

            return StatusCode(StatusCodes.Status200OK, result);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            //log error 

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
