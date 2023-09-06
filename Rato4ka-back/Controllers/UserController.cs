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
using Rato4ka_back.DTO;
using System.Security.Claims;
using Rato4ka_back.Exceptions;

namespace Rato4ka_back.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserSevice _users;
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger, IUserSevice users)
        {
            _logger = logger;
            _users = users;
        }
        [HttpGet]
        [Route("[action]")]
        public async IAsyncEnumerable<UserDTO> GetUsers()
        {
            var toReturn = _users.GetUsers();
            await foreach (var item in toReturn)
            {
                yield return item;
            }
            //try
            //{
                
            //}
            //catch (Exception e)
            //{
            //    _logger.Log(LogLevel.Error, e.Message);
            //    return new StatusCodeResult(500);

            //}
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("[action]")]
        public IEnumerable<User> GetUserByIdSecured(int id)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        [Route("[action]/{link}")]
        public async Task<ActionResult<UserDTO>> GetUserByLink(string link)
        {
            try
            {
                var toReturn = await _users.GetUser(link);
                return new OkObjectResult(toReturn);

            }
            catch (ServiceException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Warning, exception:e,message:$"Something went wrong in \"GetUserById\"\n\tGotParameter: \n\t\tLink: {link}\n");
                return new StatusCodeResult(500);
            };
        }
        [Authorize]
        [HttpGet]
        [Route("profile")]
        public async Task<ActionResult<UserDetailInfoDTO>> GetUserByLogin()
        {
            try
            {
                int userId;
                if(Int32.TryParse(User.Claims.First(x => x.Type == "id").Value, out userId)){
                    var toReturn = await _users.GetUserDetailInfoById(userId);
                    return new OkObjectResult(toReturn);
                }
                _logger.LogWarning("Error in GetUserByLogin method in UserController. User was authorized but failed to obtain profile");

                return new StatusCodeResult(500);

            }
            catch (ServiceException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Warning, exception:e,message:$"Something went wrong in \"GetUserByLogin\"\n\tGotParameter: \n");
                return new StatusCodeResult(500);

            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                throw new NotImplementedException();

            }
            catch (Exception e)
            {
                _logger.LogWarning($"Something went wrong in method DeleteUser: {e.Message}\n{id}", e);
                return new StatusCodeResult(500);

            }
        }

        [Authorize]
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult<UserDetailInfoDTO>> UpdateUser([FromBody] UserUpdateDTO user)
        {
            try
            {
                int userId;
                if (Int32.TryParse(User.Claims.First(x => x.Type == "id").Value, out userId))
                {
                    var userToUpdate = _users.GetUserById(userId);
                     
                    //var toReturn = await _users.UpdateUser(userToUpdate);
                    return new OkObjectResult(toReturn);
                }
                _logger.LogWarning("Error in UpdateUser method in Usercontroller. User was authorized but failed to obtain profile");
                return new StatusCodeResult(500);

            }
            catch (ServiceException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, "Error in method UpdateUser" + Environment.NewLine + user.ToString());
                return new StatusCodeResult(500);

            }
        }
        [HttpGet]
        [Route("[action]/{link}")]
        public async Task<ActionResult> GetImage(string link)
        {
            try
            {
                throw new NotImplementedException();

            }
            catch (ServiceException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message, e);
                return new StatusCodeResult(500);

            }
        }
        [Authorize]
        [HttpPatch]
        [Route("[action]")]
        public async Task<ActionResult> UpdateUserAvatar([FromBody] Byte[] avatar)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (ServiceException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message, e);
                return new StatusCodeResult(500);
            }
        }
        
    }
}