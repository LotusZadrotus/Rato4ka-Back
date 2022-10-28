using Rato4ka_back.Models;

namespace Rato4ka_back.DTO
{
    public record RegistrateUserDTO: UserDetailInfoDTO
    {
        public string Password { get; set; }
        public byte[] Avatar { get; set; }
        public RegistrateUserDTO(User user)
            :base(user)
        {
            Password = user.Password;
            Avatar = user.Avatar;
        }
        public RegistrateUserDTO()
        {

        }
    }
}
