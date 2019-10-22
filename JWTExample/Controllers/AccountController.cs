using JWTExample.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IOptions<AppConfig> _options;
        UserManager<IdentityUser> _userManager;
        
        public AccountController(UserManager<IdentityUser> userManager, IOptions<AppConfig> options)
        {
            _options = options;
            _userManager = userManager;
        }

        // GET: api/Account
        [Authorize]
        [HttpGet]
        public IEnumerable<IdentityUser> Get()
        {
            return _userManager.Users;
        }

        // GET: api/Account/5
        [HttpGet("{username}")]
        public async Task<IdentityUser> Get(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        // POST: api/Account
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByNameAsync(loginViewModel.Username);
            var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

            if (passwordIsCorrect)
            {
                var secretKey = _options.Value.Secret;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claimsList = new[] { new Claim(ClaimTypes.Name, user.Id) };

                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44364", 
                    audience: "https://localhost:44364",
                    claims: new List<Claim>(claimsList),
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: signinCredentials);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpPost("register")]
        public async Task Register([FromBody] LoginViewModel loginViewModel)
        {
            IdentityUser user = new IdentityUser(loginViewModel.Username);
            var result = await _userManager.CreateAsync(user, loginViewModel.Password);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("userinfo")]
        public string GetCurrentUser()
        {
            return User.Identity.Name;
        }
    }
}
