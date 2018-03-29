using System;
using Discord_BOT.Misc;
using Discord_BOT.Misc.Modules.InventorySystem;

namespace Discord_BOT.Core.UserAccounts
{
    public class UserAccount
    {
        public ulong Id { get; set; }

        private long _cash;
        private long _xp;

        public long Cash
        {
            get => _cash;
            set => _cash = value <= 0 ? 0 : value;
        }

        public long Xp
        {
            get => _xp;
            set
            {
                _xp = value;
                Level = LevelCounter.WhichLevelWith(_xp);
            }
        }

        public uint Level { get; set; }
        public uint OpenedCases { get; set; }
        public uint CasesProfit { get; set; }
        public int ActuallyColor { get; set; }
        public bool Vip { get; set; }
        public Inventory Inventory { get; set; }
        public TimeSpan TimeSpendOnDiscord { get; set; }
        public DateTime LastBigDotationUse { get; set; }
        public DateTime LastLittleDotationUse { get; set; }
        public DateTime LastKitVipUse { get; set; }
        public DateTime LastTransfer { get; set; }
    }
}
