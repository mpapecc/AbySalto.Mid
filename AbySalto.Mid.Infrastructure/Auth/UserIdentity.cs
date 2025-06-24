using AbySalto.Mid.Application.Interfaces.Auth;
using AbySalto.Mid.Application.Interfaces.Repositories;
using AbySalto.Mid.Application.Models;
using AbySalto.Mid.Domain.Entites;

namespace AbySalto.Mid.Infrastructure.Auth
{
    public class UserIdentity: IUserIdentity
    {
        public UserIdentity()
        {
        }

        public UserIdentity(int userId, IRepository<User> userRepository)
        {
            var user = userRepository.GetById(userId)
                .Select(x => new 
                {
                    Email,
                    FirstName,
                    LastName
                })
                .FirstOrDefault();

            this.Id = new Id(userId);
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
        }


        public string Email { get; set; }
        public Id Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
