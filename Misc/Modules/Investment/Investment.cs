using System;
using Discord_BOT.Core.UserAccounts;

namespace Discord_BOT.Misc.Modules.Investment
{
    public class Investment 
    {
        public uint ID { get; set; }
        public ulong CustomerID { get; set; }
        public DateTime PaymentDate { get; set; }
        public TimeSpan InvestmentDuration { get; set; }
        public int PaymentedMoney { get; set; }
        public int MoneyToWithdraw { get; set; }

        public Investment(uint id, ulong customerID, DateTime date, TimeSpan duration, int paymentedMoney)
        {
            ID = id;
            CustomerID = customerID;
            PaymentDate = date;
            InvestmentDuration = duration;
            PaymentedMoney = paymentedMoney;
            MoneyToWithdraw = paymentedMoney + CountProfit(paymentedMoney, (int)duration.TotalHours);
        }

        private int CountProfit(int amount, int hours)
        {
            double multiplier = (double)hours / 8;
            double profit = multiplier * amount;
            Console.WriteLine($"Hebe {amount} i {hours}");
            return (int)profit;
        }
    }
}
