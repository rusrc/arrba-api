using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain.Models;

namespace Arrba.Repositories
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        IEnumerable<ExchangeRate> GetExchangeRates();
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync();
    }
}
