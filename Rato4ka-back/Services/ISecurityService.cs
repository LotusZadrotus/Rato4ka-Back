using Rato4ka_back.DTO;
using System.Threading.Tasks;

namespace Rato4ka_back.Services
{
    public interface ISecurityService
    {
        Task<string> Login(string login, string password);
        Task ConfirmEmail(string login, string key);
        Task RegistrateUser(RegistrateUserDTO user);
    }
}
