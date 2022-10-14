using System.ComponentModel.DataAnnotations;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public enum PriceSorter
    {
        [Display(Name = "SorterPriceDescending")]
        PriceDesc = 1,
        [Display(Name = "SorterPriceAscending")]
        PriceAsc = 2,
        [Display(Name = "SorterDateDescending")]
        DateDesc = 0,
        [Display(Name = "SorterDateAscending")]
        DateAsc = 3
    }
}