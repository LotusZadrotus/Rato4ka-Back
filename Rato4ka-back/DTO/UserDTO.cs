#nullable enable
using Rato4ka_back.Models;

namespace Rato4ka_back.DTO
{

    public record UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? DiscordId { get; set; }
        public bool IsAdmin { get; set; }
        public UserDTO()
        {

        }
        public UserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            DiscordId = user.DiscordId;
            IsAdmin = user.IsAdmin;
        }
        public static implicit operator User(UserDTO user)
        {
            return new User
            {
                Id = user.Id,
                Name = user.Name,
                IsAdmin = user.IsAdmin,
                DiscordId = user.DiscordId
            };
        }
    }
}
