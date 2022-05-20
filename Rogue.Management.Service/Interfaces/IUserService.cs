using Rogue.Management.View.Model;

namespace Rogue.Management.Service.Interfaces
{
    public interface IUserService
    {
        Task<Response<AuthenticationResult>> RegisterAsync(RegisterViewModel userRegisterModel);

        Task<Response<AuthenticationResult>> LoginAsync(LoginViewModel userLoginModel);
    }
}
