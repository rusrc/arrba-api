using Newtonsoft.Json;

namespace Arrba.ImageLibrary.ModelViews
{
    public enum AdImageStatus
    {
        //[Display(Name = "Не основная фотография объявления")]
        /// <summary>
        /// Не основная фотография объявления
        /// </summary>
        NotMain,
        //[Display(Name = "Основная фотография объявления")]
        /// <summary>
        /// Основная фотография объявления
        /// </summary>
        Main
    }

    public class AdImgDto
    {
        [JsonIgnore]
        public long AdVehicleID { get; set; }
        /// <summary>
        /// 1024x720
        /// </summary>
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
        public AdImageStatus ImageStatus { get; set; }
        [JsonIgnore]
        public int CurImgCount { get; set; }
    }
}
