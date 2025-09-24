using System.Threading.Tasks;

namespace Reclutamiento.WebApp.Services
{
    public interface IAuthService
    {
        Task<string> LoginWithGithub();
        Task Logout();
    }
}
