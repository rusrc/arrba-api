using System.ComponentModel.DataAnnotations;

namespace Arrba.Domain.Models
{
    public class ExchangeRate
    {
        [Key]
        public string TargetName { get; set; }
        public long TargetCurrencyId { get; set; }
        public string SourceCurrencyName { get; set; }
        public long SourceCurrencyId { get; set; }
        public long FaceValue { get; set; }
        public float Rate { get; set; }
    }
}