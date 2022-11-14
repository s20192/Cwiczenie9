using Exercise9.LoggingServices;
using Exercise9.Models;
using Exercise9.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Exercise9.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private MainDbContext _mainDbContext;
        private readonly IConfiguration _configuration;
        private ILoggerManager _logger;

        public AccountsController(IConfiguration configuration, MainDbContext mainDbContext, ILoggerManager logger)
        {
            _configuration = configuration;
            _mainDbContext = mainDbContext;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterUser(RegisterRequest registerRequest)
        {
            var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(registerRequest.Password);

            var user = new AppUser()
            {
                Email = registerRequest.Email,
                Login = registerRequest.Login,
                Password = hashedPasswordAndSalt.Item1,
                Salt = hashedPasswordAndSalt.Item2,
                RefreshToken = SecurityHelpers.GenerateRefreshToken(),
                RefreshTokenExp = DateTime.Now.AddDays(1)
            };

            _mainDbContext.Users.Add(user);
            _mainDbContext.SaveChanges();

            return Ok();
        }



        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            AppUser user = _mainDbContext.Users.Where(u => u.Login == loginRequest.Login).FirstOrDefault();

            string passwordHashFromDb = user.Password;
            string curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginRequest.Password, user.Salt);

            if(passwordHashFromDb != curHashedPassword)
            {
                return Unauthorized();
            }

            Claim[] userclaim = new[] {
                    new Claim(ClaimTypes.Name, "s20192"),
                    new Claim(ClaimTypes.Role, "user"),
                    new Claim(ClaimTypes.Role, "admin")
                };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: userclaim,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
            user.RefreshTokenExp = DateTime.Now.AddDays(1);
           _mainDbContext.SaveChanges();

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = user.RefreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromHeader(Name = "Authorization")] string token, RefreshTokenRequest refreshToken)
        {
            AppUser user = _mainDbContext.Users.Where(u => u.RefreshToken == refreshToken.RefreshToken).FirstOrDefault();
            if(User == null)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            if(user.RefreshTokenExp< DateTime.Now)
            {
                throw new SecurityTokenException("Refresh token expires");
            }

            var login = SecurityHelpers.GetUserIdFromAccessToken(token.Replace("Bearer ", ""), _configuration["SecretKey"]);
            Claim[] userClaim = new[]
            {
                new Claim(ClaimTypes.Name, "s20192"),
                    new Claim(ClaimTypes.Role, "user"),
                    new Claim(ClaimTypes.Role, "admin")
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwttoken = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: userClaim,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
            user.RefreshTokenExp = DateTime.Now.AddDays(1);
            _mainDbContext.SaveChanges();

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(jwttoken),
                refreshToken = user.RefreshToken
            });

        }
    }
}
