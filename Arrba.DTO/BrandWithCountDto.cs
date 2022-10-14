namespace Arrba.DTO
{
    public class BrandWithCountDto
    {
        public long CategoryId { get; set; }
        public long BrandId { get; set; }
        public string BrandName { get; set; }
        public int BrandVehiclesCount { get; set; }  
    }
}
