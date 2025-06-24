using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AbySalto.Mid.Application.Interfaces.Auth;
using AbySalto.Mid.Application.Interfaces.Repositories;
using AbySalto.Mid.Application.Models.Auth;
using AbySalto.Mid.Domain.Entites;
using AbySalto.Mid.Infrastructure.ConfigurationOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AbySalto.Mid.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IRepository<User> userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly JwtOptions jwtOptions;

        public AuthService(
            IRepository<User> userRepository,
            IPasswordHasher<User> passwordHasher, 
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtOptions> jwtOptions)
        {
            this.passwordHasher = passwordHasher;
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.jwtOptions = jwtOptions.Value;
        }

        public AuthResponse Login(Login login)
        {
            var user = this.userRepository.Query().Where(x => x.Email == login.Email).FirstOrDefault();
            if (user == null) 
                return new AuthResponse { IsSuccess = false, Message = "User does not exists for provided credentials" };

            if (!VerifyHashedPassword(user, login.Password))
                return new AuthResponse { IsSuccess = false, Message = "Wrong password" };

            this.httpContextAccessor.HttpContext.Response.Cookies.Append("Refresh-Token", GenerateRefreshToken());

            return new AuthResponse
            {
                IsSuccess = true,
                Message = $"Welcome {user.FirstName}!",
                Data = new { AccessToken = GenerateJwtToken(user.Id) }
            };
        }

        private bool VerifyHashedPassword(User user, string password)
        {
            return passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Success;
        }

        public string GenerateJwtToken(int userId)
        {
            var claims = new[]
            {
                new Claim("UserId", userId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtOptions.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = this.jwtOptions.Issuer,
                Audience = this.jwtOptions.Audience,
                Expires = DateTime.UtcNow.AddMinutes(this.jwtOptions.ExpirationInMinutes),
                SigningCredentials = creds,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public AuthResponse Register(Registration registration)
        {
            var user = this.userRepository.Query().Where(x => x.Email == registration.Email).FirstOrDefault();
            if (user != null)   
                return new AuthResponse { IsSuccess = false, Message = "User with same email already exists" };

            var newUser = new User
            {
                Email = registration.Email,
                FirstName = registration.FirstName,
                LastName = registration.LastName
            };

            newUser.PasswordHash = HashPassword(newUser, registration.Password);

            this.userRepository.Insert(newUser);
            this.userRepository.Commit();

            return new AuthResponse { IsSuccess = true, Message = $"Welcome {registration.FirstName}!" };
        }

        private string HashPassword(User user, string password)
        {
            return passwordHasher.HashPassword(user, password);
        }
    }
}
