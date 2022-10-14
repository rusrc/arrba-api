// using Arrba.ImageLibrary.ModelViews;

using System.Collections.Generic;

namespace Arrba.DTO
{
    public class VehicleWithPropertyDto : VehicleDto
    {
        //public ICollection<AdImgModelView> Images { get; set; }
        public ICollection<object> Images { get; set; }
        public string CombinedProperties { get; set; }
        public ICollection<PropertyDto> Properties { get; set; }
    }
}
