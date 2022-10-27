using Rato4ka_back.Models;

namespace Rato4ka_back.DTO
{
    public record UserDetailInfoDTO: UserDTO
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public UserDetailInfoDTO()
        {

        }
        public UserDetailInfoDTO(User user)
            :base(user)
        {
            Login = user.Login;
            Email = user.Email;
            IsEmailConfirmed = user.Confirmed;
        }
        public static implicit operator User(UserDetailInfoDTO user)
        {
            return new User
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                Name = user.Name,
                IsAdmin = user.IsAdmin,
                Confirmed = user.IsEmailConfirmed,
                DiscordId = user.DiscordId
            };
        }
    }
}
