using Rato4ka_back.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rato4ka_back.Services
{
    public interface IUserSevice
    {
        public Task<UserDTO> GetUserById(int id);
        public Task<UserDTO> GetUser(string link);
        public Task<UserDetailInfoDTO> GetUserDetailInfo(string link);
        public Task<UserDetailInfoDTO> GetUserDetailInfoById(Int32 id);
        public Task<byte[]> GetUserImage(string link);
        public Task<IEnumerable<UserDTO>> GetUsers();
        public Task<UserDetailInfoDTO> GetUserSecured(int id);
        public Task DeleteUser(UserDTO user);
        public Task DeleteUser(int id);
        public Task UpdateUser(UserDetailInfoDTO user);
    }
}
