using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class Comment
    {
        public long ID { get; set; }
        /// <summary>
        ///     ID объявления, которому пренадлежит комментарий
        /// </summary>
        public long AdVehicleID { get; set; }

        /// <summary>
        ///     ID-шник подкомментария наследника
        /// </summary>
        public long? CommentParentID { get; set; }

        /// <summary>
        ///     ID пользователя осатвившего комментарий
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        ///     ID пользователя, которому было написан комментарий
        /// </summary>
        public long ForUserID { get; set; }

        /// <summary>
        ///     Предложение об обмене, ID объявления, который предлагает пользователь
        /// </summary>
        public long? OfferAdID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [StringLength(2048)]
        public string Text { get; set; }

        /// <summary>
        ///     Количество клэймов или жалоб от пользователей
        /// </summary>
        public int? ClaimCount { get; set; }


        [JsonIgnore]
        [ForeignKey("CommentParentID")]
        public virtual Comment CommentParent { get; set; }

        public virtual ICollection<Comment> CommentParents { get; set; }

        [NotMapped]
        public string DateShort => string.Concat(Date.ToShortDateString(), " ", Date.ToShortTimeString());
        [NotMapped]
        public virtual AdVehicle OfferAd { get; set; }
        [NotMapped]
        public virtual string UserName { get; set; }
        [NotMapped]
        public virtual string ForUserName { get; set; }
    }
}