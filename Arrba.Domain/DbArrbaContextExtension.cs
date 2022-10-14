using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Domain
{
    public static class DbArrbaContextExtension
    {
        public static bool AllMigrationsApplied(this DbArrbaContext context)
        {
            return !context.Database.GetPendingMigrations().Any();
        }

        public static void SeedData(this DbArrbaContext context)
        {
            var seedRequired =
                !context.SuperCategories.Any() &&
                !context.Categories.Any() &&
                !context.Brands.Any() &&
                !context.ItemModels.Any() &&
                !context.Cities.Any();

            if (seedRequired)
            {
                context.Database.ExecuteSqlCommand("");
            }
        }
    }
}
