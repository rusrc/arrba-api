using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Arrba.Admin.ViewModels
{
    public class UserModelLogin
    {
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
