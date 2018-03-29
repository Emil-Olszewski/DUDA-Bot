using System;
using System.Linq;
using Discord_BOT.Core.UserAccounts;
using Discord_BOT.Misc.Modules.CasesSystem;
using Discord_BOT.Misc.Modules.ColorsManagment;

namespace Discord_BOT.Misc.Modules
{
    public class Prize
    {
        public string Name { get; }
        public long Cash { get; private set; }
        public Case Case { get; }
        public Color Color { get; }
        public Key Key { get; }
        public int Chance { get; }

        public Prize(string name = "", long cash = 0, Case @case = null, Color color = null, Key key = null, int chance = 0)
        {
            Cash = cash;
            Case = @case;
            Color = color;
            Key = key;
            Chance = chance;

            Name = name == "" ? GetName() : name;
        }

        private string GetName()
        {
            if (Case is null == false) return Case.Name;
            else if (Color is null == false) return Color.Name;
            else if (Key is null == false) return Key.Name;
            else return "UNDEFINED";
        }

        public static Prize GetRandomPrize(Prize[] prizes, int bonus = 0)
        {
            var randomizer = new Random();
            double random = randomizer.Next(0, 1000);
            random += bonus * 10;

            if (random > 999) random = 999;
            int numberOfPrize = prizes.TakeWhile(p => !(random >= p.Chance)).Count();

            return prizes[numberOfPrize];
        }

        public long GiveAndReturnProfit(UserAccount user)
        {
            long profit = Cash;
            if (Key != null)
            {
                profit += Key.Price;
                user.Inventory.Add(Key);
            }

            if (Case != null)
            {
                profit += Case.Price;
                user.Inventory.Add(Case);
            }

            if (Color != null)
            {
                profit += Color.Price;
                user.Inventory.Add(Color);
            }

            user.Cash += Cash;
            profit += Cash;
            return profit;
        }
    }
}
