using ExpenseTracker.Client.DTOs;
using ExpenseTracker.Client.Models;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace ExpenseTracker.Client.Services
{
    public class ExpenseService
    {
        private readonly HttpClient _httpClient;
        public ExpenseService(HttpClient httpClient) =>  _httpClient = httpClient;
        public async Task<List<Expense>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Expense>>("api/Expenses/GetAll") ?? new List<Expense>();
        }

        public async Task<Expense?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Expense>($"api/Expenses/GetById/{id}");
        }
        public async Task<bool> CreateAsync(Expense expense)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Expenses/Create", expense);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateAsync(Expense expense)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Expenses/Update/{expense.Id}", expense);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Expenses/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<MonthlyTotalDto>> GetMonthlyTotalsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<MonthlyTotalDto>>("api/Expenses/monthly-total")
                   ?? new List<MonthlyTotalDto>();
        }
    }
}
