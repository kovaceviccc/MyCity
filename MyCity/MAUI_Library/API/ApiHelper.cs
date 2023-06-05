 using MAUI_Library.API.Interfaces;
using MAUI_Library.Helpers;
using MAUI_Library.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MAUI_Library.API;

public class ApiHelper : IApiHelper
{
    private HttpClient _apiClient { get; set; }
    private LoggedInUserModel _loggedInUserModel { get; set; }
    
    public HttpClient ApiClient
    {
        get { return _apiClient; }
    }


    public ApiHelper(LoggedInUserModel loggedInUserModel)
    {
        Initialize();
        _loggedInUserModel = loggedInUserModel;
        
    }

    private void Initialize()
    {
        string api;

#if DEBUG && ANDROID
        HttpsClientHandlerService handler = new HttpsClientHandlerService();
        _apiClient = new HttpClient(handler.GetPlatformMessageHandler());
        api = "https://10.0.2.2:7266/";
#else
        _apiClient = new HttpClient();
        api = "https://localhost:7266/";
#endif

        _apiClient.BaseAddress = new Uri(api);
        _apiClient.DefaultRequestHeaders.Accept.Clear();
        _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<bool> Authenticate(string email, string password)
    {
        var data = new
        {
            email,
            password
        };
        var content = JsonContent.Create(data);

        using (HttpResponseMessage response = await _apiClient.PostAsync("api/Auth/Login",content))
        {
            if(response.IsSuccessStatusCode) 
            {
                var result = await response.Content.ReadFromJsonAsync<AuthenticatedUser>();
                await UserSessionManager.StoreTokensAsync(result);
                return true;
            }
            else
            {
                var errors = await response.Content.ReadFromJsonAsync<List<string>>();
                return false;
            }
        }
    }

    public async Task<bool> RefreshTokens()
    {
        try
        {
            (string token, string refreshToken) = await UserSessionManager.GetTokens();

            if(token is null || refreshToken is null)
            {
                return false;
            }

            var data = new
            {
                token,
                refreshToken
            };

            using (HttpResponseMessage response = await _apiClient.PostAsync("api/Auth/RefreshToken", JsonContent.Create(data)))
            {
                if(response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthenticatedUser>();
                    await UserSessionManager.StoreTokensAsync(result);
                    return true;
                }
            }
            return false;
        }
        catch (Exception)
        {

            return false;
            //log error
        }
    }

    public async Task<bool> GetLoggedInUserInfo()
    {
        string token = await SecureStorage.GetAsync("token");

        if(token is null)
        {
            return false;
        }

        _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

        using(HttpResponseMessage response = await _apiClient.GetAsync("api/Auth/GetUserInfo"))
        {
            if(response.IsSuccessStatusCode)
            {
                var userInfo = await response.Content.ReadFromJsonAsync<LoggedInUserModel>();

                _loggedInUserModel.Email = userInfo.Email;
                _loggedInUserModel.FirstName = userInfo.FirstName;
                _loggedInUserModel.LastName = userInfo.LastName;
                _loggedInUserModel.UserName  = userInfo.UserName;
                await UserSessionManager.StoreUserInfoAsync(_loggedInUserModel);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
