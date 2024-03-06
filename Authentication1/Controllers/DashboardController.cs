using Authentication1.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication1.Controllers
{
    [ApiController]
    [Route("/api/")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("investmentdata")]
        public async Task<IActionResult> UserInvestments(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound($"User with email {userEmail} not found.");
            }

            var investments = await _context.InvestmentInfo
                .Where(i => i.UserID == user.UserID)
                .ToListAsync();

            if (investments == null || investments.Count == 0)
            {
                return NotFound($"No investments found for user with email {userEmail}.");
            }

            return Ok(new { message = "Investment data sent successfully!", data = investments });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok(new { message = "Logout successful" });
        }



        // api for adding investments
        // api for updating profile
    }
}
