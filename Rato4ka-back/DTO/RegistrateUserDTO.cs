using Rato4ka_back.Models;

namespace Rato4ka_back.DTO
{
    public record RegistrateUserDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public RegistrateUserDTO(User user)
        {
            Login = user.Login;
            Password = user.Password;
            Email = user.Email;
        }
        public RegistrateUserDTO()
        {

        }
    }
}
