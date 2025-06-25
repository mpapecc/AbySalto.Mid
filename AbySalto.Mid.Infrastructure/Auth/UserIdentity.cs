using AbySalto.Mid.Application.Extensions;
using AbySalto.Mid.Application.Interfaces.Auth;
using AbySalto.Mid.Application.Interfaces.Repositories;
using AbySalto.Mid.Application.Models;
using AbySalto.Mid.Domain.Entites;
using Microsoft.Extensions.Caching.Memory;

namespace AbySalto.Mid.Infrastructure.Auth
{
    public class UserIdentity: IUserIdentity
    {
        private readonly IRepository<User> userRepository;
        private readonly IMemoryCache cache;

        public UserIdentity()
        {
        }

        public UserIdentity(int userId, IRepository<User> userRepository, IMemoryCache cache)
        {
            var key = $"user-{userId}";

            var entryFactory = (ICacheEntry entry) =>
            {
                return userRepository.GetById(userId)
                        .Select(x => new
                        {
                            x.Email,
                            x.FirstName,
                            x.LastName
                        }).FirstOrDefault();
            };

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Today.AddDays(1)
            };

            var user = cache.GetOrCreate(key, entryFactory, cacheOptions);

            this.Id = new Id(userId);
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.userRepository = userRepository;
            this.cache = cache;
        }



        public string Email { get; set; }
        public Id Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
