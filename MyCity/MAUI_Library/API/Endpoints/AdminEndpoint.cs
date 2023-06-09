using MAUI_Library.API.Interfaces;
using MAUI_Library.Helpers;
using MAUI_Library.Models.Incoming;
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

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/Event/Admin/GetAll/{timeSpan}"))
        {
            if (!response.IsSuccessStatusCode) return Enumerable.Empty<BasicEventModel>();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<BasicEventModel>>();

            return result;
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

    public AdminEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;   
    }
}
