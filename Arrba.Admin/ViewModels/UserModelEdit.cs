using Arrba.Domain.Models;

namespace Arrba.Admin.ViewModels
{
    public class UserModelEdit
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public UserAuthStatus UserStatus { get; set; }
        public string PhoneNumber { get; set; }
        public long RoleId { get; set; }
    }
}
