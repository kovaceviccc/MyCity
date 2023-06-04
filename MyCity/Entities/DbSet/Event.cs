using Entities.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbSet;

public class Event : BaseEntity
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public EventTypeEnum EventType{ get; set; }
    public User Publisher { get; set; } = default!;
    public List<Comment> Comments { get; set; } = new();
    public List<Like> Likes { get; set; } = new();
}
