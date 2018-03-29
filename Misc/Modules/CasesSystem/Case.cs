namespace Discord_BOT.Misc.Modules.CasesSystem
{
    public abstract class Case : Item
    {
        public Prize[] Prizes { get; set; }
        public string Emoji;

        public enum Droptype
        {
            Cash,
            Color
        };

        public Droptype DropType;

        protected Case()
        {
            Type = Item.TYPE.CASE;
        }

        public abstract Prize Open(Key key);
    }
}
