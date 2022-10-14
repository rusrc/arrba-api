using System.ComponentModel.DataAnnotations;
using Arrba.Domain.Models;

namespace Arrba.Admin.ViewModels
{
    public class UserModelCreate
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public UserAuthStatus Status { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public long RoleId { get; set; }
    }
}
