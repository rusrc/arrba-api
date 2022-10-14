using System.ComponentModel.DataAnnotations;

namespace Arrba.Domain.Models
{
    public class FeedBack 
    {
        public long ID { get; set; }
        public string Email { get; set; }
        [MaxLength(512)]
        [Required]
        public string Text { get; set; }
    }
}
