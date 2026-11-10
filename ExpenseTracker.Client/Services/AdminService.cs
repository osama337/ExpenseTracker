using System.Net.Http.Json;
using ExpenseTracker.Client.DTOs;
using ExpenseTracker.Client.Models;

namespace ExpenseTracker.Client.Services;

public class AdminService
{
    private readonly HttpClient _http;

    public AdminService(HttpClient http)
    {
        _http = http;
    }

    public async Task<AdminStats?> GetStatsAsync()
    {
        return await _http.GetFromJsonAsync<AdminStats>("api/Admin/stats");
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        return await _http.GetFromJsonAsync<List<UserDto>>("api/Admin/users")
               ?? new List<UserDto>();
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var response = await _http.DeleteAsync($"api/Admin/users/{id}");
        return response.IsSuccessStatusCode;
    }
}