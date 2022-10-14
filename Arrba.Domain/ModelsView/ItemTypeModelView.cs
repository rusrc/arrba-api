using Arrba.Domain.Models;

namespace Arrba.Domain.ModelsView
{
    public class ItemTypeModelView
    {
        public long ID { get; set; }
        public long CategId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public long ClicksPerDay { get; set; }
        public WatchWeightStatus Weight { get; set; }
        public ActiveStatus Status { get; set; }
    }
}