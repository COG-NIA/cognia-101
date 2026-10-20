using Microsoft.JSInterop;

namespace Cognia.Client.Services;

public sealed class AuthTokenStore(IJSRuntime jsRuntime)
{
    private const string TokenKey = "cognia.authToken";

    public ValueTask<string?> GetAsync() => jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", TokenKey);

    public ValueTask SetAsync(string token) => jsRuntime.InvokeVoidAsync("sessionStorage.setItem", TokenKey, token);

    public ValueTask ClearAsync() => jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", TokenKey);
}