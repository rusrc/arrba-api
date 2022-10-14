namespace Arrba.DTO
{
    public class VehicleImageDto
    {
        public string FullFileName { get; set; }
        /// <summary>
        /// 400x300
        /// </summary>
        public string MiddleFileName { get; set; }
        /// <summary>
        /// 200x150
        /// </summary>
        public string SmallFileName { get; set; }
        /// <summary>
        /// 60x45
        /// </summary>
        public string SuperSmallFileName { get; set; }
        public string Path { get; set; }
        public string ImageStatus { get; set; }
    }
}
