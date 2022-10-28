using Microsoft.Extensions.Logging;
using Rato4ka_back.DTO;
using Rato4ka_back.Exceptions;
using Rato4ka_back.Models;
using Rato4ka_back.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Rato4ka_back.Services.impl
{
    public class UserService : IUserSevice
    {
        private readonly IUnit unit;
        private readonly ILogger<UserService> _logger;
        public UserService(IUnit unit, ILogger<UserService> logger)
        {
            this.unit = unit;
            _logger = logger;
        }

        public async Task DeleteUser(UserDTO user)
        {
            try
            {
                await unit.GetRepo<User>().DeleteAsync(user);
                await unit.SaveAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                var ex = new ServiceException($"Error happened in UserService by method DeleteUser.{Environment.NewLine}Message: {e.Message}{Environment.NewLine}Inner Exception: {e.InnerException.Message}", $"User : {user}");
                throw ex;
            }
        }

        public Task DeleteUser(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<UserDTO> GetUser(string link)
        {
            try
            {
                var id = await unit.GetIdByLink(link);
                if (id is null)
                {
                    throw new InvalidOperationException("Can't find a User by that link");
                }
                return new UserDTO(await unit.GetRepo<User>().GetAsync(id.Value));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                var ex = new ServiceException($"Error happened in UserService by method GetUser.{Environment.NewLine}Message: {e.Message}{Environment.NewLine}Inner Exception: {e.InnerException.Message}", $"Link : {link}");
                throw ex;
            }
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            try
            {
                var item = await unit.GetRepo<User>().GetAsync(id);
                if (item is null)
                {
                    throw new InvalidOperationException("Can't find a User with such id");
                }
                return new UserDTO(item);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                var ex = new ServiceException($"Error happened in UserService by method GetUserById.{Environment.NewLine}Message: {e.Message}{Environment.NewLine}Inner Exception: {e.InnerException.Message}", $"Id : {id}");
                throw ex;
            }
        }

        public async Task<UserDetailInfoDTO> GetUserDetailInfo(string link)
        {
            try
            {
                
                var id = await unit.GetIdByLink(link);
                if (id is null)
                {
                    throw new InvalidOperationException("Can't find a User by that link");
                }
                return new UserDetailInfoDTO(await unit.GetRepo<User>().GetAsync(id.Value));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                var ex = new ServiceException($"Error happened in UserService by method GetUserDetailInfo.{Environment.NewLine}Message: {e.Message}{Environment.NewLine}Inner Exception: {e.InnerException.Message}", $"Link : {link}");
                throw ex;
            }
        }

        public async Task<UserDetailInfoDTO> GetUserDetailInfoById(Int32 id)
        {
            try
            {
                var item = await unit.GetRepo<User>().GetAsync(id);
                if (item is null)
                {
                    throw new InvalidOperationException("Can't find a User with such id");
                }
                return new UserDetailInfoDTO(item);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                var ex = new ServiceException($"Error happened in UserService by method GetUserDetailInfoById.{Environment.NewLine}Message: {e.Message}{Environment.NewLine}Inner Exception: {e.InnerException.Message}", $"Id : {id}");
                throw ex;
            }
        }

        public async Task<byte[]> GetUserImage(string link)
        {
            try
            {
                var id = await unit.GetIdByLink(link);
                if (id is null)
                {
                    throw new InvalidOperationException("Can't find a User by that link");
                }
                return unit.GetRepo<User>().GetAsync(id.Value).Result.Avatar;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                var ex = new ServiceException($"Error happened in UserService by method GetUserImage.{Environment.NewLine}Message: {e.Message}{Environment.NewLine}Inner Exception: {e.InnerException.Message}", $"Link : {link}");
                throw ex;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            try
            {
                var item = await unit.GetRepo<User>().GetListAsync();
                if (item is null)
                {
                    throw new InvalidOperationException("Can't find a User with such id");
                }
                return item.Select(x=>new UserDTO(x));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                var ex = new ServiceException($"Error happened in UserService by method GetUsers.{Environment.NewLine}Message: {e.Message}{Environment.NewLine}Inner Exception: {e.InnerException.Message}");
                throw ex;
            }
        }
        public async Task UpdateUser(UserDetailInfoDTO user)
        {
            try
            {
                var toUpdate = await unit.GetRepo<User>().GetAsync(user.Id);
                toUpdate.Name = user.Name;
                await unit.GetRepo<User>().UpdateAsync(toUpdate);
                await unit.SaveAsync();
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                throw new ServiceException($"Error happened in UserService by method UpdateUser.{Environment.NewLine}Message: {e.Message}{Environment.NewLine}Inner Exception: {e.InnerException.Message}{Environment.NewLine}User: {user}");
            }
        }
        public async Task UpdateUserAvatar(int id, byte[] avatar)
        {
            try
            {
                var toUpdate = await unit.GetRepo<User>().GetAsync(id);
                toUpdate.Avatar = avatar;
                await unit.GetRepo<User>().UpdateAsync(toUpdate);
                await unit.SaveAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new ServiceException($"Error happened in UserService by method UpdateUserAvatar.{Environment.NewLine}Message: {e.Message}{Environment.NewLine}Inner Exception: {e.InnerException.Message}{Environment.NewLine}Id: {id}{Environment.NewLine}Image: {avatar}");
            }
        }
    }
}
