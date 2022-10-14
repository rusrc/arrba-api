//using Arrba.Domain.Models.News;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class Country : MultiLangPropertyExtension
    {
        [Key]
        public long ID { get; set; }
        public override string Name { get; set; }
        public string Alias { get; set; }

        /// <summary>
        ///     The name that matched with country
        /// </summary>
        [MaxLength(6)]
        public string FirstDomainName { get; set; }

        public ActiveStatus ActiveStatus { get; set; }

        /// <summary>
        ///     If you don't want to show prices with any currencies.
        ///     Kazahstan for example initialize stupid law that restrict any foreign currencies
        /// </summary>
        public ActiveStatus UseNativeCurrencyOnly { get; set; }

        [JsonIgnore]
        public virtual ICollection<Region> Regions { get; set; }

        [JsonIgnore]
        public virtual ICollection<City> Cities { get; set; }

    }
}