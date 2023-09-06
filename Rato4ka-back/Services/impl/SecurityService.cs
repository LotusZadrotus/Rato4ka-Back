using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Rato4ka_back.DTO;
using Rato4ka_back.Exceptions;
using Rato4ka_back.Models;
using Rato4ka_back.Repositories;
using Rato4ka_back.Repositories.impl;
using Rato4ka_back.Util;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rato4ka_back.Services.impl
{
    public class SecurityService : ISecurityService
    {
        public readonly IUnit _unit;
        public const string LOGIN_ERROR = "Incorrect login or password";
        public readonly IMailService _mailService;
        public readonly ILogger<SecurityService> _logger;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly Random _random = new Random();

        public SecurityService(IUnit unit, IMailService mailService, ILogger<SecurityService> logger)
        {
            _unit = unit;
            _logger = logger;
            _mailService = mailService;
        }

        public async Task ConfirmEmail(string login, string key)
        {
            try
            {
                var cred = await _unit.GetCred(login);
                if(cred == null)
                {
                    throw new ServiceException("There was no such user or you have already confirmed your email"); 
                }
                if(cred.Login == login && cred.Key == key)
                {
                    var user = await _unit.GetUserByLogin(login);
                    user.Confirmed = true;
                    await _unit.DeleteCred(login);
                    await _unit.SaveAsync();
                }
            }catch (InvalidOperationException e)
            {
                throw new ServiceException(e.Message);
            }
            catch (Exception e) {
                _logger.LogWarning(e.Message, e);
                throw new ApplicationException($"Error happened in SecurityService by method ConfirmEmail.{Environment.NewLine}Message: {e.Message}{Environment.NewLine}Inner Exception: {e.InnerException.Message}{Environment.NewLine}Login: {login}{Environment.NewLine}");
            }
        }

        public async Task<TokenDTO> Login(string login, string password)
        {
            try
            {
                var identity = await GetIdentity(login, password);
                var now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromDays(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                return new TokenDTO(token, login);
            }
            catch(InvalidOperationException e) { throw new ServiceException(e.Message); }
            catch(Exception e)
            {
                _logger.LogWarning(e.Message, e);
                throw new ApplicationException($"Error happened in SecurityService by method Login.{Environment.NewLine}Message: {e.Message}{Environment.NewLine}Inner Exception: {e.InnerException.Message}{Environment.NewLine}Login: {login}{Environment.NewLine}");
            }
        }

        public async Task RegistrateUser(RegistrateUserDTO user)
        {
            try
            {
                byte[] salt = new byte[128 / 8];
                using (var rngCsp = RandomNumberGenerator.Create())
                {
                    rngCsp.GetNonZeroBytes(salt);
                }
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: user.Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
                var toCreate = new User()
                {
                    Email = user.Email,
                    IsAdmin = false,
                    Name = user.Login,
                    Login = user.Login,
                    Confirmed = false
                };
                toCreate.Password = hashed;
                toCreate.Salt = Convert.ToBase64String(salt);
                var key = Enumerable.Repeat(chars, 128)
                    .Select(s => s[_random.Next(s.Length)]).ToArray();
                var link = Enumerable.Repeat(chars, 64)
                    .Select(s => s[_random.Next(s.Length)]).ToArray();
                toCreate = await _unit.GetRepo<User>().AddAsync(toCreate);
                await _unit.AddCred(new Cred()
                {
                    Login = user.Login,
                    ExpDate = DateTime.Now.Add(TimeSpan.FromDays(10)).ToUniversalTime(),
                    IsEmail = true,
                    Key = new string(key)
                });
                await _unit.GetRepo<LinksUsers>().AddAsync(new LinksUsers()
                {
                    Id = toCreate.Id,
                    Link = new string(link)
                });
                await _unit.SaveAsync();
                _mailService.SentMail(user.Email, MailSubject.Registration, user.Login, new string(key));
                
            }
            catch (InvalidOperationException e)
            {
                throw new ServiceException(e.Message);
            }
            catch (Exception e)
            {
                var npge = e.InnerException as Npgsql.PostgresException;
                if (npge != null)
                {
                    
                    switch (npge.SqlState)
                    {
                        case "23505":
                            throw new ServiceException("Such loggin already exist", "login");
                        
                            
                    }
                }
                _logger.LogWarning($"{$"Something went wrong in method CreateUser: {e.Message}"}{user}", e); 
                throw new ApplicationException($"Error happened in SecurityService by method RegistrateUser.{Environment.NewLine}Message: {e.Message}{Environment.NewLine}Inner Exception: {e.InnerException.Message}{Environment.NewLine}User: {user}");
            }
        }
        private async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            var user = await _unit.GetUserByLogin(login);
            if(user == null)
            {
                throw new InvalidOperationException(LOGIN_ERROR);
            }
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
            throw new InvalidOperationException(LOGIN_ERROR);
        }
    }
}
