using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Arrba.DTO
{
    public class SignUpDto
    {
        [Required]
        [JsonProperty("grecaptchaToken")]
        public string GrecaptchaToken { get; set; }
        [Required]
        [JsonProperty("email")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
