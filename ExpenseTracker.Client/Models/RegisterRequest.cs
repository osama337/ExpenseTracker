using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Client.Models
{
    public class RegisterRequest
    {
        [Required]
        public string FullName { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, MinLength(8)]
        public string Password { get; set; } = "";

        [Required, Compare(nameof(Password),
            ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
