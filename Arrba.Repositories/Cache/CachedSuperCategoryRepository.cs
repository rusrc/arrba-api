using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Repositories.EntityFramework.PostgreSQL;
using Arrba.Services;
using Arrba.Services.Logger;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.Cache
{
    public class CachedSuperCategoryRepository : SuperCategoryRepository
    {
        const string KEY = "ListSuperCateg";
        private readonly ILogService _logService;
        public CachedSuperCategoryRepository(DbArrbaContext context, ILogService logService)
        : base(context, logService)
        {
            this._logService = logService;
        }

        public override async Task<IEnumerable<SuperCategory>> GetAllAsync(Expression<Func<SuperCategory, bool>> predicate)
        {
            List<SuperCategory> result = null;

            if ((result = CacheService.GetData<List<SuperCategory>>(KEY)) != null)
            {
                this._logService.Info("ListSuperCateg cached result");
                return result;
            }

            result = await base._context.SuperCategories
                .Include(c => c.Categories)
                .Where(predicate)
                .ToListAsync();

            await CacheService.SetDataAsync(KEY, result);
            this._logService.Info("ListSuperCateg not cached result");

            return result;
        }
    }
}
