using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI.Helpers;
using MAUI.Models;
using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Helpers;
using MAUI_Library.Models.Incoming;
using MAUI_Library.Models.OutgoingDto;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace MAUI.ViewModel;

[QueryProperty("EventId", "Event")]
public partial class EventDetailsPageViewModel : ObservableObject
{

    [ObservableProperty]
    private Entry entry;

    [ObservableProperty]
    public EventDisplayModel eventDisplayModel;

    [ObservableProperty]
    private string formattedDateCreated = string.Empty;

    [ObservableProperty]
    private bool liked = false;

    [ObservableProperty]
    private ObservableCollection<CommentDisplayModel> comments = new();

    [ObservableProperty]
    private string likeButtonImageSource;

    [ObservableProperty]
    private string comment;

    [ObservableProperty]
    private string eventId;
    
    private readonly IEventHub _eventHub;
    private readonly IEventEndpoint _eventEndpoint;
    private System.Timers.Timer timer;

    public async Task OnAppearingAsync()
    {

        //razmisli da ovo uradis u MainPage pa da prosledis ceo dogadjaj odje kao do sad!
        (bool result, EventModel eventModel)= await _eventEndpoint.GetEventByIdAsync(EventId);

        if (!result)
        {
            await Shell.Current.Navigation.PopAsync();
            return;
        }

        EventDisplayModel = eventModel.MapToDisplayModel();

        string userId = await UserSessionManager.GetUserId();
        if (userId == null) userId = "";

        Liked = EventDisplayModel.Likes.Contains(userId);

        FormattedDateCreated = FormatDateCreated(EventDisplayModel.DateCreated);
        await GetAllCommentsAsync();
        await ResetUIAsync();
    }

    private async Task ResetUIAsync()
    {
        string userId = await UserSessionManager.GetUserId();
        Liked = EventDisplayModel.Likes.Contains(userId);

        if (Liked)
        {
            LikeButtonImageSource = "heartfull.svg";
            return;
        }

        LikeButtonImageSource = "heartempty.svg";
    }

    [RelayCommand]
    public async Task AddCommentAsync()
    {
        if (Comment.Length <= 1) return;

        var result = await _eventHub.CommentEvent(new CommentDto
        {
            EventId = EventDisplayModel.Id,
            Content = Comment
        });

        if (!result)
        {
            await Shell.Current.DisplayAlert("Error", "Comment not added, please try again!", "Ok");
            return;
        }

        Comment = "";
        Entry.Unfocus();
    }

    [RelayCommand]
    public async Task GetAllCommentsAsync()
    {
        var commentModels = await _eventEndpoint.GetAllComments(EventDisplayModel.Id);
        Comments = commentModels.Map();

        foreach(var comment in Comments)
        {
            comment.DateCreatedString = FormatDateCreated(comment.DateCreated);
        }
        SortComments();
    }

    [RelayCommand]
    public async Task LikeAsync()
    {
        var result = await _eventHub.LikeEvent(EventDisplayModel.Id);

        if(!result) await Shell.Current.DisplayAlert("Error", "Please try again!", "Ok");
    }

    private void SortComments()
    {
        IEnumerable<CommentDisplayModel> sortedComments = new List<CommentDisplayModel>(Comments);

        Comments = new(sortedComments.OrderByDescending(x => x.DateCreated).ToList());
    }

    private async Task CommentAddedAsync(CommentDisplayModel comment)
    {
        if(comment.EventId == EventDisplayModel.Id)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                comment.DateCreatedString = FormatDateCreated(comment.DateCreated);
                Comments.Add(comment);
                SortComments();
            });

        }
    }

    private async Task LikeAddedAsync(LikeDisplayModel likeModel)
    {
        if (likeModel.EventId == EventDisplayModel.Id)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if(EventDisplayModel.Likes.Contains(likeModel.UserId))
                {
                    EventDisplayModel.Likes.Remove(likeModel.UserId);
                }
                else
                {
                    EventDisplayModel.Likes.Add(likeModel.UserId);
                }
                await ResetUIAsync();
            });
        }
    }

    private string FormatDateCreated(DateTime date)
    {
        TimeSpan timeElapsed = DateTime.UtcNow - date;

        if(timeElapsed.TotalSeconds < 0) // error handling
        {
            return "0 sec";
        }
        else if (timeElapsed.TotalSeconds < 60) // Less than a minute ago
        {
            return $"{(int)timeElapsed.TotalSeconds} sec";
        }
        else if (timeElapsed.TotalMinutes < 60) // Less than an hour ago
        {
            return $"{(int)timeElapsed.TotalMinutes}min";
        }
        else if (timeElapsed.TotalHours < 24) // Less than a day ago
        {
            return $"{(int)timeElapsed.TotalHours}h";
        }
        else if (timeElapsed.TotalDays < 30) // Less than a month ago
        {
            int days = (int)timeElapsed.TotalDays;
            return days == 1 ? "yesterday" : $"{days}d";
        }
        else // More than a month ago
        {
            return EventDisplayModel.DateCreated.ToString("MMMM dd, yyyy");
        }
    }

    private async void TimmerElapsedAsync(object sender, ElapsedEventArgs e)
    {
       await MainThread.InvokeOnMainThreadAsync(() =>
        {
            FormattedDateCreated = FormatDateCreated(EventDisplayModel.DateCreated);

            foreach(var comment in Comments)
            {
                comment.DateCreatedString = FormatDateCreated(comment.DateCreated);
            }
        });
    }


    public EventDetailsPageViewModel(IEventHub eventHub,
                                     IEventEndpoint eventEndpoint)
    {
        Entry = new Entry();

        _eventHub = eventHub;
        _eventEndpoint = eventEndpoint;

        _eventHub.Connection.On<LikeDisplayModel>("EventLiked", LikeAddedAsync);
        _eventHub.Connection.On<CommentDisplayModel>("EventCommented", CommentAddedAsync);

        timer = new(TimeSpan.FromMinutes(1));
        timer.Elapsed += TimmerElapsedAsync;
        timer.Start();

    }
}
