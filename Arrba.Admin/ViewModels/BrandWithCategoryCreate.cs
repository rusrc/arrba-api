using Arrba.Domain.Models;

namespace Arrba.Admin.ViewModels
{
    public class BrandWithCategoryCreate
    {
        public long BrandId { get; set; }
        public string BrandName { get; set; }
        public long CategoryId { get; set; }
        public ActiveStatus Status { get; set; }
        public WatchWeightStatus WatchWeightStatus { get; set; }
    }
}
