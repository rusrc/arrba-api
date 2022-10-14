using System.Collections.Generic;

namespace Arrba.DTO
{
    public class SuperCategoryWithBrandsDto
    {
        public string CityName { get; set; }
        public long SuperCategoryId { get; set; }
        public string SuperCategoryName { get; set; }
        public string SuperCategoryAlias { get; set; }
        public IEnumerable<CategoryWithBrandsDto> Categories { get; set; }
    }
}
