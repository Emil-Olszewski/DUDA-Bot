namespace Discord_BOT.Misc.Modules.CasesSystem.CaseTypes
{
    public class CashCase : Case
    {
        public CashCase()
        {
            DropType = Droptype.Cash;
            Emoji = ":small_orange_diamond:";
        }

        public override Prize Open(Key key)
        {
            return Prize.GetRandomPrize(Prizes, key.Level);
        }
    }
}
