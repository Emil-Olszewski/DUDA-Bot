using System;

namespace Discord_BOT.Modules
{
    class Prize
    {
        public string Name { get; }
        public int Keys { get; }
        public int Cases { get; }
        public int Cash { get; }
        public int Chance { get; }

        public Prize(string name, int keys = 0, int cases = 0, int cash = 0, int chance = 0)
        {
            Name = name;
            Keys = keys;
            Cases = cases;
            Cash = cash;
            Chance = chance;
       }

        static public Prize GetRandomPrize(Prize[] prizes, int scale)
        {
            Random randomizer = new Random();
            double random = randomizer.Next(0, scale);
            int numberOfPrize = 0;

            foreach (var p in prizes)
            {
                if (random >= p.Chance) break;
                numberOfPrize++;
            }

            return prizes[numberOfPrize];
        }
    }
}
