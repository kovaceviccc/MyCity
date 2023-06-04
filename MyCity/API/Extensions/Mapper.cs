using API.Models.DTOs;
using Entities.DbSet;
using System.Runtime.CompilerServices;

namespace API.Extensions;

public static class Mapper
{
    public static User Map(this UserRegistrationRequestDto request)
    {
        return new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.DisplayName,
            DateOfBirth = request.DateOfBirth,
            DisplayName = request.DisplayName,
        };
    }

    public static Event Map(this EventDto eventDto, User user) 
    {
        return new Event
        {
            Title = eventDto.Title,
            Description = eventDto.Description,
            Latitude = eventDto.Latitude,
            Longitude = eventDto.Longitude,
            Publisher = user,
            EventType = eventDto.EventType
        };
    }

    public static EventResponseDto Map(this Event e)
    {

        List<string> likes = new();

        foreach (var item in e.Likes)
        {
            likes.Add(item.User.Id);
        }

        return new EventResponseDto
        {
            Id = e.Id,
            PublisherId = e.Publisher.Id,
            PublisherUserName = e.Publisher.DisplayName,
            Title = e.Title,
            Description = e.Description,
            DateCreated = e.DateCreated,
            Latitude = e.Latitude,
            Longitude = e.Longitude,
            EventType = e.EventType,
            Likes = likes
        };
    }

    public static List<EventResponseDto> Map(this IEnumerable<Event> events)
    {
        List<EventResponseDto> result = new();

        foreach (var e in events)
        {
            List<string> likes = new();

            foreach (var item in e.Likes)
            {
                likes.Add(item.User.Id);
            }

            result.Add(new EventResponseDto
            {
                Id = e.Id,
                PublisherId = e.Publisher.Id,
                Title = e.Title,
                Description = e.Description,
                DateCreated = e.DateCreated,
                Latitude = e.Latitude,
                Longitude = e.Longitude,
                EventType = e.EventType,
                Likes = likes
                
            });
        }

        return result;
    }

    public static IEnumerable<CommentResponseDto> Map(this IEnumerable<Comment> comments)
    {
        List<CommentResponseDto> result = new();

        foreach (var comment in comments)
        {
            result.Add(new CommentResponseDto
            {
                EventId = comment.Event.Id,
                PublisherUserName = comment.User?.UserName!,
                Content = comment.Content,
                DateCreated = comment.DateCreated
            });
        }

        return result;
    }
}

