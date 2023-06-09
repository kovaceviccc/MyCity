using Entities.DbSet;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Repository.Interfaces;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllEventsAsync(TimeSpan timeSpan);
    Task<IEnumerable<BasicEventModel>> GetAllEventsAsync();
    Task<bool> AddEvent(Event @event);
    Task<bool> RemoveEvent(string eventId);
    Task<bool> BlockEvent(string eventId); //Admin only
    Task<bool> LikeEvent(string eventId, string userId);
    Task<Event?> GetById(string eventId);
    Task<(bool, Comment)> AddComment(string eventId, string content, string userId);
    Task<IEnumerable<Comment>> GetAllComments(string eventId);
}
