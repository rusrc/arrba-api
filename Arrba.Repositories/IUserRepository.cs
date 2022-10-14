using System.Threading.Tasks;
using Arrba.Domain.Models;

namespace Arrba.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> CheckUser(string email, string password);
        Task<User> GetAsync(string email, bool loadPhones = false, bool loadBalance = false);
        Task<User> CreateUnauthorizedUserOrGetExistedAsync(string email);
        Task<bool> Add(User user, string password);
    }
}
