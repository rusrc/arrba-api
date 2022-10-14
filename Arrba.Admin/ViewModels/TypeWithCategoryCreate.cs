using Arrba.Domain.Models;

namespace Arrba.Admin.ViewModels
{
    public class TypeWithCategoryCreate
    {
        public long ItemTypeId { get; set; }
        public long CategoryId { get; set; }

        public string ItemTypeName { get; set; }
        public string ItemTypeComment { get; set; }
        public ActiveStatus ItemTypeStatus { get; set; }

        public WatchWeightStatus ItemTypeWatchWeightStatus { get; set; }
    }
}
