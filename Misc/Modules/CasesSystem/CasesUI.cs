using System.Threading.Tasks;
using Discord.Commands;
using Discord_BOT.Core.UserAccounts;

namespace Discord_BOT.Misc.Modules.CasesSystem
{
    public class CasesUi : ModuleBase<SocketCommandContext>
    {
        [Command("os")]
        public async Task OpenCaseShort(int caseId, int keyId = 0)
        {
            await OpenCase(caseId, keyId);
        }

        [Command("otworzskrzynie")]
        public async Task OpenCase(int caseId, int keyId = 0)
        {
            var account = UserAccounts.GetAccount(Context.User);
            var item = account.Inventory.Get().Find(x => x.ID == caseId);

            if (item is null || item.Type != Item.TYPE.CASE)
            {
                await Context.Channel.SendMessageAsync(
                    ":warning: Blad! Nie posiadasz takiej skrzynki, albo ID jest nieprawdziwe.");
                return;
            }

            var @case = (Case)item;

            Key key;
            if (keyId != 0)
            {
                key = account.Inventory.Keys.Find(x => x.ID == keyId);
                if (key is null)
                {
                    await Context.Channel.SendMessageAsync(
                        ":warning: Blad! Nie posiadasz takiego klucza, albo jego ID jest nieprawdziwe.");
                    return;
                }
            }
            else
                key = account.Inventory.Keys.Find(x => x.Quality == @case.Quality);

            if (key is null)
            {
                await Context.Channel.SendMessageAsync(":warning: Nie posiadasz klucza odpowiedniej jakosci.");
                return;
            }

            var prize = @case.Open(key);
            if (prize is null)
            {
                await Context.Channel.SendMessageAsync(":warning: Klucz nie pasuje do skrzynki!");
                return;
            }

            account.Inventory.Remove(@case.ID);
            account.Inventory.Remove((key.ID));

            account.CasesProfit += (uint)prize.GiveAndReturnProfit(account);
            account.OpenedCases++;
            UserAccounts.SaveAccounts();

            await Context.Channel.SendMessageAsync($"{Context.User.Mention} wygrales {@case.Emoji} **{prize.Name}**!");
        }
    }
}
