using System.Collections.Generic;

namespace Arrba.Domain.Models
{
    public class CategGroup
    {
        public long ID { get; set; }
        public string Name { get; set; }


        public virtual ICollection<Category> Categs { get; set; }
    }
}