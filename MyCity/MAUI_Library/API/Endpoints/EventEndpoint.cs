using MAUI_Library.API.Interfaces;
using MAUI_Library.Helpers;
using MAUI_Library.Models.Incoming;
using MAUI_Library.Models.OutgoingDto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MAUI_Library.API.Endpoints;

public class EventEndpoint : IEventEndpoint
{
    private readonly IApiHelper _apiHelper;

    public Task<bool> DeleteEventAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<EventModel>> GetAllEventsAsync(TimeSpan timespan)
    {

        using(HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/Event/GetAll/{timespan}"))
        {
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<EventModel>>();
                return result;
            }
            else
            {
                //log error
                return Enumerable.Empty<EventModel>();
            }
        }
    }

    public Task<IEnumerable<EventModel>> GetUserEventsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> PostEventAsync(EventDto eventDto)
    {
        var valid = await UserSessionManager.IsTokenValid();

        if (!valid)
        {
            var refresh = await _apiHelper.RefreshTokens();

            if (!refresh) return false;
        }

        var jwtToken = await SecureStorage.GetAsync("token");

        if (jwtToken is null)
        {
            return false;
        }

        _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _apiHelper.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        using (HttpResponseMessage responese = await _apiHelper.ApiClient.PostAsync("api/Event/Create", JsonContent.Create(eventDto)))
        {    
            return responese.IsSuccessStatusCode;
        }
    }

    public Task<bool> UpdateEventAsync(string id, string title, string description)
    {
        throw new NotImplementedException();
    }

    public async Task<(bool,EventModel)> GetEventByIdAsync(string id)
    {
        //var valid = await UserSessionManager.IsTokenValid();

        //if (!valid)
        //{
        //    var refresh = await _apiHelper.RefreshTokens();

        //    if (!refresh) return (false,null);
        //}

        //var jwtToken = await SecureStorage.GetAsync("token");

        //if (jwtToken is null)
        //{
        //    return (false, null);
        //}

        _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //_apiHelper.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);


        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/Event/{id}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<EventModel>();
                return (true, result);
            }

            return (false, null);
        }
    }

    public Task<bool> PostCommentAsync(string eventId, string content)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CommentModel>> GetAllComments(string id)
    {
        _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/Event/Comments/{id}"))
        {
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<IEnumerable<CommentModel>>();
                return result;
            }
            return Enumerable.Empty<CommentModel>();
        }
    }

    public EventEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }
}
