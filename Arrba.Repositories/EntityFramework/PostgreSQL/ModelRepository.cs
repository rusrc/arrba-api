using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class ModelRepository : Repository<ItemModel>, IModelRepository
    {
        public ModelRepository(DbArrbaContext context) : base(context)
        {
        }

        public virtual async Task<ItemModel> GetAsync(string modelName, long brandId)
        {
            var brand = await _context
                .Brands
                .Include(b => b.ItemModels)
                .SingleOrDefaultAsync(b => b.ID == brandId);

            return brand?.ItemModels?.SingleOrDefault(m => m.Name.Equals(modelName, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<IEnumerable<ItemModel>> GetModels(long categId, long brandId)
        {
            var result = await (from m in _context.ItemModels
                                where m.CategID == categId && m.BrandID == brandId
                                select m).ToListAsync();

            if (!result.Any())
            {
                return new List<ItemModel>
                {
                    new ItemModel { Name = "Н/Д"}
                };
            }

            return result;
        }

        public async Task<IEnumerable<ItemModel>> GetWithBrand()
        {
            var models = await _context.ItemModels.Include(m => m.Brand).ToListAsync();
            return models;
        }
    }
}
