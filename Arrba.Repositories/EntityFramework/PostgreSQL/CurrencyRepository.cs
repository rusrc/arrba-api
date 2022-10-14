using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{

    public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(DbArrbaContext context)
            :base(context)
        {
        }

        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            var exchangeRateTable = from c in _context.Currencies
                join cr in _context.CurrencyRates on c.ID equals cr.CurrencyBaseRateID
                join c2 in _context.Currencies on cr.CurrencyID equals c2.ID
                select new ExchangeRate
                {
                    TargetName = c.Name,
                    TargetCurrencyId = cr.CurrencyBaseRateID,
                    SourceCurrencyName = c2.Name,
                    SourceCurrencyId = cr.CurrencyID,
                    FaceValue = cr.FaceValue,
                    Rate = cr.Rate
                };


            return exchangeRateTable.ToList();
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync()
        {
            var exchangeRateTable = from c in  _context.Currencies
                join cr in _context.CurrencyRates on c.ID equals cr.CurrencyBaseRateID
                join c2 in _context.Currencies on cr.CurrencyID equals c2.ID
                select new ExchangeRate
                {
                    TargetName = c.Name,
                    TargetCurrencyId = cr.CurrencyBaseRateID,
                    SourceCurrencyName = c2.Name,
                    SourceCurrencyId = cr.CurrencyID,
                    FaceValue = cr.FaceValue,
                    Rate = cr.Rate
                };


            return await exchangeRateTable.ToListAsync();
        }
    }
}
