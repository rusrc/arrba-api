using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Arrba.Domain.Models
{
    public class UserPhone
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        [StringLength(16)]
        public string Number { get; set; }
        public PriorityStatus PriorityStatus { get; set; }
        public ActiveStatus Status { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}