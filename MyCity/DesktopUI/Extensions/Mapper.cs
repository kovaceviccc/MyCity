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
                Responded = e.Responded
            });
        }

         return result;
    } 
}
