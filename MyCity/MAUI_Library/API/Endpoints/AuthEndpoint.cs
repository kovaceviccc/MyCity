using MAUI_Library.API.Interfaces;
using MAUI_Library.Helpers;
using MAUI_Library.Models.OutgoingDto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MAUI_Library.API.Endpoints;

public class AuthEndpoint : IAuthEndpoint
{
    private readonly IApiHelper _apiHelper;

    public AuthEndpoint(IApiHelper apiHelper)
	{
		_apiHelper = apiHelper;
	}

	public async Task<bool> Register(RegisterRequestDto request)
	{

		_apiHelper.ApiClient.DefaultRequestHeaders.Clear();
		_apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
		_apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("api/Auth/Register", request))
		{
			return response.IsSuccessStatusCode;
		}
	}

	public async Task<bool> ChangePassword(ChangePasswordRequestDto request)
	{
		var isValid = await UserSessionManager.IsTokenValid();

		if (!isValid) await _apiHelper.RefreshTokens();

		string jwtToken = (await UserSessionManager.GetTokens()).Item1;

        //_apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        //_apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
        //_apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _apiHelper.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("api/Auth/ChangePassword",request))
		{
			return response.IsSuccessStatusCode;
		}
	}

}
