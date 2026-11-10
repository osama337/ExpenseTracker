namespace ExpenseTracker.Api.DTOs
{
    public class MonthlyTotalDto 
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Total { get; set; }
    }
}
