using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rato4ka_back.Models;
using Rato4ka_back.Services;
using Rato4ka_back.Util;
using System.Drawing.Imaging;

namespace Rato4ka_back.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly IMailService _mailService;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly Random _random= new Random();
        private readonly string api_url = "https://rato4ka.net/api/";
        public UserController(ILogger<UserController> logger, DBContext dbContext, IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
            _context = dbContext;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<User>> GetUsers()
        {
            try
            {
                var toReturn = await _context.Users.Select(v=>
                    new
                    {
                        id=v.Id,
                        
                        email=v.Email,
                        name=v.Name,
                        discordId=v.DiscordId,
                        isAdmin=v.IsAdmin
                    }).ToListAsync();
                if (toReturn.Count==0)
                {
                    return new NotFoundResult();
                }
                return new JsonResult(toReturn);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                throw new Exception($"method: {this.GetType()}" ,e);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<User>> GetAllAdmins()
        {
            try
            {
                var toReturn = await _context.Users.Where(v => v.IsAdmin).Select(v=>
                    new
                    {
                        id=v.Id,
                        
                        email=v.Email,
                        name=v.Name,
                        discordId=v.DiscordId,
                        isAdmin=v.IsAdmin
                    }).ToListAsync();
                if (toReturn.Count==0)
                {
                    return new NotFoundObjectResult(toReturn);
                }
                return new JsonResult(toReturn);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return new BadRequestResult();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("[action]")]
        public IEnumerable<User> GetUserByIdSecured(int id)
        {
            return _context.Users.Where(v => v.Id == id);
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var item = await _context.Users.Where(x=>x.Id==id).Select(x=> new
                {
                    id = x.Id,
                    avatar = String.Concat(api_url, "/User/GetImage/", _context.Links.First(y=>y.Id == x.Id).Link),
                    email = x.Email,
                    confirmed = x.Confirmed,
                    name = x.Name,
                    isAdmin = x.IsAdmin,
                    discoredId = x.DiscordId,
                    isOwned = x.Login.Equals(User.Identity.Name),
                    link = _context.Links.First(y=>y.Id==x.Id).Link
                }).FirstOrDefaultAsync();
                if (item is null)
                {
                    return new NotFoundObjectResult(id);
                }
                return new JsonResult(item);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Warning, exception:e,message:$"Something went wrong in \"GetUserById\"\n\tGotParameter: \n\t\tid: {id}\n");
                return new BadRequestObjectResult(error: e);
            };
        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<User>> GetUserByLogin()
        {
            try
            {
                var item = await _context.Users.Where(x=>x.Login == User.Identity.Name).Select(x=>new
                {
                    x.Id,
                    
                    x.Email,
                    x.Confirmed,
                    x.Name,
                    x.IsAdmin,
                    x.DiscordId,
                    x.Login
                }).FirstOrDefaultAsync();
                if (item == null)
                {
                    return new NotFoundObjectResult(User.Identity.Name);
                }
                return new JsonResult(item);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Warning, exception:e,message:$"Something went wrong in \"GetUserByLogin\"\n\tGotParameter: \n");
                return new BadRequestObjectResult(error: e);
            }
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<User>> MailTest()
        {
            _mailService.SentMail("nikitamikhalchenko@gmail.com", MailSubject.Registration, null, null);
            return new OkResult();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateUser([FromBody]User user)
        {
            try
            {
                if (user.Password is null || user.Email is null || user.Login is null || user.IsAdmin)
                    return new BadRequestResult();
                
                byte[] salt = new byte[128 / 8];
                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetNonZeroBytes(salt);
                }
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: user.Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
                user.Password = hashed;
                user.Salt = Convert.ToBase64String(salt);
                var key = Enumerable.Repeat(chars, 128)
                    .Select(s => s[_random.Next(s.Length)]).ToArray();
                var link = Enumerable.Repeat(chars, 64)
                    .Select(s => s[_random.Next(s.Length)]).ToArray();
                await _context.Users.AddAsync(user);
                await _context.Credentials.AddAsync(new Cred()
                {
                    Login = user.Login,
                    ExpDate = DateTime.Now.Add(TimeSpan.FromDays(10)),
                    IsEmail = true,
                    Key = new string(key)
                });
                await _context.Links.AddAsync(new Links()
                {
                    Id = user.Id,
                    Link = Convert.ToString(link)
                });
                _mailService.SentMail(user.Email, MailSubject.Registration, user.Login, new string(key));
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Warning, message:e.Message, exception:e);
                _logger.LogWarning($"{$"Something went wrong in method CreateUser: {e.Message}"}{user}", e);
                return new BadRequestObjectResult(e.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var userToDelete = new User(){Id = id};
                _context.Users.Attach(userToDelete);
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Something went wrong in method DeleteUser: {e.Message}\n{id}");
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> UpdateUser([FromBody] User user)
        {
            try
            {
                _logger.LogInformation(user.ToString());
                var userId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);
                if (userId != user.Id) return new BadRequestObjectResult("U are not owner of this user");
                var entity = await _context.Users.FindAsync(user.Id);
                entity.Name = user.Name;
                entity.Avatar = user.Avatar;
                entity.Email = user.Email;
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return new OkObjectResult(entity);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, "Error in method UpdateUser" + '\n' + user.ToString());
                return new BadRequestObjectResult(e);
            }
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<User>> GetImage(string id)
        {
            try
            {
                var item = _context.Users.FirstOrDefault(x => x.Id == _context.Links.First(y=>y.Link == id).Id)
                    ?.Avatar;
                //_logger.LogInformation(item.ToString());
                
                return File(item,"image/jpeg");
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                throw new Exception($"method: {this.GetType()}" ,e);
            }
        }
        
    }
}