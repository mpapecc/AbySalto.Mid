using AbySalto.Mid.Application.Models;

namespace AbySalto.Mid.Application.Interfaces.Auth
{
    public interface IUserIdentity
    {
        public Id Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
