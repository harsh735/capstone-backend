using Authentication1.Data;
using Authentication1.Models;
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

        [HttpGet("investmentData")]
        public async Task<IActionResult> UserInvestments(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound($"User with email {userEmail} not found.");
            }

            var investments = await _context.InvestmentInfo
                .Where(i => i.UserID == user.UserID)
                .GroupBy(i => i.InvestorInfoID)
                .Select(group => group.First())
                .ToListAsync();

            if (investments == null || investments.Count == 0)
            {
                return NotFound($"No investments found for user with email {userEmail}.");
            }

            return Ok(new { message = "Investment data sent successfully!", data = investments });
        }



        [HttpPost("addBankDetails")]
        public async Task<IActionResult> AddBankDetails([FromQuery] string userEmail, [FromBody] UserBankDetails bankDetailsDto, IFormFile panFile)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("User email is required.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound($"User with email {userEmail} not found.");
            }

            var bankDetails = new UserBankDetails
            {
                UserID = user.UserID,
                PanNumber = bankDetailsDto.PanNumber,
                BankAccNumber = bankDetailsDto.BankAccNumber,
                AccHolderName = bankDetailsDto.AccHolderName,
                BankName = bankDetailsDto.BankName,
                IFSCCode = bankDetailsDto.IFSCCode
            };

            if (panFile != null && panFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await panFile.CopyToAsync(memoryStream);
                    bankDetails.PanFile = memoryStream.ToArray();
                }
            }
            _context.UserBankDetails.Add(bankDetails);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Bank details added successfully!", success = true });
        }


        [HttpPut("updateBankDetails")]
        public async Task<IActionResult> UpdateBankDetails([FromQuery] string userEmail, [FromBody] UserBankDetails bankDetailsDto)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("User email is required.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound($"User with email {userEmail} not found.");
            }

            var existingBankDetails = await _context.UserBankDetails.FirstOrDefaultAsync(b => b.UserID == user.UserID);
            if (existingBankDetails == null)
            {
                return NotFound($"Bank details not found for user with email {userEmail}.");
            }

            existingBankDetails.UserID = user.UserID;
            existingBankDetails.PanNumber = bankDetailsDto.PanNumber;
            existingBankDetails.PanFile = bankDetailsDto.PanFile;
            existingBankDetails.BankAccNumber = bankDetailsDto.BankAccNumber;
            existingBankDetails.AccHolderName = bankDetailsDto.AccHolderName;
            existingBankDetails.BankName = bankDetailsDto.BankName;
            existingBankDetails.IFSCCode = bankDetailsDto.IFSCCode;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Bank details updated successfully!" });
        }


        [HttpPut("updateProfile")]
        public IActionResult UpdateUser([FromQuery] string email, [FromBody] RegisterUser updatedUser)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Phone = updatedUser.Phone;
            user.Company = updatedUser.Company;
            user.City = updatedUser.City;
            user.State = updatedUser.State;
            user.Address = updatedUser.Address;

            _context.SaveChanges();

            return Ok("User information updated successfully");
        }




        [HttpPost("newInvestment/gold")]
        public async Task<ActionResult> AddGoldInvestment(string userEmail, [FromBody] InvestmentModel investmentModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            investmentModel.UserID = user.UserID;

            _context.InvestmentInfo.Add(new InvestmentModel
            {
                Amount = investmentModel.Amount ?? 0,
                UserID = investmentModel.UserID,
                InvestmentName = investmentModel.InvestmentName,
                Active = investmentModel.Active,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                DeleteFlag = investmentModel.DeleteFlag,
                InvestmentType = investmentModel.InvestmentType,
                Risk = investmentModel.Risk,
                ExpectedAmount = investmentModel.Amount * 1.5m ?? 0
            });
            await _context.SaveChangesAsync();

            return Ok(new { message = "Investment added successfully", success = true });
        }



        [HttpPost("deleteInvestment")]
        public async Task<ActionResult> DeleteInvestment(string userEmail, int investorInfoID)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var existingInvestment = await _context.InvestmentInfo.FirstOrDefaultAsync(i => i.InvestorInfoID == investorInfoID);
            if (existingInvestment == null)
            {
                return BadRequest("Unable to delete an investment which does not exist!");
            }

            _context.InvestmentInfo.Remove(existingInvestment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Investment deleted successfully" });
        }




        [HttpPost("purchaseSubscription")]
        public async Task<IActionResult> PurchaseSubscription([FromQuery] string userEmail, [FromBody] Subscriptions newSubscriber)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var existingSubscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserID == user.UserID);
            if (existingSubscription != null)
            {
                return BadRequest("User already has an active subscription.");
            }

            var newSubscription = new Subscriptions
            {
                UserID = user.UserID,
                SubscriptionID = newSubscriber.SubscriptionID,
                SubscriptionName = newSubscriber.SubscriptionName,
                StartDate = DateTime.Now,
                EndDate = newSubscriber.EndDate,
                Price = newSubscriber.Price,
                Active = true
            };

            _context.Subscriptions.Add(newSubscription);
            await _context.SaveChangesAsync();

            return Ok(new { message = "New Subscription Added!", subData = newSubscription });
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
