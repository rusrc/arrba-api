using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class Dealership
    {
        public long Id { get; set; }
        public long CityId { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string SubwayStations { get; set; }
        public string PhoneNumber { get; set; }
        public string MapCoords { get; set; }
        public bool OfficialDealer { get; set; }

        public string MoWorkTime { get; set; }
        public string TuWorkTime { get; set; }
        public string WeWorkTime { get; set; }
        public string ThWorkTime { get; set; }
        public string FrWorkTime { get; set; }
        public string SaWorkTime { get; set; }
        public string SuWorkTime { get; set; }

        public virtual City City { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public ICollection<AdVehicle> Vehicles { get; set; }

        [NotMapped]
        public string FullWorkTime {
            get
            {
                var list = new List<string>
                {
                    MoWorkTime,
                    TuWorkTime,
                    WeWorkTime,
                    ThWorkTime,
                    FrWorkTime,
                    SaWorkTime,
                    SuWorkTime
                };

                return list.Distinct().Aggregate((prev, next) => $"{prev}, {next}");
            }}
    }

    public class DealershipConfiguration : IEntityTypeConfiguration<Dealership>
    {
        public void Configure(EntityTypeBuilder<Dealership> builder)
        {
            builder.Property(p => p.MoWorkTime).HasMaxLength(15);
            builder.Property(p => p.TuWorkTime).HasMaxLength(15);
            builder.Property(p => p.WeWorkTime).HasMaxLength(15);
            builder.Property(p => p.ThWorkTime).HasMaxLength(15);
            builder.Property(p => p.FrWorkTime).HasMaxLength(15);
            builder.Property(p => p.SaWorkTime).HasMaxLength(15);
            builder.Property(p => p.SuWorkTime).HasMaxLength(15);
        }
    }
}
