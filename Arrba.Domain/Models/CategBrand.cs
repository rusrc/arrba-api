using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arrba.Domain.Models
{
    public class CategBrand
    {
        public long CategID { get; set; }
        public long BrandID { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Categ { get; set; }
    }

    public class CategBrandConfiguration : IEntityTypeConfiguration<CategBrand>
    {
        public void Configure(EntityTypeBuilder<CategBrand> builder)
        {
            builder.HasKey(p => new {p.CategID, p.BrandID});
        }
    }
}