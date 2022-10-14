using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arrba.Domain.Models
{
    public class CategType
    {
        public long CategID { get; set; }
        public long ItemTypeID { get; set; }


        public virtual Category Categ { get; set; }
        public virtual ItemType ItemType { get; set; }
    }

    public class CategTypeConfiguration : IEntityTypeConfiguration<CategType>
    {
        public void Configure(EntityTypeBuilder<CategType> builder)
        {
            /* Equvelent of
              [Key]
              [Column(Order = 1)]
            */
            builder.HasKey(p => new { p.CategID, p.ItemTypeID });
        }
    }
}