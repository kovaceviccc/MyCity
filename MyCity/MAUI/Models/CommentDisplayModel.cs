using Android.Widget;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MAUI.Models;

public partial class CommentDisplayModel : ObservableObject
{
    public string EventId { get; set; }
    public string PublisherUserName { get; set; }
    public DateTime DateCreated { get; set; }
	public string Content { get; set; }

    [ObservableProperty]
    private string dateCreatedString;
}
