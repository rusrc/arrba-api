using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.DTO;
using Arrba.Services.Logger;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private ILogService _logService;
        public BrandRepository(DbArrbaContext context, ILogService logService)
            : base(context)
        {
            _logService = logService;
        }

        public virtual async Task<Brand> GetAsync(string brandName)
        {
            return await this.GetAsync(b => b.Name.Equals(brandName, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<IEnumerable<CategoryWithBrandsDto>> GetCategoriesWithBrandsAsync(
            int vehiclesCount, Expression<Func<AdVehicle, bool>> selectByCityIdPredicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<AdVehicle> query = _context.AdVehicles;

            if (selectByCityIdPredicate != null)
            {
                query = query.Where(selectByCityIdPredicate);
            }

            var query2 =
                (from av in query.Include(v => v.Categ).Include(v => v.SuperCateg)
                 // where av.Categ.Status == ActiveStatus.active
                 // where av.SuperCateg.Status == ActiveStatus.active
                 group av by new { av.BrandID, av.CategID, av.SuperCategID } into g
                 select new
                 {
                     superCategoryId = g.Key.SuperCategID,
                     categoryId = g.Key.CategID,
                     brandId = g.Key.BrandID,
                     vehicleCount = g.Count()
                 })
                .OrderBy(x => x.superCategoryId)
                .Take(vehiclesCount);

            var query3 =
                (from bc in query2
                 join superCategory in _context.SuperCategories on bc.superCategoryId equals superCategory.ID
                 join category in _context.Categories on bc.categoryId equals category.ID
                 join brand in _context.Brands on bc.brandId equals brand.ID
                 select new CategoryWithBrandsDto
                 {
                     SuperCategoryId = superCategory.ID,
                     CategoryId = category.ID,
                     VehicleCount = bc.vehicleCount,
                     BrandName = brand.Name,
                     SuperCategoryAlias = superCategory.Alias,
                     SuperCategoryName = superCategory.Name,
                     CategoryAlias = category.Alias,
                     CategoryName = category.Name,
                     CategoryFileName = category.FileName
                 });

            return await query3.ToListAsync(cancellationToken);
        }
    }
}
