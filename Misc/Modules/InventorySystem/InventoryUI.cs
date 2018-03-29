using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_BOT.Core.UserAccounts;
using Discord_BOT.Misc.Modules.ColorsManagment;

namespace Discord_BOT.Misc.Modules.InventorySystem
{
    public class InventoryUi : ModuleBase<SocketCommandContext>
    {
        [Command("eq")]
        public async Task ShowEq()
        {
            var user = UserAccounts.GetAccount(Context.User);
            var embed = new EmbedBuilder();
            embed.WithTitle($"Ekwipunek {Context.User.Username}");
            embed.WithColor(Colors.List[user.ActuallyColor].RightColor);
            var eq = user.Inventory.Get();
            var usedId = new List<int>();

            foreach (var item in eq)
            {
                if (usedId.Contains(item.ID))
                    continue;
                else
                {
                    embed.AddField($"[x{eq.FindAll(x => x.ID == item.ID).Count}] {item.ID} {item.Name}",
                        $"Wartosc: {item.Price}");
                    usedId.Add(item.ID);
                }
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("mojekolory")]
        public async Task ShowUserColors(int page = 0)
        {
            var account = UserAccounts.GetAccount(Context.User);
            var embed = new EmbedBuilder();
            embed.WithTitle($"Kolory {Context.User.Username}");
            embed.WithColor(Colors.List[account.ActuallyColor].RightColor);
            var usedId = new List<int>();
            var counter = 0;
            foreach (var c in account.Inventory.Colors)
            {
                counter++;
                if (counter <= page * 28)
                    continue;

                if (usedId.Contains(c.ID))
                    continue;

                var quality = "";
                switch (c.Quality)
                {
                    case Item.QUALITY.BRONZE:
                        quality = "BRAZOWA";
                        break;
                    case Item.QUALITY.SILVER:
                        quality = "SREBRNA";
                        break;
                    case Item.QUALITY.GOLD:
                        quality = "ZLOTA";
                        break;
                }

                embed.AddField(":shirt: " + c.Name, $"[x{account.Inventory.Colors.FindAll(x => x.ID == c.ID).Count}] " +
                $"{c.ID} " + $"Jakosc {quality}");
                usedId.Add(c.ID);
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("comam")]
        public async Task DescribeWear()
        {
            var user = UserAccounts.GetAccount(Context.User);
            await Context.Channel.SendMessageAsync(
                $"{Context.User.Mention}, :shirt: **{user.ActuallyColor}" +
                $" {Colors.List.Find(x => x.ID == user.ActuallyColor).Name}**");
        }
    }
}
