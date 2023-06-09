using MAUI_Library.Models.Incoming;
using MAUI_Library.Models.OutgoingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Library.API.Interfaces;

public interface IEventEndpoint
{
    Task<IEnumerable<EventModel>> GetAllEventsAsync(TimeSpan timeSpan);
    Task<IEnumerable<EventModel>> GetUserEventsAsync(); //samo za trenutnog korisnika
    Task<bool> PostEventAsync(EventDto eventDto);
    Task<bool> DeleteEventAsync(string id);
    Task<bool> UpdateEventAsync(string id, string title, string description);
    Task<(bool,EventModel)> GetEventByIdAsync(string id);
    Task<IEnumerable<CommentModel>> GetAllComments(string eventId);
}
