using ExpenseTracker.Client.Models;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ExpenseTracker.Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _js;

    public AuthService(HttpClient httpClient, IJSRuntime js)
    {
        _httpClient = httpClient;
        _js = js;
    }

    public async Task<HttpResponseMessage> RegisterAsync(RegisterRequest request)
    {
        return await _httpClient.PostAsJsonAsync("api/Auth/register", request);
    }

    public async Task<HttpResponseMessage> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/Login", request);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<AuthResponse>();
            await _js.InvokeVoidAsync("localStorage.setItem", "token", data!.Token);
            await _js.InvokeVoidAsync("localStorage.setItem", "fullName", data.FullName);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", data.Token);
        }

        return response;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", "token");
        if (string.IsNullOrEmpty(token)) return false;

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        return true;
    }

    public async Task<bool> IsAdminAsync()
    {
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", "token");
        if (string.IsNullOrEmpty(token)) return false;

        try
        {
            var payload = token.Split('.')[1].Replace('-', '+').Replace('_', '/');
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            using var doc = JsonDocument.Parse(Convert.FromBase64String(payload));

            var names = new[]
            {
                "role",
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            };

            foreach (var name in names)
            {
                if (!doc.RootElement.TryGetProperty(name, out var role)) continue;

                if (role.ValueKind == JsonValueKind.Array)
                    return role.EnumerateArray().Any(r => r.GetString() == "Admin");

                return role.GetString() == "Admin";
            }
        }
        catch
        {
            return false;         
        }

        return false;
    }

    public async Task LogoutAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", "token");
        await _js.InvokeVoidAsync("localStorage.removeItem", "fullName");
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string> GetFullNameAsync()
    {
        return await _js.InvokeAsync<string?>("localStorage.getItem", "fullName") ?? "";
    }
}