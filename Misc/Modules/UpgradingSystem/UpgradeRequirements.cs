using System.Collections.Generic;

namespace Discord_BOT.Misc.Modules.UpgradingSystem
{
    public class UpgradeRequirement
    {
        public int ItemId { get; }
        public int Cash { get; set; }
        public List<Item> Items { get; set; }
        public int Chance { get; set; }

        public UpgradeRequirement(int itemid, int cash, int chance, List<Item> items = null)
        {
            ItemId = itemid;
            Cash = cash;
            Items = items;
            Chance = chance;
        }
    }

    public static class UpgradeRequirements
    {
        public static List<UpgradeRequirement> List;

        static UpgradeRequirements()
        {
            List = new List<UpgradeRequirement>()
            {
                new UpgradeRequirement(itemid: 200, cash: 25 , chance: 65),
                new UpgradeRequirement(itemid: 201, cash: 25 , chance: 65 ),
                new UpgradeRequirement(itemid: 202, cash: 25 , chance: 65 ),
                new UpgradeRequirement(itemid: 203, cash: 25 , chance: 65 ),
                new UpgradeRequirement(itemid: 204, cash: 25 , chance: 65 ),
                new UpgradeRequirement(itemid: 205, cash: 25 , chance: 45 ),
                new UpgradeRequirement(itemid: 206, cash: 25 , chance: 45 ),
                new UpgradeRequirement(itemid: 207, cash: 25 , chance: 45 ),
                new UpgradeRequirement(itemid: 208, cash: 25 , chance: 45 ),
                new UpgradeRequirement(itemid: 230, cash: 200, chance: 65 ),
                new UpgradeRequirement(itemid: 231, cash: 200, chance: 65 ),
                new UpgradeRequirement(itemid: 232, cash: 200, chance: 65 ),
                new UpgradeRequirement(itemid: 233, cash: 200, chance: 65 ),
                new UpgradeRequirement(itemid: 234, cash: 200, chance: 65 ),
                new UpgradeRequirement(itemid: 235, cash: 200, chance: 45 ),
                new UpgradeRequirement(itemid: 236, cash: 200, chance: 45 ),
                new UpgradeRequirement(itemid: 237, cash: 200, chance: 45 ),
                new UpgradeRequirement(itemid: 238, cash: 200, chance: 45 ),
                new UpgradeRequirement(itemid: 260, cash: 2750, chance: 65 ),
                new UpgradeRequirement(itemid: 261, cash: 2750, chance: 65 ),
                new UpgradeRequirement(itemid: 262, cash: 2750, chance: 65 ),
                new UpgradeRequirement(itemid: 263, cash: 2750, chance: 65 ),
                new UpgradeRequirement(itemid: 264, cash: 2750, chance: 65 ),
                new UpgradeRequirement(itemid: 265, cash: 2750, chance: 45 ),
                new UpgradeRequirement(itemid: 266, cash: 2750, chance: 45 ),
                new UpgradeRequirement(itemid: 267, cash: 2750, chance: 45 ),
                new UpgradeRequirement(itemid: 268, cash: 2750, chance: 45 ),
            };
        }
    }
}
