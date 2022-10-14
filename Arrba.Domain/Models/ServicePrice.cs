using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arrba.Domain.Models
{
    public class ServicePrice
    {
        public long ID { get; set; }
        public string SevicePriceName { get; set; }

        public double Price { get; set; }
        public ServiceEnum ServiceType { get; set; }
    }

    public class ServicePriceConfiguration : IEntityTypeConfiguration<ServicePrice>
    {
        public void Configure(EntityTypeBuilder<ServicePrice> builder)
        {
            builder.HasIndex(p => p.SevicePriceName).IsUnique();
            builder.Property(p => p.SevicePriceName).HasMaxLength(20);
        }
    }
}