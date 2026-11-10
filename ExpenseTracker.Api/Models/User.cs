using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Api.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
