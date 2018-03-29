using Discord_BOT.Misc.Modules.UpgradingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_BOT.Misc.Modules
{
    public abstract class Item : ICloneable
    {
        public enum QUALITY { BRONZE, SILVER, GOLD };
        public enum TYPE { COLOR = 0, CASE = 100, KEY = 200 };
        public QUALITY Quality;
        public TYPE Type;
        public int ID { get; set; }
        public int UniqueID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        static List<int> UsedIDs;

        static Item()
        {
            UsedIDs = new List<int>();
        }

        public Item()
        {
            GetNewUniqueID();
        }

        public void GetNewUniqueID()
        {
            Random randomizer = new Random();
            int random;
            do
            {
                random = randomizer.Next(0, Int32.MaxValue);
            } while (UsedIDs.Exists(x => x == random));

            UsedIDs.Add(random);
            UniqueID = random;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
