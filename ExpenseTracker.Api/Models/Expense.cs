using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Api.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Required,MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Required,MaxLength(50)]
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Today;

        public string? UserId { get; set; } 
        [JsonIgnore]
        public User? user { get; set; } = null!; 
    }
}
