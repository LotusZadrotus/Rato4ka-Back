using Rato4ka_back.Models;

namespace Rato4ka_back.DTO
{
    public record UserDetailInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DiscordId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public UserDetailInfoDTO()
        {

        }
        public UserDetailInfoDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            DiscordId = user.DiscordId;
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
                Confirmed = user.IsEmailConfirmed,
                DiscordId = user.DiscordId
            };
        }
    }
}
