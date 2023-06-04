using Entities.DbSet;
using Entities.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Entities.Repository;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;
    //private readonly ILogger _logger;

    public EventRepository(
        ApplicationDbContext dbContext)
        //ILogger logger)
    {
        _context = dbContext;
        //_logger = logger;
    }


    public async Task<bool> AddEvent(Event eventToAdd)
    {
        try
        {
            await _context.Events.AddAsync(eventToAdd);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            //_logger.LogError($"Failed to add event, Error {ex.Message}");
            return false;
        }   
    }

    public async Task<bool> BlockEvent(string eventId)
    {
        try
        {
            var e = await _context.Events.FindAsync(eventId);

            if(e is null)
            {
                //_logger.LogInformation($"Failed to find event {eventId}");
                return false;
            }

            e.Archived = true;
            _context.Events.Update(e);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            //_logger.LogError($"Failed to block event {eventId}, Error {ex.Message}");
            return false;
        }
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync(TimeSpan timeSpan)
    {
        try
        {

            DateTime startTime = DateTime.Now.Subtract(timeSpan);

            List<Event> events = await _context.Events
                                                .Where(e => e.DateCreated >= startTime && e.Archived == false)
                                                .Include(x=> x.Publisher)
                                                .Include(x=> x.Likes)
                                                .ThenInclude(x=> x.User)
                                                .ToListAsync();
             return events;
        }
        catch (Exception ex)
        {
            string s = ex.Message;
            //_logger.LogError($"Failed to fetch all events in the timespan: {timeSpan}, Error {ex.Message}");
            return Enumerable.Empty<Event>();
        }
    }

    public async Task<Event?> GetById(string eventId)
    {
        try
        {
            var result = await _context.Events.Where(x=> x.Id == eventId)
                                                .Include(x=> x.Likes)
                                                .ThenInclude(x=> x.User)
                                                .Include(x=> x.Publisher)
                                                .FirstOrDefaultAsync();
            return result;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> LikeEvent(string eventId, string userId)
    {

        var likeExists = await _context.
                            Likes.
                            Where(x => x.User.Id == userId && x.Event.Id == eventId).
                            FirstOrDefaultAsync();

        if (likeExists is not null)
        {
            _context.Likes.Remove(likeExists);
            await _context.SaveChangesAsync();
            return true;
        }

        var eventToLike = await _context.Events.FindAsync(eventId);
        var user = await _context.Users.FindAsync(userId);

        if (eventToLike is null || user is null) return false;

        Like like = new Like
        {
            Event = eventToLike,
            User = user
        };

        await _context.Likes.AddAsync(like);
        await _context.SaveChangesAsync(); 

        return true;
    }

    public async Task<bool> RemoveEvent(string eventId) // for user to remove event or for admin
    {
        try
        {
            var e = await _context.Events.FindAsync(eventId);
            if(e is null)
            {
                //_logger.LogWarning($"Failed to remove event with id: {eventId}");
                return false;
            }

            _context.Events.Remove(e);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            //_logger.LogError($"Failed to acces database in RemoveEvent method, Error {ex.Message}");
            return false;
        }
    }

    public async Task<(bool, Comment)> AddComment(string eventId, string content, string userId)
    {

        try
        {
            var user = await _context.Users.FindAsync(userId);
            var eventToComment = await _context.Events.FindAsync(eventId);

            if(user is null || eventToComment is null) return (false, new Comment());

            var comment = new Comment
            {
                User = user, 
                Event = eventToComment,
                Content = content
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return (true, comment);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            //logg error 
            return (false, new Comment());
        }
    }

    public async Task<IEnumerable<Comment>> GetAllComments(string eventId)
    {
        try
        {
            var result = await _context.Comments
                .Where(x=> x.Archived == false)
                .Include(x => x.Event)
                .Include(x=> x.User)
                .Where(x=> x.Event.Id == eventId)
                .ToListAsync();

            return result;
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            return Enumerable.Empty<Comment>();
        }
    }
}
