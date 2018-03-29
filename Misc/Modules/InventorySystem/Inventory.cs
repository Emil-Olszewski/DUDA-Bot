using System.Collections.Generic;
using Discord_BOT.Misc.Modules.CasesSystem;
using Discord_BOT.Misc.Modules.CasesSystem.CaseTypes;
using Discord_BOT.Misc.Modules.ColorsManagment;
using Discord_BOT.Misc.Modules.UpgradingSystem;

namespace Discord_BOT.Misc.Modules.InventorySystem
{
    public class Inventory
    {
        public List<Color> Colors;
        public List<ColorCase> ColorCases;
        public List<CashCase> CashCases;
        public List<Key> Keys;

        public Inventory()
        {
            Colors = new List<Color>();
            ColorCases = new List<ColorCase>();
            CashCases = new List<CashCase>();
            Keys = new List<Key>();
        }

        public void Add(Item item)
        {
            switch (item.Type)
            {
                case Item.TYPE.COLOR:
                    var color = (Color)item.Clone();
                    color.GetNewUniqueID();
                    Colors.Add(color);
                    break;

                case Item.TYPE.CASE:
                    var @case = (Case)item.Clone();
                    @case.GetNewUniqueID();
                    if (@case.DropType == Case.Droptype.Color)
                    {
                        var colorCase = (ColorCase)@case;
                        ColorCases.Add(colorCase);
                    }
                    else
                    {
                        var cashCase = (CashCase)@case;
                        CashCases.Add(cashCase);
                    }

                    break;

                case Item.TYPE.KEY:
                    var key = (Key)item.Clone();
                    key.GetNewUniqueID();
                    Keys.Add(key);
                    break;
            }
        }

        public List<Item> Get()
        {
            var list = new List<Item>();
            foreach (var c in ColorCases)
                list.Add(c);

            foreach (var c in CashCases)
                list.Add(c);

            foreach (var k in Keys)
                list.Add(k);

            return list;
        }

        public List<IUpgradable> GetUpgradables()
        {
            var list = new List<IUpgradable>();
            foreach (var k in Keys)
                list.Add(k);

            return list;
        }

        public bool Remove(int id)
        {
            if (Colors.Exists(x => x.ID == id))
            {
                Colors.Remove(Colors.Find(x => x.ID == id));
                return true;
            }

            if (ColorCases.Exists(x => x.ID == id))
            {
                ColorCases.Remove(ColorCases.Find(x => x.ID == id));
                return true;
            }

            if (CashCases.Exists(x => x.ID == id))
            {
                CashCases.Remove(CashCases.Find(x => x.ID == id));
                return true;
            }

            if (!Keys.Exists(x => x.ID == id)) return false;
            {
                Keys.Remove(Keys.Find(x => x.ID == id));
                return true;
            }
        }
    }
}

