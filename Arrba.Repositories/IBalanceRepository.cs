using System.Threading.Tasks;

namespace Arrba.Repositories
{
    public interface IBalanceRepository
    {
        Task<bool> AddAmount(long userId, double amount, long currencyId);
        Task<bool> SubtractAmountAsync(long userId, double amount, long currencyId);
    }
}