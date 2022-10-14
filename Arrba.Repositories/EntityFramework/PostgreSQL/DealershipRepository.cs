using Arrba.Domain;
using Arrba.Domain.Models;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class DealershipRepository : Repository<Dealership>, IDealershipRepository
    {
        public DealershipRepository(DbArrbaContext context) : base(context)
        {
        }


    }
}
