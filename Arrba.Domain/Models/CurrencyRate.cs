using System.ComponentModel.DataAnnotations.Schema;

namespace Arrba.Domain.Models
{
    public class CurrencyRate
    {
        public long ID { get; set; }
        public long CurrencyID { get; set; }

        [ForeignKey("Currency")]
        public long CurrencyBaseRateID { get; set; }

        public int FaceValue { get; set; }
        public float Rate { get; set; }


        [InverseProperty("CurrencyBaseRates")]
        public virtual Currency CurrencyBaseRate { get; set; }

        [InverseProperty("CurrencyRates")]
        public virtual Currency Currency { get; set; }
    }
}