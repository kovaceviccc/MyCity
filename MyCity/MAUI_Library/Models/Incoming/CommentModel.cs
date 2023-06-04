using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Library.Models.Incoming;

public class CommentModel
{
    public string EventId { get; set; }
    public string PublisherUserName { get; set; }
    public string Content { get; set; }
    public DateTime DateCreated { get; set; }
}
