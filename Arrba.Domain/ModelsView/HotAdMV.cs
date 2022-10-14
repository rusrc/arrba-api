using Arrba.ImageLibrary.Json;

namespace Arrba.Domain.ModelsView
{
    public class HotAdMV
    {
        public long ID { get; set; }
        public long CategID { get; set; }
        public string FolderImgName { get; set; }
        public string ModelName { get; set; }
        public string TypeName { get; set; }
        public string CityName { get; set; }
        public string Text { get; set; }
        public double Price { get; set; }
        public string CurrencyName { get; set; }
        public string Year { get; set; }
        public string ImgJson { get; set; }
        public ImgJson ImgJsonObject { get; set; }

        public string Title { get; set; }
    }
}