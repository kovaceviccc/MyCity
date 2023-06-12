using DesktopUI.Models;
using MAUI_Library.Models.Incoming;
using System.Collections.ObjectModel;

namespace DesktopUI.Extensions;

public static class Mapper
{

    public static ObservableCollection<BasicEventDisplayModel> Map(this IEnumerable<BasicEventModel> events)
    {
        ObservableCollection<BasicEventDisplayModel> result = new();

        foreach(var e in events)
        {
            result.Add(new BasicEventDisplayModel
            {
                Id = e.Id,
                EventType = e.EventType,
                DateCreated = e.DateCreated,
                EventTitle = e.EventTitle,
                EventDescription = e.EventDescription,
                Responded = e.Responded,
                //PersonelResponded = new PersonelDisplayModel
                //{
                //    Id = e.PersonelResponded.Id,
                //    FirstName = e.PersonelResponded.FirstName,
                //    LastName = e.PersonelResponded.LastName,
                //    Email = e.PersonelResponded.Email,
                //    Roles = e.PersonelResponded.Roles
                //},
            });
        }

         return result;
    }
    
    public static ObservableCollection<RoleRequestDisplayModel> Map(this IEnumerable<RoleRequestModel> roleRequests)
    {
        ObservableCollection<RoleRequestDisplayModel> result = new();


        foreach(var roleRequest in roleRequests)
        {
            result.Add(new RoleRequestDisplayModel
            {
                Id = roleRequest.Id,
                RoleName = roleRequest.RoleName,
                DateCreated = roleRequest.DateCreated,
                UserUserName = roleRequest.User.UserName,
                UserId = roleRequest.User.Id,
                UserFirstName = roleRequest.User.FirstName,
                UserLastName = roleRequest.User.LastName,
                UserEmail = roleRequest.User.Email
            });
        }

        return result;

    }
}
