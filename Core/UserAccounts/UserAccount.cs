using System;
using System.Collections.Generic;
using Discord_BOT.Modules.ColorsManagment;

namespace Discord_BOT.Core.UserAccounts
{
    public class UserAccount
    {
        public ulong ID { get; set; }

        private int _cash;
        private int _xp;
        
        public int Cash
        {
            get { return _cash; }
            set
            {
                if (value <=0) _cash = 0;
                else _cash = value;
            }
        }

        public int XP
        {
            get { return _xp; }
            set
            {
                _xp = value;
                while(_xp >= (2 * Math.Pow(Level + 1, 3)) + 50)
                {
                    Level++;
                }
            }
        }

        public uint Level { get; set; }
        public uint NumberOfCases { get; set; }
        public uint NumberOfKeys { get; set; }
        public uint OpenedCases { get; set; }
        public uint CasesProfit { get; set; }
        public List<Color> Equipment { get; set; }
        public uint ActuallyColor { get; set; }
        
        public DateTime last500Use { get; set; }
        public DateTime last10Use { get; set; }
        public DateTime lastTransfer { get; set; }
        public DateTime lastRoulette { get; set; }
        public DateTime lastCaseOpen { get; set; }
    }
}
