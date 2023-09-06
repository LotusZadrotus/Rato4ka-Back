#nullable enable
using Rato4ka_back.Models;

namespace Rato4ka_back.DTO
{

    public record UserDTO
    {
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public bool IsAdmin { get; set; }
        public UserDTO()
        {

        }
        public UserDTO(User user)
        {
            Name = user.Name;
            Desc = user.Description;
            IsAdmin = user.IsAdmin;
        }
        public static implicit operator User(UserDTO user)
        {
            return new User
            {
                Name = user.Name,
                IsAdmin = user.IsAdmin
            };
        }
    }
}
