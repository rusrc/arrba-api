using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class User : IdentityUser<long>
    {
        public string UserNickName { get; set; }
        public string UserLastName { get; set; }

        [StringLength(35)]
        public string AvatarImgName { get; set; }

        public DateTime RegistrationDate { get; set; }
        public DateTime LastLogin { get; set; }
        public UserAuthStatus UserStatus { get; set; }

        public virtual ICollection<Dealership> Dealerships { get; set; }
        [JsonIgnore]/*To prevent Self referencing loop detected with type 'Arrba.Domain.Models.AdVehicle'. Path 'User.Ads'.*/
        public virtual ICollection<AdVehicle> Ads { get; set; }
        public virtual ICollection<UserPhone> UserPhones { get; set; }
        public virtual Balance Balance { get; set; }
    }
}
