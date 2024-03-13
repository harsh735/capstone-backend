using Authentication1.Data;
using Authentication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication1.Controllers
{

    [ApiController]
    [Route("/api/")]
    public class AdvisorController : Controller
    {

        private readonly AppDbContext _context;
        public AdvisorController(AppDbContext context)
        {
            _context = context;

        }


        //[HttpGet("viewClient")]
        //public async Task<IActionResult> ViewClient([FromBody] int advisorID)
        //{
        //   var user = await _context.Users.FirstOrDefaultAsync(u => u.)
        //}






        [HttpPost("addClient")]
        public async Task<IActionResult> AddClient([FromBody] RegisterUser newClient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == newClient.Email);
                if (user != null)
                {
                    return Conflict("Client is alredy registered!");
                }


                _context.Users.Add(newClient);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Client registered successfully", client = newClient });
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding client!");
            }
        }


        [HttpPost("deleteClient")]
        public async Task<IActionResult> DeleteClient([FromBody] int userID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userID);
                if (user == null)
                {
                    return Conflict("Client does not exist!");
                }


                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Client removed successfully" });
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting client!");
            }
        }
    }
}
