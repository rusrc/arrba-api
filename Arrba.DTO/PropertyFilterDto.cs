using System.Collections.Generic;
using Arrba.Domain.Models;

namespace Arrba.DTO
{
    public class PropertyFilterDto
    {
        public long CategID { get; set; }
        public long PropertyID { get; set; }
        public int Priority { get; set; }
        public string ControlType { get; set; }
        public string AddToCard { get; set; }
        public string AddToFilter { get; set; }
        public string PropertyName { get; set; }
        public string PropertyDescription { get; set; }
        public string PropertyActiveStatus { get; set; }
        public string PropertyGroupName { get; set; }
        public ICollection<SelectOption> SelectOptions { get; set; }
    }
}

