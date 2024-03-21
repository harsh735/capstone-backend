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
        public async Task<ActionResult> AddGoldInvestment(string userEmail, [FromBody] GoldInvestments goldInvestment)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            goldInvestment.UserID = user.UserID;

            _context.GoldInvestments.Add(new GoldInvestments
            {
                Amount = goldInvestment.Amount ?? 0,
                UserID = goldInvestment.UserID,
                PurchaseDate = goldInvestment.PurchaseDate,
                Type = "Gold",
                Risk = goldInvestment.Risk,
                Frequency = goldInvestment.Frequency,
                Timeframe = goldInvestment.Timeframe,
                ExpectedAmount = goldInvestment.ExpectedAmount,
            });

            _context.InvestmentInfo.Add(new InvestmentModel
            {
                Amount = goldInvestment.Amount,
                UserID = goldInvestment.UserID,
                InvestmentName = "Digital Gold",
                Active = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                DeleteFlag = false,
                InvestmentType = "Gold",
                Risk = goldInvestment.Risk,
            });


            await _context.SaveChangesAsync();

            return Ok(new { message = "Investment added successfully", success = true });
        }





        [HttpPost("newInvestment/mutualFunds")]
        public async Task<ActionResult> AddMutualFundInvestment(string userEmail, [FromBody] MutualFundsInvestments mutualFundInvestmentModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            mutualFundInvestmentModel.UserID = user.UserID;

            _context.MutualFundsInvestments.Add(new MutualFundsInvestments
            {
                UserID = mutualFundInvestmentModel.UserID,
                Amount = mutualFundInvestmentModel.Amount,
                PurchaseDate = mutualFundInvestmentModel.PurchaseDate,
                Type = "Mutual Funds",
                Risk = mutualFundInvestmentModel.Risk,
                ExpectedAmount = mutualFundInvestmentModel.ExpectedAmount,
                MFName = mutualFundInvestmentModel.MFName,
                MFType = mutualFundInvestmentModel.MFType,
                NAV = mutualFundInvestmentModel.NAV,
            });

            _context.InvestmentInfo.Add(new InvestmentModel
            {
                Amount = mutualFundInvestmentModel.Amount,
                UserID = mutualFundInvestmentModel.UserID,
                InvestmentName = mutualFundInvestmentModel.MFName,
                Active = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                DeleteFlag = false,
                InvestmentType = "Mutual Funds",
                Risk = mutualFundInvestmentModel.Risk,
                ExpectedAmount = mutualFundInvestmentModel.ExpectedAmount
            });

            await _context.SaveChangesAsync();

            return Ok(new { message = "Mutual fund investment added successfully", success = true });
        }






        [HttpPost("newInvestment/bonds")]
        public async Task<ActionResult> AddBondsInvestment(string userEmail, [FromBody] BondsInvestments newBonds)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            newBonds.UserID = user.UserID;

            _context.BondsInvestments.Add(new BondsInvestments
            {
                UserID = newBonds.UserID,
                Amount = newBonds.Amount,
                PurchaseDate = newBonds.PurchaseDate,
                Type = "Bonds",
                Risk = newBonds.Risk,
                ExpectedReturn = newBonds.ExpectedReturn,
                BondName = newBonds.BondName,
                Series = newBonds.Series,
                CouponValue = newBonds.CouponValue,
                Credit = newBonds.Credit,
                MaturityDate = newBonds.MaturityDate,
            });

            _context.InvestmentInfo.Add(new InvestmentModel
            {
                Amount = newBonds.Amount,
                UserID = newBonds.UserID,
                InvestmentName = newBonds.BondName,
                Active = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                DeleteFlag = false,
                InvestmentType = "Bonds",
                Risk = newBonds.Risk,
                ExpectedAmount = (decimal?)newBonds.ExpectedReturn * newBonds.Amount,
            });

            await _context.SaveChangesAsync();

            return Ok(new { message = "Mutual fund investment added successfully", success = true });
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



        [HttpPost("updateInvestment")]
        public async Task<IActionResult> AddInvestment([FromQuery] string userEmail, [FromBody] InvestmentModel updateInvestment)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                if (user == null)
                {
                    return NotFound($"User with email {userEmail} not found.");
                }


                var investmentInfo = new InvestmentModel
                {
                    UserID = user.UserID,
                    InvestmentName = updateInvestment.InvestmentName,
                    InvestmentType = updateInvestment.InvestmentType,
                    Amount = updateInvestment.Amount,
                    Risk = updateInvestment.Risk,
                    Active = true,
                    DeleteFlag = true,
                };

                _context.InvestmentInfo.Add(investmentInfo);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Investment added successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the investment" });
            }
        }



        [HttpPost("requestPlan")]
        public async Task<IActionResult> AddNewPlan(string userEmail, [FromBody] RequestPlan plan)
        {
            try
            {
                if (string.IsNullOrEmpty(userEmail) || plan == null)
                {
                    return BadRequest("Invalid request parameters");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var requestPlan = new RequestPlan
                {
                    UserID = user.UserID,
                    AdvisorID = plan.AdvisorID,
                    InvestmentName = plan?.InvestmentName,
                    InvestmentType = plan?.InvestmentType,
                    Amount = plan.Amount ?? 0,
                    Risk = plan.Risk,
                    Status = "Pending",
                    StartDate = DateTime.Now
                };

                var advisorPlan = new AdvisorPlan
                {
                    UserID = user.UserID,
                    AdvisorID = plan.AdvisorID,
                    InvestmentName = plan?.InvestmentName,
                    InvestmentType = plan?.InvestmentType,
                    Amount = plan.Amount ?? 0,
                    Risk = plan.Risk,
                    Status = "Pending",
                    StartDate = DateTime.Now
                };

                _context.RequestPlans.Add(requestPlan);
                _context.AdvisorPlans.Add(advisorPlan);
                await _context.SaveChangesAsync();

                return Ok(new { message = "New Investment Plan added successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return StatusCode(500, "An error occurred while processing the request");
            }
        }




        [HttpGet("viewPlans")]
        public async Task<ActionResult<IEnumerable<RequestPlan>>> GetPlans(int advisorId)
        {
            var allPlans = await _context.RequestPlans.Where(u => u.AdvisorID == advisorId).ToListAsync();
            return Ok(new { allPlans });
        }


        [HttpGet("viewAdvisorPlans")]
        public async Task<ActionResult<IEnumerable<AdvisorPlan>>> GetAdvisorPlans(int advisorId)
        {
            var allPlans = await _context.AdvisorPlans.Where(u => u.AdvisorID == advisorId).ToListAsync();
            return Ok(new { allPlans });
        }


        [HttpGet("getAdvisors")]
        public async Task<ActionResult<IEnumerable<User>>> GetAdvisors()
        {
            var advisors = await _context.Users.Where(u => u.RoleID == 1337).ToListAsync();

            if (advisors == null || advisors.Count == 0)
            {
                return NotFound();
            }

            return Ok(advisors);
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
