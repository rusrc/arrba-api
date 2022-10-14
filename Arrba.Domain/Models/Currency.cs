using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class Currency
    {
        public long ID { get; set; }
        public long CountryID { get; set; }
        public int Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }

        [JsonIgnore]
        public virtual ICollection<AdVehicle> Ads { get; set; }
        [JsonIgnore]
        public virtual ICollection<CurrencyRate> CurrencyBaseRates { get; set; }
        [JsonIgnore]
        public virtual ICollection<CurrencyRate> CurrencyRates { get; set; }
        [JsonIgnore]
        public virtual ICollection<BalanceTransaction> BalanceTransactions { get; set; }
    }

    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(p => p.Name).HasMaxLength(3);
            builder.Property(p => p.Symbol).HasMaxLength(3);
        }
    }
}