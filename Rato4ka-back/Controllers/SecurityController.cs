using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Rato4ka_back.Models;
using Rato4ka_back.Util;

namespace Rato4ka_back.Controllers
{
    [ApiController]
    [Route("/api/security")]
    public class SecurityController:ControllerBase
    {
        private readonly DBContext _context;
        private readonly ILogger<SecurityController> _logger;

        public SecurityController(DBContext context, ILogger<SecurityController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpPost]
        [Route("[action]/{login}")]
        public async Task<IActionResult> GetToken(string login, string password)
        {
            _logger.LogInformation(login + password);
            var identity = GetIdentity(login, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
 
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromDays(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
 
            var response = new
            {
                access_token = encodedJwt,
                login = identity.Name
            };
 
            return new JsonResult(response);
        }

        [HttpGet]
        [Route("[action]/{login}")]
        public async Task<IActionResult> Email(string login, string key)
        {
            try
            {
                var item = await _context.Credentials.FirstOrDefaultAsync(x => x.Login == login);
                if (key.Equals(item.Key) && item.IsEmail && DateTime.Now < item.ExpDate)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
                    user.Confirmed = true;
                    _context.Credentials.Remove(item);
                    await _context.SaveChangesAsync();
                }
                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Warning, message:e.Message, exception:e);
                return new BadRequestResult();
            }
        }
        private ClaimsIdentity GetIdentity(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login==login);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(user.Salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            if (user.Password == hashed)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim("id", Convert.ToString(user.Id)),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.IsAdmin ? "Admin": "User"),
                };
                _logger.LogInformation(claims[1].Value);
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}