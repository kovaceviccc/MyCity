using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbSet;

public class Like : BaseEntity
{
    public User User { get; set; } = default!;
    public Event Event  { get; set; } = default!;
}
