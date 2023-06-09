using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.Models;
using MAUI_Library.Models.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace MAUI_Library.Helpers;

public static class UserSessionManager
{
    public static ShellItem LogOfButton { get; set; }
    public static ShellItem LoginPage { get; set; }
    public static ShellItem RegisterPage { get; set; }

#if !ANDROID
    public static ShellItem AdminMainPage { get; set; }
    public static ShellItem AuthorizedPersonelMainPage { get; set; }
    public static ShellItem UnauthorizedPage { get; set; }
#endif

    public static ShellItem AccountPage { get; set; }

    public static IEventHub _eventHub;

    public static async Task LogofAsync()
    {

        if(LoginPage is not null) Shell.Current.Items.Add(LoginPage);
        if(RegisterPage is not null) Shell.Current.Items.Add(RegisterPage);
        Shell.Current.CurrentItem = LoginPage;

#if !ANDROID
        if(AdminMainPage is not null) Shell.Current.Items.Remove(AdminMainPage);
        if (AuthorizedPersonelMainPage is not null) Shell.Current.Items.Remove(AuthorizedPersonelMainPage);
#endif
        if (LogOfButton is not null) Shell.Current.Items.Remove(LogOfButton);
        if (AccountPage is not null) Shell.Current.Items.Remove(AccountPage);

        RemoveTokens();

#if ANDROID
        await ReconnectAsync();
#endif
        //var loginPage = Shell.Current.Items.FirstOrDefault(item => item.Title == "Login");
        //loginPage.FlyoutItemIsVisible= true;

        //var accountPage = Shell.Current.Items.FirstOrDefault(item => item.Title == "Account");
        //accountPage.FlyoutItemIsVisible= false;

        //Shell.Current.FlyoutIsPresented= false;

        //if(LogOfButton is not null)
        //{
        //    Shell.Current.Items.Remove(LogOfButton);
        //}
    }

    private static async Task ReconnectAsync()
    {
        if (_eventHub is null) return;

        try
        {
            var isDisconected = await _eventHub.DisconnectAsync();
            if(isDisconected) await _eventHub.ConnectAsync();
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }
    }

    public static async Task LoginAsync()
    {
#if !ANDROID

        var roles = await GetUserRoles();

        if (roles.Contains("Admin"))
        {
            if (AdminMainPage is not null)
            {
                Shell.Current.Items.Add(AdminMainPage);
                Shell.Current.CurrentItem = AdminMainPage;
            }
        }
        else if(roles.Contains("AuthorizedPersonel"))
        {
            if (AuthorizedPersonelMainPage is not null)
            {
                Shell.Current.Items.Add(AuthorizedPersonelMainPage);
                Shell.Current.CurrentItem = AuthorizedPersonelMainPage;
            }
        }
        else
        {
            if(UnauthorizedPage is not null)
            {
                Shell.Current.Items.Add(UnauthorizedPage);
                Shell.Current.CurrentItem = UnauthorizedPage;
            }
        }



        //var registerPage = Shell.Current.Items.FirstOrDefault(item => item.Title == "Register");

        //if (registerPage is not null) Shell.Current.Items.Remove(registerPage);

        //if(registerPage == RegisterPage)
        //{
        //    string error = "Glup si ko kurac";
        //}


#endif
        if (AccountPage is not null) Shell.Current.Items.Add(AccountPage);
        if (LogOfButton is not null) Shell.Current.Items.Add(LogOfButton);
        if (LoginPage is not null) Shell.Current.Items.Remove(LoginPage);
        if (RegisterPage is not null) Shell.Current.Items.Remove(RegisterPage);



        await ReconnectAsync();
        //#if ANDROID
        //        if (_logOfButton is not null)
        //        {
        //            Shell.Current.Items.Add(_logOfButton);
        //        }
        //        var accountPage = Shell.Current.Items.FirstOrDefault(item => item.Title == "Account");
        //        accountPage.FlyoutItemIsVisible = true;

        //        var loginPage = Shell.Current.Items.FirstOrDefault(item => item.Title == "Login");
        //        loginPage.FlyoutItemIsVisible = false;
        //#endif
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

    public static async Task<IEnumerable<string>> GetUserRoles()
    {
        var jwtToken = await SecureStorage.GetAsync("token");
        
        if(jwtToken is null) return Enumerable.Empty<string>();

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);
        var claims = token.Claims;

        var roles = new List<string>();
        foreach (var claim in claims)
        {
            if (claim.Type == "role")
            {
                roles.Add(claim.Value);
            }
        }
        return roles;
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
