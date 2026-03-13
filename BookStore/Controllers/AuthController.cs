using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Controllers
{
    
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid user data.");
            }
            // Check if the email is already registered
            var existingUser = _context.Users
                          .FirstOrDefault(x => x.Email == user.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }


            user.Role = "User";

            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(jwtSettings["DurationInMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost]
        public IActionResult Login(LoginModel login)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Email == login.Email &&
                                     x.Password == login.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(login);
            }

            var token = GenerateJwtToken(user);

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(60)
            });

            if (user.Role == "Admin")
            {
                return RedirectToAction("AdminDashboard", "Home");
            }
            else
            {
                return RedirectToAction("UserDashboard", "Home");
            }
        }

        


    }
}
