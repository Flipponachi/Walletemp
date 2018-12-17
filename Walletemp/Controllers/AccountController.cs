using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Walletemp.DAL;
using Walletemp.Dto;
using Walletemp.Models;
using Walletemp.Services;

namespace Walletemp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IAttendanceRegistrationService _attendanceRegistration;
        private readonly AttendanceContext _context;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            IAttendanceRegistrationService attendanceRegistration,
           AttendanceContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _attendanceRegistration = attendanceRegistration;
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            var userAcc = await _userManager.FindByNameAsync(model.Email);

            if (result.Succeeded)
            {
               var accessToken = await GenerateJwtToken(model.Email, userAcc);

                //Register attendance for a day
                await _attendanceRegistration.RegistrationForToday(userAcc.Id);
                //no attendance registration for weekend
                return Ok(accessToken);
            }

            return BadRequest("Incorrect username or password");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Fill in the necessary fields");


            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                Fullname = model.Name
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                //Create employee record

                var newEmpy = new Employee
                {
                    Fullname = model.Name,
                    UserId = user.Id
                };

                _context.Employees.Add(newEmpy);
                await _context.SaveChangesAsync();
                return Ok("Account Created successfully");
            }

            return BadRequest("Username already exists");
        }

      
        private async Task<object> GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

      

        
    }
}