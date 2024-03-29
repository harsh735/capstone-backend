﻿using Authentication1.Data;
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

        [HttpGet("viewClients")]
        public async Task<IActionResult> GetClientsByEmail(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }


            var clients = await _context.Clients.Where(c => c.AdvisorID == user.AdvisorID).ToListAsync();

            if (clients == null)
            {
                return NotFound();
            }

            return Ok(new { message = "Clients fetched successfully", clients, success = true });
        }




        [HttpPost("addClient")]
        public async Task<IActionResult> AddClient(string userEmail, [FromBody] Clients newClient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                newClient.UserID = user.UserID;

                var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.UserID == newClient.UserID && c.AdvisorID == newClient.AdvisorID);
                if (existingClient != null)
                {
                    return Conflict("Client already exists!");
                }
                _context.Clients.Add(new Clients
                {
                    UserID = newClient.UserID,
                    AdvisorID = newClient.AdvisorID,
                    TypeOfInvestment = newClient.TypeOfInvestment,
                    RoleID = newClient.RoleID,
                    ClientName = newClient.ClientName,
                    SubscribedOn = newClient.SubscribedOn,
                    PortfolioValue = newClient.PortfolioValue,
                });


                await _context.SaveChangesAsync();
                return Ok(new { message = "Client registered successfully", client = newClient });
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding client!", ex);
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
