using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Client.Models
{
    public class LoginRequest

    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

    }
}
