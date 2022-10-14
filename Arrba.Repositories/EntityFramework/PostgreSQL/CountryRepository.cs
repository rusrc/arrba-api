using Arrba.Domain;
using Arrba.Domain.Models;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(DbArrbaContext context) 
            : base(context)
        {
        }
    }
}
