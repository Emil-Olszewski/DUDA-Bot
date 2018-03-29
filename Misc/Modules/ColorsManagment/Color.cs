namespace Discord_BOT.Misc.Modules.ColorsManagment
{
    public class Color : Item
    {
        public Discord.Color RightColor { get; }

        public Color(int id, string name, Discord.Color color, QUALITY quality = QUALITY.BRONZE)
        {
            ID = id;
            Name = name;
            RightColor = color;
            Type = TYPE.COLOR;
            Quality = quality;
            Price = 0;
        }
    }
}
