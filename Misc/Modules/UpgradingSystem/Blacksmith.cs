using Discord;
using Discord.Commands;
using Discord_BOT.Core.UserAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_BOT.Misc.Modules.UpgradingSystem
{
    public class Blacksmith : ModuleBase<SocketCommandContext>
    {
        [Command("uinfo")]
        public async Task GiveInfo(int id)
        {
            var requirement = UpgradeRequirements.List.Find(x => x.ItemId == id);
            if (requirement is null)
            {
                await Context.Channel.SendMessageAsync(":warning: Przedmiot nie znajduje sie w bazie ulepszen.");
                return;
            }

            var embed = new EmbedBuilder();
            embed.WithTitle("Wymagania do ulepszenia");
            embed.WithColor(new Color(222, 252, 113));
            embed.AddField("Szansa", requirement.Chance);
            embed.AddField("Koszt", requirement.Cash);

            if (requirement.Items is null == false)
            {
                foreach (var item in requirement.Items)
                    embed.AddField($"{item.ID}", item.Name);
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("ulepsz")]
        public async Task Upgrade(int itemid)
        {
            var user = UserAccounts.GetAccount(Context.User);
            var item = user.Inventory.GetUpgradables().Find(x => x.GetID() == itemid);
            if (item is null)
            {
                await Context.Channel.SendMessageAsync(":warning: Nie posiadasz takiego przedmiotu.");
                return;
            }

            await Do(user, item);
        }

        public async Task Do(UserAccount user, IUpgradable item)
        {
            var requirement = UpgradeRequirements.List.Find(x => x.ItemId == item.GetID());
            if (requirement is null)
            {
                await Context.Channel.SendMessageAsync(":warning: Przedmiotu nie da sie ulepszyc.");
                return;
            }
            if (item.Level == item.MaxLevel)
            {
                await Context.Channel.SendMessageAsync(":warning: Item jest juz na max poziomie!");
                return;
            }

            if (TakeRequirements(user, requirement))
            {
                Random randomizer = new Random();
                int random = randomizer.Next(0, 101);
                if (requirement.Chance > random)
                {
                    item.Upgrade();
                    await Context.Channel.SendMessageAsync(":white_check_mark: Przedmiot pomyslnie ulepszony!");
                }
                else
                {
                    await Context.Channel.SendMessageAsync(":x: Zawiodles!");
                    user.Inventory.Remove(item.GetID());
                }
                UserAccounts.SaveAccounts();
            }
            else
            {
                await Context.Channel.SendMessageAsync(":warning: Nie posiadasz wszystkich wymaganych przedmiotow.");
                return;
            }
        }

        private bool TakeRequirements(UserAccount user, UpgradeRequirement requirement)
        {
            if (requirement.Items is null == false)
            {
                foreach (var item in requirement.Items)
                {
                    if (user.Inventory.Get().Exists(x => x.ID == item.ID) == false)
                        return false;
                }

                foreach (var item in requirement.Items)
                {
                    user.Inventory.Remove(item.ID);
                }
            }

            if (user.Cash >= requirement.Cash)
            {
                user.Cash -= requirement.Cash;
                return true;
            }
            else return false;
        }
    }
}
