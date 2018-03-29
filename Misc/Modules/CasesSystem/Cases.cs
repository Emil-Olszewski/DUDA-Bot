using System.Collections.Generic;
using Discord_BOT.Misc.Modules.CasesSystem.CaseTypes;

namespace Discord_BOT.Misc.Modules.CasesSystem
{
    public static class Cases
    {
        public static List<Case> List;
        static Cases()
        {
            List = new List<Case>()
            {
                new CashCase()
                {
                    ID = 100,
                    Name = ":lock: Brazowa Skrzynka Dudow",
                    Price = 500,
                    Prizes = new[]
                    {
                        new Prize("100 000 DUDOW!", cash: 100000, chance: 998),
                        new Prize("5 000 DUDOW!", cash: 5000, chance: 990),
                        new Prize("3 000 DUDOW!", cash: 3000, chance: 980),
                        new Prize("2 000 DUDOW!", cash: 2000, chance: 970),
                        new Prize("1 000 DUDOW!", cash: 1000, chance: 960),
                        new Prize("800 DUDOW!", cash: 800, chance: 900),
                        new Prize("500 Dudow", cash: 500, chance: 600),
                        new Prize("300 Dudow", cash: 300, chance: 300),
                        new Prize("200 Dudow", cash: 200),
                    },
                    Quality = Item.QUALITY.BRONZE
                },
                new CashCase()
                {
                    ID = 101,
                    Name = ":lock: Srebrna Skrzynka Dudow",
                    Price = 4000,
                    Prizes = new[]
                    {
                        new Prize("800 000 DUDOW!", cash: 800000, chance: 998),
                        new Prize("40 000 DUDOW!", cash: 40000, chance: 990),
                        new Prize("24 000 DUDOW!", cash: 24000, chance: 980),
                        new Prize("16 000 DUDOW!", cash: 16000, chance: 970),
                        new Prize("8 000 DUDOW!", cash: 8000, chance: 960),
                        new Prize("6 400 DUDOW!", cash: 6400, chance: 900),
                        new Prize("4 000 Dudow", cash: 4000, chance: 600),
                        new Prize("2 400 Dudow", cash: 2400, chance: 300),
                        new Prize("1 600 Dudow", cash: 1600),
                    },
                    Quality = Item.QUALITY.SILVER
                },
                new CashCase()
                {
                    ID = 102,
                    Name = ":lock: Zlota Skrzynka Dudow",
                    Price = 36000,
                    Prizes = new[]
                    {
                        new Prize("7 200 000 DUDOW!", cash: 7200000, chance: 998),
                        new Prize("320 000 DUDOW!", cash: 320000, chance: 990),
                        new Prize("192 000 DUDOW!", cash: 192000, chance: 980),
                        new Prize("128 000 DUDOW!", cash: 128000, chance: 970),
                        new Prize("64 000 DUDOW!", cash: 64000, chance: 960),
                        new Prize("57 600 DUDOW!", cash: 57600, chance: 900),
                        new Prize("36 000 Dudow", cash: 36000, chance: 600),
                        new Prize("21 600 Dudow", cash: 21600, chance: 300),
                        new Prize("14 400 Dudow", cash: 14400),
                    },
                    Quality = Item.QUALITY.GOLD
                },

                new ColorCase()
                {
                    ID = 103,
                    Name = ":lock: Brazowa Skrzynka Kolorow",
                    Price = 50000,
                    Prizes = new Prize[] { },
                    Quality = Item.QUALITY.BRONZE
                },

                new ColorCase()
                {
                    ID = 104,
                    Name = ":lock: Srebrna Skrzynka Kolorow",
                    Price = 200000,
                    Prizes = new Prize[] { },
                    Quality = Item.QUALITY.SILVER
                },

                new ColorCase()
                {
                    ID = 105,
                    Name = ":lock: Zlota Skrzynka Kolorow",
                    Price = 890000,
                    Prizes = new Prize[] { },
                    Quality = Item.QUALITY.GOLD
                }
            };
        }
    }
}
