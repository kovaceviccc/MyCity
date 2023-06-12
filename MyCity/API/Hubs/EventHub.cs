using API.Extensions;
using API.Models.DTOs;
using Entities.Domain.Enums;
using Entities.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.Hubs;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AllowAnonymous]
public class EventHub : Hub
{
	private readonly IEventRepository _eventRepository;
    private readonly IAuthRepository _authRepository;


    [AllowAnonymous]
    public override Task OnConnectedAsync()
    {

        string userId;

        try
        {
            userId = Context.GetHttpContext()!.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
        }
        catch (Exception ex) 
        {
            string error = ex.Message;
            throw;
        }

        if (!Context.User!.Identity!.IsAuthenticated)
        {
            userId = "peraperapera";
        }
        return base.OnConnectedAsync();
    }


    public override Task OnDisconnectedAsync(Exception? exception)
    {
        string userId;


        try
        {
            userId = Context.GetHttpContext()!.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            throw;
        }

        if (!Context.User!.Identity!.IsAuthenticated)
        {
            userId = "peraperapera";
        }


        return base.OnDisconnectedAsync(exception);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task EventRecived(EventDto eventDto)
    {
		try
		{
			string userId = Context.GetHttpContext()!.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;

			var user = await _authRepository.FindByEmailAsync(userId);

			if (user is null) return;

			var eventToAdd = Mapper.Map(eventDto, user);

			var result = await _eventRepository.AddEvent(eventToAdd);

            if (!result) return;


            await Clients.All.SendAsync("EventRecived", new 
            {
                Id = eventToAdd.Id,
                Title = eventToAdd.Title,
                Description = eventToAdd.Description,
                PublisherId = eventToAdd.Publisher.Id,
                DateCreated = eventToAdd.DateCreated,
                Latitude = eventToAdd.Latitude,
                Longitude = eventToAdd.Longitude,
                EventType = eventToAdd.EventType
            });
        }
		catch (Exception ex)
		{
			string message = ex.Message;
			string inner = ex.InnerException!.Message;

		}
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task LikeEvent(string eventId)
    {

        try
        {
            string userId = Context.GetHttpContext()!.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!;
            if (eventId is null || userId is null) return;
            var result = await _eventRepository.LikeEvent(eventId, userId);

            if(result)
            {
                await Clients.All.SendAsync("EventLiked", new
                {
                    UserId = userId,
                    EventId = eventId
                });
            }

        }
        catch (Exception ex)
        {
            string error = ex.Message;
            string inner = ex.InnerException!.Message;
            //log error;
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task CommentEvent(CommentDto commentDto)
    {
        try
        {
            string userId = Context.GetHttpContext()!.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!;
            if (commentDto is null || userId is null) return;

            var result = await _eventRepository.AddComment(commentDto.EventId, commentDto.Content, userId);

            if (!result.Item1) return;

            await Clients.All.SendAsync("EventCommented", new
            {
                EventId = result.Item2.Event.Id,
                PublisherUserName   = result.Item2.User.UserName,
                DateCreated = result.Item2.DateCreated,
                Content = result.Item2.Content
            });
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            string inner = ex.InnerException!.Message;
            //log error
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "AuthorizedPersonel")]
    public async Task RespondToEvent(string eventId)
    {
        try
        {
            string userId = Context.GetHttpContext()!.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!;

            if (userId is null || eventId is null) return;

            var result = await _eventRepository.RespondToEmergencyEvent(eventId, userId);

            if (!result) return;

            await Clients.All.SendAsync("EventResponded", eventId, userId);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public EventHub(IEventRepository eventRepository,
					IAuthRepository authRepository)
    {
		_eventRepository = eventRepository;
		_authRepository = authRepository;
    }
}
