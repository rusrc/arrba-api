using System.ComponentModel.DataAnnotations.Schema;

namespace Arrba.Domain.ModelsView
{
    [NotMapped]
    public class MapJsonCoord
    {
        /// <summary>
        ///     Google, yandex, 2gis
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        ///     Долгота
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        ///     Широта
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        ///     Высота
        /// </summary>
        public double Altitude { get; set; }
    }
}