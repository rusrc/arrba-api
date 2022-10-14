using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Arrba.Domain.Models;

namespace Arrba.Domain.ModelsView
{
    public class PropertyModelView
    {
        public long ID { get; set; }

        public long PropertyGroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Значение свойства")]
        public string Value { get; set; }

        public ControlTypeEnum ControlType { get; set; }
        public ActiveStatus ActiveStatus { get; set; }

        public virtual ICollection<SelectOption> SelectOptions { get; set; }
    }
}