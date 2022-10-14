using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arrba.Domain.Models
{
    public class PropertyCateg
    {
        public long PropertyID { get; set; }
        public long CategID { get; set; }
        public AddToFilter AddToFilter { get; set; }
        public AddToCard AddToCard { get; set; }

        public long Priority { get; set; }

        public virtual Property Property { get; set; }
        public virtual Category Categ { get; set; }
    }

    public class PropertyCategConfiguration : IEntityTypeConfiguration<PropertyCateg>
    {
        public void Configure(EntityTypeBuilder<PropertyCateg> builder)
        {
            builder.HasKey(p => new { p.PropertyID, p.CategID });
        }
    }
}