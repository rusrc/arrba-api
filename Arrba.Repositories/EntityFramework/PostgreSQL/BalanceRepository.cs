using System;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Constants;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class BalanceRepository : IBalanceRepository
    {
        public DbArrbaContext ctx;
        public BalanceRepository(DbArrbaContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<bool> AddAmount(long userId, double Price, long CurrencyID)
        {
            try
            {
                var balance = ctx.Balances.SingleOrDefault(b => b.UserID == userId);
                balance.Amount += balance.Amount + Price;

                ctx.Entry(balance).State = EntityState.Modified;
                ctx.BalanceTransactions.Add(new BalanceTransaction
                {
                    BalanceUserID = userId,
                    Amount = Price,
                    CurrencyID = CurrencyID,
                    DateTransaction = DateTime.Now,
                    PaymentSourceID = 0
                });

                if (await ctx.SaveChangesAsync() > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Отнять сумму из баланса
        /// </summary>
        /// <param name="price">Сумма</param>
        /// <param name="currencyID">ID валюты</param>
        /// <returns></returns>
        public async Task<bool> SubtractAmountAsync(long userId, double price, long currencyID = 0)
        {
            currencyID = currencyID == 0 ? SETTINGS.DefaultCurrencyID : currencyID;
            var balance = ctx.Balances.SingleOrDefault(b => b.UserID == userId);

            if (balance == null)
                throw new Exceptions.BusinessCriticalLogicException($@"Can't get balance by UserID {userId}");

            if (balance.Amount >= price)
            {
                balance.Amount -= price;
            }
            else
            {
                var msg = string.Format("MsgErrorNotEnougthMoneyToPayForService",
                        balance.Amount, price);

                throw new Exceptions.BusinessUserLogicException(msg);
            }

            //ctx.Entry(balance).State = EntityState.Modified;
            ctx.BalanceTransactions.Add(new BalanceTransaction
            {
                BalanceUserID = userId,
                Amount = price,
                CurrencyID = currencyID,
                DateTransaction = DateTime.Now,
                PaymentSourceID = 0,
                BalanceTransactionType = Domain.Models.BalanceTransactionType.Substract
            });

            if (await ctx.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
