namespace ExpenseTracker.Client.Models
{
    public class AuthResponse
    {
        public string Token { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
