using System.Security.Claims;
using ExpenseTracker.Api.DTOs;
using ExpenseTracker.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ExpensesController(AppDbContext context) =>_context = context;


        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Expense>>> GetAllAsync()
        {
            var expenses = await _context.Expenses
                .Where(e => e.UserId == CurrentUserId)
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            return Ok(expenses);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Expense>> GetById(int id)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == CurrentUserId);

            if (expense == null)
                return NotFound();

            return Ok(expense);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Expense>> Create(Expense expense)
        {
            expense.Id = 0;
            expense.UserId = CurrentUserId; 

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
        }

        [HttpGet("monthly-total")]
        public async Task<ActionResult<List<MonthlyTotalDto>>> GetMonthlyTotal()
        {
            var totals = await _context.Expenses
                .Where(e => e.UserId == CurrentUserId)
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g => new MonthlyTotalDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Sum(e => e.Amount)
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToListAsync();

            return Ok(totals);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, Expense updated)
        {
            if (id != updated.Id)
                return BadRequest();

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == CurrentUserId);

            if (expense == null)
                return NotFound();

            expense.Title = updated.Title;
            expense.Amount = updated.Amount;
            expense.Category = updated.Category;
            expense.Date = updated.Date;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == CurrentUserId);

            if (expense == null)
                return NotFound();

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}