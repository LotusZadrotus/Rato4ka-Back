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
using Rato4ka_back.DTO;
using Rato4ka_back.Exceptions;
using Rato4ka_back.Models;
using Rato4ka_back.Services;
using Rato4ka_back.Services.impl;
using Rato4ka_back.Util;

namespace Rato4ka_back.Controllers
{
    [ApiController]
    [Route("/api/security")]
    public class SecurityController: ControllerBase
    {
        private readonly ISecurityService _security;
        private readonly ILogger<SecurityController> _logger;


        public SecurityController(ISecurityService context, ILogger<SecurityController> logger)
        {
            _security = context;
            _logger = logger;
        }
        [HttpPost]
        [Route("[action]/{login}")]
        public async Task<ActionResult<TokenDTO>> GetToken(string login, string password)
        {
            try
            {
                var item = await _security.Login(login, password);
                return new OkObjectResult(item);
            }
            catch (ServiceException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e) {
                _logger.LogWarning(message: e.Message, exception: e);
                return StatusCode(500);
            }
        }
        [HttpPost]
        [Route("registrate")]
        public async Task<ActionResult<TokenDTO>> Registrate([FromBody]RegistrateUserDTO user)
        {
            try
            {
                await _security.RegistrateUser(user);
                var item = _security.Login(user.Login, user.Password).Result;
                return new OkObjectResult(item);

            }
            catch (ServiceException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogWarning(message: e.Message, exception: e);
                return new BadRequestResult();
            }
        }

        [HttpGet]
        [Route("[action]/{login}")]
        public async Task<ActionResult> Email(string login, string key)
        {
            try 
            { 
                await _security.ConfirmEmail(login, key);
                return new OkObjectResult(true);
            } catch (ServiceException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e) { _logger.LogWarning(message: e.Message, exception: e); return StatusCode(500); }
        }
        
    }
}