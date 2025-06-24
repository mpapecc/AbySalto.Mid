using AbySalto.Mid.Application.Models.Auth;

namespace AbySalto.Mid.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        AuthResponse Login(Login login);
        AuthResponse Register(Registration registration);
    }
}
