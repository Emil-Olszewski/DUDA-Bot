using System;
using System.Collections.Generic;
using Discord_BOT.Misc.Modules.ColorsManagment;

namespace Discord_BOT.Misc.Modules.CasesSystem.CaseTypes
{
    public class ColorCase : Case
    {
        private static readonly int[] BronzeChances = { 100, 0, 0 };
        private static readonly int[] SilverChances = { 99, 1, 0 };
        private static readonly int[] GoldChances = { 96, 3, 1 };

        public ColorCase()
        {
            DropType = Droptype.Color;
            Emoji = ":shirt:";
        }

        public override Prize Open(Key key)
        {
            var randomizer = new Random();
            int random = randomizer.Next(1, 101);
            random += key.Level;
            if (random > 100) random = 100;

            var quality = new QUALITY();
            switch (Quality)
            {
                case QUALITY.BRONZE:
                    quality = GetColorQuality(BronzeChances, random);
                    break;
                case QUALITY.SILVER:
                    quality = GetColorQuality(SilverChances, random);
                    break;
                case QUALITY.GOLD:
                    break;
                default:
                    quality = GetColorQuality(GoldChances, random);
                    break;
            }

            Color color;
            do
            {
                random = randomizer.Next(0, Colors.List.Count);
                color = Colors.List.Find(x => x.ID == random);
                if (color.Quality != quality) continue;
                else break;
            } while (true);

            return new Prize(color.Name, color: color);
        }

        private static QUALITY GetColorQuality(IReadOnlyList<int> chances, int random)
        {
            if (random <= chances[0]) return QUALITY.BRONZE;
            else if (random <= chances[0] + chances[1]) return QUALITY.SILVER;
            else return QUALITY.GOLD;
        }
    }
}
