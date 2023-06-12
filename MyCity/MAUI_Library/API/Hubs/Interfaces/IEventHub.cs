using MAUI_Library.Models.Incoming;
using MAUI_Library.Models.OutgoingDto;
using Microsoft.AspNetCore.SignalR.Client;

namespace MAUI_Library.API.Hubs.Interfaces;

public interface IEventHub
{
    public HubConnection Connection { get;}
    Task<bool> AddEvent(EventDto eventDto);
    Task<bool> LikeEvent(string eventId);
    Task<bool> CommentEvent(CommentDto commentDto);
    Task<bool> DeleteEvent(string eventId);
    Task<bool> RespondToEventAsync(string eventId);
    Task<bool> ConnectAsync();
    Task<bool> DisconnectAsync();
}
