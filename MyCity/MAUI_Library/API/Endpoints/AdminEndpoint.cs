using MAUI_Library.API.Interfaces;
using MAUI_Library.Helpers;
using MAUI_Library.Models.Incoming;
using MAUI_Library.Models.OutgoingDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Library.API.Endpoints;

public class AdminEndpoint : IAdminEndpoint
{
    private readonly IApiHelper _apiHelper;

    public async Task<IEnumerable<BasicEventModel>> GetAllEvents(TimeSpan timeSpan)
    {
        var valid = await UserSessionManager.IsTokenValid();

        if (!valid)
        {
            var refresh = await _apiHelper.RefreshTokens();

            if (!refresh) return Enumerable.Empty<BasicEventModel>();
        }

        var jwtToken = await SecureStorage.GetAsync("token");

        if (jwtToken is null)
        {
            return Enumerable.Empty<BasicEventModel>();
        }

        _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _apiHelper.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);


        try
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/Event/Admin/GetAll/{timeSpan}"))
            {
                if (!response.IsSuccessStatusCode) return Enumerable.Empty<BasicEventModel>();

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<BasicEventModel>>();

                return result;
            }
        }
        catch (Exception ex)
        {

            return Enumerable.Empty<BasicEventModel>();
        }
        
    }

    public async Task<IEnumerable<RoleModel>> GetAllRoles()
    {
        var valid = await UserSessionManager.IsTokenValid();

        if (!valid)
        {
            var refresh = await _apiHelper.RefreshTokens();

            if (!refresh) return Enumerable.Empty<RoleModel>();
        }

        var jwtToken = await SecureStorage.GetAsync("token");

        if (jwtToken is null)
        {
            return Enumerable.Empty<RoleModel>();
        }

        _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _apiHelper.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);


        using(HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/Auth/Admin/Roles/GetAll"))
        {
            if (!response.IsSuccessStatusCode) return Enumerable.Empty<RoleModel>();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<RoleModel>>();

            return result;
        }
    }

    public async Task<IEnumerable<RoleModel>> GetAllRequiredRoles()
    {
        var valid = await UserSessionManager.IsTokenValid();

        if (!valid)
        {
            var refresh = await _apiHelper.RefreshTokens();

            if (!refresh) return Enumerable.Empty<RoleModel>();
        }

        var jwtToken = await SecureStorage.GetAsync("token");

        if (jwtToken is null)
        {
            return Enumerable.Empty<RoleModel>();
        }

        _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _apiHelper.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);


        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/Auth/Admin/Roles/GetAllRequired"))
        {
            if (!response.IsSuccessStatusCode) return Enumerable.Empty<RoleModel>();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<RoleModel>>();

            return result;
        }
    }

    public async Task<(bool, string)> SubmitRequestAsync(string roleId)
    {
        var valid = await UserSessionManager.IsTokenValid();

        if (!valid)
        {
            var refresh = await _apiHelper.RefreshTokens();

            if (!refresh) return (false, "Error with authentication, please try to login again!");
        }

        var jwtToken = await SecureStorage.GetAsync("token");

        if (jwtToken is null)
        {
            return (false, "Error with authentication, please try to login again!");
        }

        _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _apiHelper.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync($"api/Auth/Admin/Roles/SubmitRoleRequest",JsonContent.Create(roleId)))
        {

           if (!response.IsSuccessStatusCode) return (false, await response.Content.ReadAsStringAsync());


           string error = await response.Content.ReadAsStringAsync();


           return (true, error);
        }
    }

    public async Task<(bool, IEnumerable<RoleRequestModel>)> GetAllRoleRequests()
    {
        var valid = await UserSessionManager.IsTokenValid();

        if (!valid)
        {
            var refresh = await _apiHelper.RefreshTokens();

            if (!refresh) return (false, Enumerable.Empty<RoleRequestModel>());
        }

        var jwtToken = await SecureStorage.GetAsync("token");

        if (jwtToken is null)
        {
            return (false, Enumerable.Empty<RoleRequestModel>());
        }

        _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _apiHelper.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/Auth/Admin/Roles/GetAllRoleRequests");

        if (!response.IsSuccessStatusCode) return (false, Enumerable.Empty<RoleRequestModel>());

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<RoleRequestModel>>();

        return (true, result);

    }

    public async Task<bool> RespondToRoleRequest(RoleRequestRespondDto responseDto)
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

        using HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("api/Auth/Admin/Roles/RespondToRoleRequest", responseDto);

        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<BasicEventModel>> GetAllEmergencies(TimeSpan timeSpan)
    {
        var valid = await UserSessionManager.IsTokenValid();

        if (!valid)
        {
            var refresh = await _apiHelper.RefreshTokens();

            if (!refresh) return Enumerable.Empty<BasicEventModel>();
        }

        var jwtToken = await SecureStorage.GetAsync("token");

        if (jwtToken is null)
        {
            return Enumerable.Empty<BasicEventModel>();
        }

        _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _apiHelper.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);


        try
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/Event/Admin/GetAllEmergencies/{timeSpan}"))
            {
                if (!response.IsSuccessStatusCode) return Enumerable.Empty<BasicEventModel>();

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<BasicEventModel>>();

                return result;
            }
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            //log error
            return Enumerable.Empty<BasicEventModel>();
        }
    }

    public async Task<bool> RespondToEmergencyEvent(string eventId)
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

        using HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("api/Event/Admin/RespondToEmergencyEvent", eventId);

        return response.IsSuccessStatusCode; 
    }

    public AdminEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;   
    }
}
