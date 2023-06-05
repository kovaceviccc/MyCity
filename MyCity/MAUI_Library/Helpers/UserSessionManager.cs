using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.Models;
using MAUI_Library.Models.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace MAUI_Library.Helpers;

public static class UserSessionManager
{

    public static ShellItem _logOfButton;
    public static ShellItem LoginPage { get; set; }
    public static ShellItem RegisterPage { get; set; }
    public static IEventHub _eventHub;

    public static async Task LogofAsync()
    {

#if ANDROID
        var loginPage = Shell.Current.Items.FirstOrDefault(item => item.Title == "Login");
        loginPage.FlyoutItemIsVisible= true;
        
        var accountPage = Shell.Current.Items.FirstOrDefault(item => item.Title == "Account");
        accountPage.FlyoutItemIsVisible= false;

        Shell.Current.FlyoutIsPresented= false;

#endif
        if(_logOfButton is not null)
        {
            Shell.Current.Items.Remove(_logOfButton);
        }

        RemoveTokens();

        await ReconnectAsync();
    }

    private static async Task ReconnectAsync()
    {
        try
        {
            var isDisconected = await _eventHub.DisconnectAsync();
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }

        await _eventHub.ConnectAsync();
    }

    public static async Task LoginAsync()
    {
#if ANDROID
        if (_logOfButton is not null)
        {
            Shell.Current.Items.Add(_logOfButton);
        }
        var accountPage = Shell.Current.Items.FirstOrDefault(item => item.Title == "Account");
        accountPage.FlyoutItemIsVisible = true;

        var loginPage = Shell.Current.Items.FirstOrDefault(item => item.Title == "Login");
        loginPage.FlyoutItemIsVisible = false;
#endif
        await ReconnectAsync();
    }

    public static async Task<Location> GetUserLocationAsync()
    {
        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            return null;
        }
        var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));
        location ??= await Geolocation.GetLastKnownLocationAsync();
        return location;
    }

    public static async Task<bool> CheckCredentialsAsync()
    {
        if(await SecureStorage.GetAsync("token") is null || await SecureStorage.GetAsync("refresh_token") is null)
        {
            await LogofAsync();
            return false;
        }

        await LoginAsync();
        return true;
    }

    public static async Task GetBasicUserModelAsync(this LoggedInUserModel userModel)
    {
        userModel.FirstName = await SecureStorage.GetAsync(LoggedInUserModelEnum.FirstName.ToString());
        userModel.LastName = await SecureStorage.GetAsync(LoggedInUserModelEnum.LastName.ToString());
        userModel.Email = await SecureStorage.GetAsync(LoggedInUserModelEnum.Email.ToString());
        userModel.UserName = await SecureStorage.GetAsync(LoggedInUserModelEnum.UserName.ToString());
    }

    public static async Task StoreUserInfoAsync(LoggedInUserModel usermodel)
    {
        await SecureStorage.SetAsync(LoggedInUserModelEnum.FirstName.ToString(), usermodel.FirstName);
        await SecureStorage.SetAsync(LoggedInUserModelEnum.LastName.ToString(), usermodel.LastName);
        await SecureStorage.SetAsync(LoggedInUserModelEnum.Email.ToString(), usermodel.Email);
        await SecureStorage.SetAsync(LoggedInUserModelEnum.UserName.ToString(), usermodel.UserName);
    }

    public static async Task<(string, string)> GetTokens()
    {
        string token = await SecureStorage.GetAsync("token");
        string refreshToken = await SecureStorage.GetAsync("refresh_token");

        return (token, refreshToken);
    }

    public static async Task StoreTokensAsync(AuthenticatedUser userCredentials)
    {
        await SecureStorage.SetAsync("token", userCredentials.Token);
        await SecureStorage.SetAsync("refresh_token", userCredentials.RefreshToken);
    }

    public static void RemoveTokens()
    {
        SecureStorage.Remove("token");
        SecureStorage.Remove("refresh_token");
    }

    public static async Task<string> GetUserId()
    {
        string token = await SecureStorage.GetAsync("token");

        if (token is null) return string.Empty;

        JwtSecurityTokenHandler tokenHandler = new();
        JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

        string userId = jwtToken.Claims.FirstOrDefault(x=> x.Type == "Id").Value;

        return userId ?? string.Empty; 
    }

    public static async Task<bool> IsTokenValid()
    {
        string jwtToken = await SecureStorage.GetAsync("token");

        if (jwtToken is null) return false;

        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadToken(jwtToken);

        var expirationDate = decodedToken.ValidTo;

        var date = DateTime.UtcNow;

        var seconds = (expirationDate.Subtract(date)).TotalSeconds;

        if (seconds >= 20)
        {
            return true;
        }
        return false;
    }
}
