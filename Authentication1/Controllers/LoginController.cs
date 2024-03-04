using Authentication1.Data;
using Authentication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Authentication1.Controllers
{

    [ApiController]
    [Route("/api/")]
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var hashedPassword = await HashPasswordAsync(model.Password);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == hashedPassword);

            if (user == null)
            {
                return NotFound(new { message = "Invalid credentials" });
            }

            return Ok(new { message = $"Welcome, {user.FirstName} {user.LastName}!", user = user });
        }


        //error prone backend sso api 

        //[HttpGet("google")]
        //public IActionResult GoogleLogin()
        //{
        //    var redirectUrl = Url.Action(nameof(GoogleLoginCallback), "Login");
        //    var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        //    return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        //}

        //[HttpGet("google-callback")]
        //public async Task<IActionResult> GoogleLoginCallback()
        //{
        //    var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    if (!authenticateResult.Succeeded)
        //    {
        //        return BadRequest("User not authenticated");
        //    }


        //    var claimAuthResult = authenticateResult.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        //    {
        //        claim.Issuer,
        //        claim.OriginalIssuer,
        //        claim.Type,
        //        claim.Value,
        //    }).ToList();

        //    return Ok(claimAuthResult);
        //}





        //working google sso api in react
        [HttpPost("oauth-login")]
        public IActionResult SSOUser([FromBody] GoogleUserData userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = _context.GoogleUserData.FirstOrDefault(u => u.Email == userData.Email);
            if (existingUser == null)
            {
                _context.GoogleUserData.Add(userData);
                _context.SaveChanges();
            }

            return Ok("User logged in successfully");
        }


        private async Task<string> HashPasswordAsync(string password)
        {
            return await Task.Run(() =>
            {
                using var sha256 = System.Security.Cryptography.SHA256.Create();
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            });
        }
    }
}