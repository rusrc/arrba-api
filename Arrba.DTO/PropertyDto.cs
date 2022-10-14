using System.Collections.Generic;
using Arrba.Domain.Models;

namespace Arrba.DTO
{
    public class PropertyDto
    {
        public long PropertyID { get; set; }
        public string PropertyValue { get; set; }
        public string PropertyName { get; set; }
        public string Description { get; set; }
        public string UnitMeasure { get; set; }
        public string PropertyType { get; set; }
        public string PropertyStatus { get; set; }
        public ICollection<SelectOption> SelectOptions { get; set; }
    }
}
