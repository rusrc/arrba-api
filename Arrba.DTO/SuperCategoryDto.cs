using System.Collections.Generic;

namespace Arrba.DTO
{
    public class SuperCategoryDto
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }
    }
}
