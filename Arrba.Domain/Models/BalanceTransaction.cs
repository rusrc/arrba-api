using System;

namespace Arrba.Domain.Models
{
    public class BalanceTransaction
    {
        public long ID { get; set; }
        public long BalanceUserID { get; set; }
        public long PaymentSourceID { get; set; }
        public long CurrencyID { get; set; }
        public double Amount { get; set; }
        public DateTime DateTransaction { get; set; }
        public BalanceTransactionType BalanceTransactionType { get; set; }


        public virtual Currency Currency { get; set; }
        public virtual Balance Balance { get; set; }
    }
}