using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Arrba.Domain.Models;

namespace Arrba.Repositories
{
    public interface IPropertyRepository : IRepository<Property>
    {
        Task<IEnumerable<Property>> GetAllAsync(bool addOptions, Expression<Func<Property, bool>> predicate);
    }
}