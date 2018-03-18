using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_BOT.Modules.ColorsManagment
{
    public class Color
    {
        public uint ID { get; }
        public string Name { get; }
        public Discord.Color RightColor { get; }
        public uint Price { get; }

        public Color(uint id, string name, Discord.Color color, uint price)
        {
            ID = id;
            Name = name;
            RightColor = color;
            Price = price;
        }
    }
}
