using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arrba.Domain.Models
{
    public class Balance
    {
        [Key]
        [ForeignKey("User")]
        public long UserID { get; set; }

        public double Amount { get; set; }
        public DateTime LastAddDate { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<BalanceTransaction> BalanceTransactions { get; set; }
    }
}