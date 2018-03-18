using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_BOT.Core.UserAccounts;
using Miscelenaous = Discord_BOT.Misc.Misc;

namespace Discord_BOT.Modules.ColorsManagment
{
    public class Colors : ModuleBase<SocketCommandContext>
    {
        public static Color[] AvalibleColors =
        {
            new Color(0, "White", new Discord.Color(255, 255, 255), price: 0),
            new Color(1, "Moccasin", new Discord.Color(255, 228, 181), price: 1000),
            new Color(2, "DarkKhaki", new Discord.Color(189, 183, 107), price: 3000),
            new Color(3, "Pink", new Discord.Color(255, 192, 203), price: 3000),
            new Color(4, "Khaki", new Discord.Color(240, 230, 140), price: 5000),
            new Color(5, "DarkGreen", new Discord.Color(0, 100, 0), price: 7500),
            new Color(6, "BlueViolet", new Discord.Color(138, 43, 226), price: 9000),
            new Color(7, "DarkMagenta", new Discord.Color(139, 0, 139), price: 9000),
            new Color(8, "PaleGreen", new Discord.Color(152, 251, 152), price: 12000),
            new Color(9, "LawnGreen", new Discord.Color(124, 252, 0), price: 12000),
            new Color(10, "GreenYellow", new Discord.Color(173, 255, 47), price: 12000),
            new Color(11, "Cyan", new Discord.Color(0, 255, 255), price: 18000),
            new Color(12, "Teal", new Discord.Color(0, 128, 128), price: 18000),
            new Color(13, "MediumSeaGreen", new Discord.Color(60, 179, 113), price: 25000),
            new Color(14, "LightSeaGreen", new Discord.Color(32, 178, 170), price: 25000),
            new Color(15, "Plum", new Discord.Color(221, 160, 221), price: 50000),
            new Color(16, "HotPink", new Discord.Color(255, 105, 180), price: 50000),
            new Color(17, "PaleVioletRed", new Discord.Color(219, 112, 147), price: 60000),
            new Color(18, "DeepPink", new Discord.Color(255, 20, 147), price: 60000),
            new Color(19, "LightSalmon", new Discord.Color(255, 160, 122), price: 100000),
            new Color(20, "Salmon", new Discord.Color(250, 128, 114), price: 120000),
            new Color(21, "IndianRed", new Discord.Color(255, 92, 92), price: 150000),
            new Color(22, "Tomato", new Discord.Color(255, 99, 71), price: 200000),
            new Color(23, "DarkOrange", new Discord.Color(255, 140, 0), price: 250000),
            new Color(24, "OrangeRed", new Discord.Color(255, 69, 0), price: 300000),
            new Color(25, "RoyalBlue", new Discord.Color(65, 105, 225), price: 700000),
            new Color(26, "SteelBlue", new Discord.Color(70, 130, 180), price: 900000),
            new Color(27, "Invisible", new Discord.Color(47, 49, 54), price: 1500000),
            new Color(28, "Red", new Discord.Color(255, 0, 0), price: 2500000),
            new Color(29, "Crimson", new Discord.Color(155, 38, 38), price: 10000000),
            new Color(30, "Gold", new Discord.Color(255, 215, 0), price: 100000000),
};

        [Command("c-creator")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CreateRolesIfDoesntExist()
        {
            SocketGuildUser user = (SocketGuildUser)Context.User;
            foreach (var color in AvalibleColors)
            {
                var result = from r in user.Guild.Roles where r.Name == color.Name select r.Id;
                var ID = result.FirstOrDefault();
                if (ID == 0)
                    await user.Guild.CreateRoleAsync(color.Name, color: color.RightColor);
            }
        }

        [Command("updatewear")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task UpdateWearOfAllUsers()
        {
            SocketGuildUser user = (SocketGuildUser)Context.User;
            foreach (var u in user.Guild.Users)
            {
                await UpdateWearOf(u);
            }
        }

        private async Task UpdateWearOf(SocketGuildUser user)
        {
            var account = UserAccounts.GetAccount(user);
            var color = account.Equipment.Find(x => x.ID == account.ActuallyColor);
            bool needToAddRole = true;

            foreach (var e in account.Equipment)
            {
                if (Miscelenaous.IsUserRankOwner(user, e.Name))
                {
                    if(e.ID == AvalibleColors[account.ActuallyColor].ID)
                    {
                        needToAddRole = false;
                        continue;
                    }

                    var roleToDelete = user.Guild.Roles.FirstOrDefault(r => r.Name == e.Name);
                    await user.RemoveRoleAsync(roleToDelete);
                }
            }

            if(needToAddRole)
            {
                var role = user.Guild.Roles.FirstOrDefault(r => r.Name == AvalibleColors[account.ActuallyColor].Name);
                await user.AddRoleAsync(role);
            }
            
            UserAccounts.SaveAccounts();
        }

        [Command("ubierz")]
        public async Task Wear(int id)
        {
            var account = UserAccounts.GetAccount(Context.User);
            if (account.ActuallyColor == id)
            {
                await Context.Channel.SendMessageAsync("Juz masz to zalozone!");
                return;
            }

            if (account.Equipment.Exists(x => x.ID == id))
            {
                account.ActuallyColor = (uint)id;
                await UpdateWearOf((SocketGuildUser)Context.User);
            }
            else
                await Context.Channel.SendMessageAsync("Nie posiadasz tego przedmiotu!");
        }

        [Command("pokazeq")]
        public async Task ShowEQ()
        {
            var account = UserAccounts.GetAccount(Context.User);
            var embed = new EmbedBuilder();
            embed.WithTitle($"Ekwipunek {Context.User.Username}");
            embed.WithColor(new Discord.Color(0, 255, 255));

            foreach (var e in account.Equipment)
            {
                embed.AddField(e.ID + " " + e.Name, $"Wartosc: {e.Price}");
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("kup")]
        public async Task BuyColor(int id)
        {
            var account = UserAccounts.GetAccount(Context.User);
            if (account.Equipment.Exists(x => x.ID == id))
            {
                await Context.Channel.SendMessageAsync("Juz posiadasz ten przedmiot!");
                return;
            }

            if (id < 0 || id >= AvalibleColors.Count())
            {
                await Context.Channel.SendMessageAsync("Taki kolor nie istnieje!");
                return;
            }

            if (account.Cash >= AvalibleColors[id].Price)
            {
                account.Cash -= (int)AvalibleColors[id].Price;
                account.Equipment.Add(AvalibleColors[id]);
                await Context.Channel.SendMessageAsync("Dokonano zakupu!");
                UserAccounts.SaveAccounts();
            }
            else
            {
                await Context.Channel.SendMessageAsync("Jestes za biedny robaku!");
                return;
            }
        }

        [Command("sklep")]
        public async Task ShowShop(int page = 0)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Sklep z KOLORAMI!-1");
            embed.WithColor(new Discord.Color(0, 255, 255));

            foreach (var e in AvalibleColors)
            {
                if (page == 0 && e.ID >= 15) continue;
                if (page == 1 && e.ID < 15) continue;
                embed.AddField(e.ID + " " + e.Name, $"Cena {e.Price}");
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("pokaz")]
        public async Task ShowColor(int id)
        {
            if(id < 0 || id >= AvalibleColors.Count())
            {
                await Context.Channel.SendMessageAsync("Nie ma takiego koloru.");
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(AvalibleColors[id].RightColor);
                embed.WithTitle($"Probka koloru {AvalibleColors[id].Name}");
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            
        }
    }
}
