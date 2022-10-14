using System.Collections.Generic;

namespace Arrba.DTO
{
    // TODO simplify the class to much unused properties
    public class CategoryWithBrandsDto
    {
        public long SuperCategoryId { get; set; }
        public long CategoryId { get; set; }
        public long VehicleCount { get; set; }
        public string BrandName { get; set; }
        public string SuperCategoryAlias { get; set; }
        public string SuperCategoryName { get; set; }
        public string CategoryAlias { get; set; }
        public string CategoryName { get; set; }
        public string CategoryFileName { get; set; }
        public IEnumerable<object> Brands { get; set; }
    }
}
