using System.Net.Http.Json;

namespace Cognia.Client.Services;

public sealed class AuthService(HttpClient httpClient, AuthTokenStore tokenStore)
{
    public async Task<string?> RegisterAsync(string email, string displayName, string password)
    {
        var response = await httpClient.PostAsJsonAsync("api/auth/register", new
        {
            Email = email,
            DisplayName = displayName,
            Password = password
        });
        return await HandleResponseAsync(response);
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var response = await httpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });
        return await HandleResponseAsync(response);
    }

    public ValueTask SignOutAsync() => tokenStore.ClearAsync();

    public async ValueTask<bool> IsSignedInAsync() => !string.IsNullOrWhiteSpace(await tokenStore.GetAsync());

    private async Task<string?> HandleResponseAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result is null || string.IsNullOrWhiteSpace(result.Token))
        {
            return "The server returned an invalid sign-in response.";
        }

        await tokenStore.SetAsync(result.Token);
        return null;
    }

    private sealed record AuthResponse(int UserId, string Email, string DisplayName, string Token);
}