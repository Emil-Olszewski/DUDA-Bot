using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord_BOT.Core.UserAccounts;
using Discord_BOT.Misc.Modules.CasesSystem;

namespace Discord_BOT.Misc.Modules.Shop
{
    public class Shop : ModuleBase<SocketCommandContext>
    {
        public static List<Item> Assortment = new List<Item>();

        static Shop()
        {
            foreach (var c in CasesSystem.Cases.List)
                Assortment.Add(c);

            Assortment.Add(new Key(Item.QUALITY.BRONZE, 0));
            Assortment.Add(new Key(Item.QUALITY.SILVER, 0));
            Assortment.Add(new Key(Item.QUALITY.GOLD, 0));
        }

        [Command("sklep")]
        public async Task ShowShopInfo()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Siec sklepow Dudodronka");
            embed.WithColor(new Color(162, 229, 80));

            embed.AddField("s-skrzynki", ":lock: Takich skrzynek nie kupisz nigdzie indziej!");
            embed.AddField("s-klucze", ":key: Najlepszy sklep z kluczami w miescie.");

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("s-skrzynki")]
        public async Task ShowCasesForSale()
        {
            await ShowItems(Item.TYPE.CASE, ":lock: Skrzynie dla malych i duzych", 61, 78, 107);
        }
        [Command("s-klucze")]

        public async Task ShowKeysForSale()
        {
            await ShowItems(Item.TYPE.KEY, ":key: Kluczoshop", 226, 68, 56);
        }

        public async Task ShowItems(Item.TYPE type, string shopName, int r, int g, int b)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(shopName);
            embed.WithColor(new Color(r, g, b));

            foreach (var item in Assortment)
            {
                if (item.Type == type)
                {
                    embed.AddField($"{item.ID} {item.Name}", $"Cena: {item.Price}");
                }
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("kup")]
        public async Task BuyItem(int id, int amount = 1)
        {
            var item = Assortment.Find(x => x.ID == id);
            if (item is null)
            {
                await Context.Channel.SendMessageAsync(":warning: Nie ma takiego przedmiotu w ofercie.");
                return;
            }

            var user = UserAccounts.GetAccount(Context.User);
            if (item.Price * amount > user.Cash)
            {
                await Context.Channel.SendMessageAsync(":warning: Nie stac cie biedaku-robaku.");
                return;
            }

            user.Cash -= item.Price * amount;
            for (int i = 0; i < amount; i++)
                user.Inventory.Add(Assortment.Find(x => x.ID == id));
            UserAccounts.SaveAccounts();

            await Context.Channel.SendMessageAsync($"{Context.User.Mention} dokonales zakupu **{item.Name} " +
                $"x{amount}**  za :small_orange_diamond:**{item.Price * amount} Dudow.**");
        }
    }
}
