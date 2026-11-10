using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Client.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required, Range(0.01, 1000000 , ErrorMessage = "Amount must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Category is required")]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Today;
    }
}
