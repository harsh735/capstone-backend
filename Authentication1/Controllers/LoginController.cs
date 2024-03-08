﻿using Authentication1.Data;
using Authentication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication1.Controllers
{
    [ApiController]
    [Route("/api/")]
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var hashedPassword = await HashPasswordAsync(model.Password);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == hashedPassword);
            var userName = user.FirstName;
            if (user == null)
            {
                return NotFound(new { message = "Invalid credentials" });
            }

            var token = GenerateJwtToken(user);

            return Ok(new { message = $"Welcome, {userName} {user.LastName}!", token, userName });
        }


        private string GenerateJwtToken(RegisterUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //working google sso api in react
        [HttpPost("oauth-login")]
        public IActionResult SSOUser([FromBody] RegisterUser userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == userData.Email);
            if (existingUser == null)
            {
                _context.Users.Add(userData);
                _context.SaveChanges();
            }

            return Ok("User logged in successfully");
        }

        private async Task<string> HashPasswordAsync(string password)
        {
            return await Task.Run(() =>
            {
                using var sha256 = System.Security.Cryptography.SHA256.Create();
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            });
        }
    }
}