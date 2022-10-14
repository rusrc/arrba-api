using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class SelectOption : MultiLangPropertyExtension
    {
        public long ID { get; set; }
        public long PropertyID { get; set; }
        public override string Name { get; set; }
        public string MetaDate { get; set; }


        [JsonIgnore]
        public virtual Property Property { get; set; }
    }

    public class SelectOptionConfiguration : IEntityTypeConfiguration<SelectOption>
    {
        public void Configure(EntityTypeBuilder<SelectOption> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(100);
        }
    }
}