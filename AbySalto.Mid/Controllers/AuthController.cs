using AbySalto.Mid.Application.Interfaces.Auth;
using AbySalto.Mid.Application.Models.Auth;
using AbySalto.Mid.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Mid.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly IUserIdentity userIdentity;

        public AuthController(IAuthService authService, IUserIdentity userIdentity)
        {
            this.authService = authService;
            this.userIdentity = userIdentity;
        }

        [HttpPost(nameof(Login))]
        public AuthResponse Login(Login login)
        {
           return this.authService.Login(login);
        }

        [HttpPost(nameof(Register))]
        public AuthResponse Register(Registration registration)
        {
            return this.authService.Register(registration);
        }

        [Authorize]
        [HttpGet(nameof(WhoAmI))]
        public IUserIdentity WhoAmI()
        {
            return this.userIdentity;
        }
    }
}
