using Discord_BOT.Misc.Modules.UpgradingSystem;

namespace Discord_BOT.Misc.Modules.CasesSystem
{
    public class Key : Item, IUpgradable
    {
        public int BaseID { get; set; }
        public string BaseName { get; set; }
        public int Level { get; set; }
        public int MaxLevel { get; set; }

        public Key(QUALITY quality, int bonus)
        {
            Level = 0;
            MaxLevel = 9;

            Type = TYPE.KEY;
            Quality = quality;
            Level = bonus;
            SetID();
            SetName();
            SetPrice();
        }

        public void SetName()
        {
            Name = $"{BaseName} +{Level}";
        }

        public void SetID()
        {
            switch (Quality)
            {
                case QUALITY.GOLD:
                    BaseName = ":key: Zloty kluczyk";
                    BaseID = 260;
                    break;

                case QUALITY.SILVER:
                    BaseName = ":key: Srebrny kluczyk";
                    BaseID = 230;
                    break;

                case QUALITY.BRONZE:
                    BaseName = ":key: Brazowy kluczyk";
                    BaseID = 200;
                    break;
            }

            ID = BaseID + Level;
        }

        public void SetPrice()
        {
            Price = 100;
            Price += Level * 1000 - Level * 80;
            if (Quality >= QUALITY.SILVER) Price *= 7;
            if (Quality >= QUALITY.GOLD) Price *= 8;
        }

        public void SetLevel()
        {
            Level = ID - BaseID;
        }

        public int GetID()
        {
            return ID;
        }

        public void Upgrade()
        {
            ID++;
            Level++;
            SetPrice();
            SetName();
        }
    }
}
