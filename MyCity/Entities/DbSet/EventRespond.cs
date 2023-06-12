using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbSet;

public class EventRespond : BaseEntity
{
    public required User User { get; set; }
    public required Event Event{ get; set; }
}
