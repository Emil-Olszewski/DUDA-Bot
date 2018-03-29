using System;

namespace Discord_BOT.Misc.Modules.Investment
{
    public class Investment
    {
        public uint Id { get; set; }
        public ulong CustomerId { get; set; }
        public DateTime PaymentDate { get; set; }
        public TimeSpan InvestmentDuration { get; set; }
        public int PaymentedMoney { get; set; }
        public int MoneyToWithdraw { get; set; }

        public Investment(uint id, ulong customerId, DateTime date, TimeSpan duration, int paymentedMoney)
        {
            Id = id;
            CustomerId = customerId;
            PaymentDate = date;
            InvestmentDuration = duration;
            PaymentedMoney = paymentedMoney;
            MoneyToWithdraw = paymentedMoney + CountProfit(paymentedMoney, (int)duration.TotalHours);
        }

        private static int CountProfit(int amount, int hours)
        {
            double multiplier = (double)hours / Config.Bot.InvestmentTimeToDoubleMoney;
            double profit = multiplier * amount;
            return (int)profit;
        }
    }
}
