using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Arrba.Constants;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class Category : MultiLangPropertyExtension
    {
        public long ID { get; set; }
        public long SuperCategID { get; set; }
        public long CategGroupID { get; set; }
        public string Alias { get; set; }

        [Display(Name = "Спрятать модель при подаче")]
        public bool HideModelField { get; set; }

        [Display(Name = "Singular name Json")]
        public string NameMultiLangSingularJson { get; set; }
        [JsonIgnore]
        public virtual SuperCategory SuperCateg { get; set; }

        [JsonIgnore]
        public virtual CategGroup CategGroup { get; set; }

        [JsonIgnore]
        public virtual ICollection<ItemModel> Models { get; set; }

        [JsonIgnore]
        public virtual ICollection<CategType> CategTypes { get; set; }

        [JsonIgnore]
        public virtual ICollection<CategBrand> CategBrands { get; set; }

        [JsonIgnore]
        public virtual ICollection<PropertyCateg> PropertyCategs { get; set; }

        [JsonIgnore]
        public virtual ICollection<AdVehicle> AdVehicles { get; set; }

        [StringLength(120)]
        public override string Name { get; set; }

        public string FileName { get; set; }
        public ActiveStatus Status { get; set; }

        /// <summary>
        ///     Полный путь к главной картинки объявления. Имеет зависимость от константы <see cref="Ad.CategID" /> |
        ///     Full path to main images. The mathod has dipendoncy from constans <see cref="Ad.CategID" />
        /// </summary>
        //[NotMapped]
        //public virtual string MainImg => CONSTANT.CATEG_FOLDER_PATH + FileName;

        /// <summary>
        /// Get one value based on current language
        /// </summary>
        [NotMapped]
        public string NameMultiLangSingular
        {
            get { return NameMultiLangSingularJsonObject.Value; }
            set
            {
                if (value == null) throw new ArgumentException(nameof(value));
            }
        }

        /// <summary>
        /// Return <see cref="NameMultiLangSingularJson"/> 
        /// object formed from json with words in sigular form 
        /// </summary>
        [NotMapped]
        public NameMultiLangJson NameMultiLangSingularJsonObject => this.MakeNameMultiLangJson(this.NameMultiLangSingularJson);
    }
}