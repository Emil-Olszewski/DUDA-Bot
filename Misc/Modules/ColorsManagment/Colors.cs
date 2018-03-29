using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_BOT.Core.UserAccounts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discord_BOT.Misc.Modules.ColorsManagment
{
    public class Colors : ModuleBase<SocketCommandContext>
    {
        public static List<Color> List = new List<Color>
        {
            new Color(0, "White", new Discord.Color(255, 255, 255)),
            new Color(1, "IndianRed", new Discord.Color(205, 92, 92), Item.QUALITY.SILVER),
            new Color(2, "LightCoral", new Discord.Color(240, 128, 128)),
            new Color(3, "Salmon", new Discord.Color(250, 128, 114)),
            new Color(4, "DarkSalmon", new Discord.Color(233, 150, 122)),
            new Color(5, "LightSalmon", new Discord.Color(255, 160, 122), Item.QUALITY.GOLD),
            new Color(6, "Crimson", new Discord.Color(220, 20, 60), Item.QUALITY.SILVER),
            new Color(7, "Red", new Discord.Color(255, 0, 0), Item.QUALITY.SILVER),
            new Color(8, "FireBrick", new Discord.Color(178, 34, 34), Item.QUALITY.SILVER),
            new Color(9, "DarkRed", new Discord.Color(139, 0, 0), Item.QUALITY.GOLD),
            new Color(10, "Pink", new Discord.Color(255, 192, 203)),
            new Color(11, "LightPink", new Discord.Color(255, 182, 193)),
            new Color(12, "HotPink", new Discord.Color(255, 105, 180)),
            new Color(13, "DeepPink", new Discord.Color(255, 20, 147)),
            new Color(14, "MediumVioletRed", new Discord.Color(199, 21, 133)),
            new Color(15, "PaleVioletRed", new Discord.Color(219, 112, 147), Item.QUALITY.GOLD),
            new Color(16, "InvisibleUserList", new Discord.Color(47, 59, 54), Item.QUALITY.GOLD),
            new Color(17, "Coral", new Discord.Color(255, 127, 80), Item.QUALITY.SILVER),
            new Color(18, "Tomato", new Discord.Color(255, 99, 71), Item.QUALITY.SILVER),
            new Color(19, "OrangeRed", new Discord.Color(255, 69, 0), Item.QUALITY.SILVER),
            new Color(20, "DarkOrange", new Discord.Color(255, 140, 0), Item.QUALITY.SILVER),
            new Color(21, "Orange", new Discord.Color(255, 165, 0)),
            new Color(22, "Gold", new Discord.Color(255, 215, 0), Item.QUALITY.GOLD),
            new Color(23, "Yellow", new Discord.Color(255, 255, 0), Item.QUALITY.SILVER),
            new Color(24, "LightYellow", new Discord.Color(255, 255, 224)),
            new Color(25, "LemonChiffon", new Discord.Color(255, 250, 205)),
            new Color(26, "LightGoldenrodYellow", new Discord.Color(250, 250, 210)),
            new Color(27, "PapayaWhip", new Discord.Color(255, 239, 213), Item.QUALITY.SILVER),
            new Color(28, "Moccasin", new Discord.Color(255, 228, 181), Item.QUALITY.SILVER),
            new Color(29, "PeachPuff", new Discord.Color(255, 218, 185), Item.QUALITY.SILVER),
            new Color(30, "PaleGoldenrod", new Discord.Color(238, 232, 170)),
            new Color(31, "Khaki", new Discord.Color(240, 230, 140)),
            new Color(32, "DarkKhaki", new Discord.Color(189, 183, 107)),
            new Color(33, "Lavender", new Discord.Color(230, 230, 250)),
            new Color(34, "Thistle", new Discord.Color(216, 191, 216)),
            new Color(35, "Plum", new Discord.Color(221, 160, 221)),
            new Color(36, "Violet", new Discord.Color(238, 130, 238)),
            new Color(37, "Orchid", new Discord.Color(218, 112, 214)),
            new Color(38, "Fuchsia", new Discord.Color(255, 0, 255)),
            new Color(39, "Magenta", new Discord.Color(255, 0, 255)),
            new Color(40, "MediumOrchid", new Discord.Color(186, 85, 211)),
            new Color(41, "MediumPurple", new Discord.Color(147, 112, 219), Item.QUALITY.GOLD),
            new Color(42, "BlueViolet", new Discord.Color(138, 43, 226)),
            new Color(43, "DarkViolet", new Discord.Color(148, 0, 211)),
            new Color(44, "DarkOrchid", new Discord.Color(153, 50, 204)),
            new Color(45, "DarkMagenta", new Discord.Color(139, 0, 139)),
            new Color(46, "Purple", new Discord.Color(128, 0, 128)),
            new Color(47, "Indigo", new Discord.Color(75, 0, 130)),
            new Color(48, "SlateBlue", new Discord.Color(106, 90, 205)),
            new Color(49, "DarkSlateBlue", new Discord.Color(72, 61, 139)),
            new Color(50, "MediumSlateBlue", new Discord.Color(123, 104, 238)),
            new Color(51, "GreenYellow", new Discord.Color(173, 255, 47)),
            new Color(52, "Chartreuse", new Discord.Color(127, 255, 0)),
            new Color(53, "LawnGreen", new Discord.Color(124, 252, 0)),
            new Color(54, "Lime", new Discord.Color(0, 255, 0)),
            new Color(55, "LimeGreen", new Discord.Color(50, 205, 50)),
            new Color(56, "PaleGreen", new Discord.Color(152, 251, 152), Item.QUALITY.SILVER),
            new Color(57, "LightGreen", new Discord.Color(144, 238, 144)),
            new Color(58, "MediumSpringGreen", new Discord.Color(0, 250, 154)),
            new Color(59, "SpringGreen", new Discord.Color(0, 255, 127)),
            new Color(60, "MediumSeaGreen", new Discord.Color(60, 179, 113), Item.QUALITY.SILVER),
            new Color(61, "SeaGreen", new Discord.Color(46, 139, 87)),
            new Color(62, "ForestGreen", new Discord.Color(34, 139, 34), Item.QUALITY.SILVER),
            new Color(63, "Green", new Discord.Color(0, 128, 0), Item.QUALITY.SILVER),
            new Color(64, "DarkGreen", new Discord.Color(0, 100, 0), Item.QUALITY.GOLD),
            new Color(65, "YellowGreen", new Discord.Color(154, 205, 50)),
            new Color(66, "OliveDrab", new Discord.Color(107, 142, 35), Item.QUALITY.SILVER),
            new Color(67, "Olive", new Discord.Color(128, 128, 0), Item.QUALITY.SILVER),
            new Color(68, "DarkOliveGreen", new Discord.Color(85, 107, 47)),
            new Color(69, "MediumAquamarine", new Discord.Color(102, 205, 170)),
            new Color(70, "DarkSeaGreen", new Discord.Color(143, 188, 143)),
            new Color(71, "LightSeaGreen", new Discord.Color(32, 178, 170), Item.QUALITY.SILVER),
            new Color(72, "DarkCyan", new Discord.Color(0, 139, 139)),
            new Color(73, "Teal", new Discord.Color(0, 128, 128), Item.QUALITY.SILVER),
            new Color(74, "Cyan", new Discord.Color(0, 255, 255)),
            new Color(75, "LightCyan", new Discord.Color(224, 255, 255)),
            new Color(76, "PaleTurquoise", new Discord.Color(175, 238, 238)),
            new Color(77, "Aquamarine", new Discord.Color(127, 255, 212)),
            new Color(78, "Turquoise", new Discord.Color(64, 224, 208)),
            new Color(79, "MediumTurquoise", new Discord.Color(72, 209, 204)),
            new Color(80, "DarkTurquoise", new Discord.Color(0, 206, 209)),
            new Color(81, "CadetBlue", new Discord.Color(95, 158, 160)),
            new Color(82, "SteelBlue", new Discord.Color(70, 130, 180), Item.QUALITY.GOLD),
            new Color(83, "LightSteelBlue", new Discord.Color(176, 196, 222)),
            new Color(84, "PowderBlue", new Discord.Color(176, 224, 230)),
            new Color(85, "LightBlue", new Discord.Color(173, 216, 230), Item.QUALITY.SILVER),
            new Color(86, "SkyBlue", new Discord.Color(135, 206, 235)),
            new Color(87, "LightSkyBlue", new Discord.Color(135, 206, 250), Item.QUALITY.GOLD),
            new Color(88, "DeepSkyBlue", new Discord.Color(0, 191, 255)),
            new Color(89, "DodgerBlue", new Discord.Color(30, 144, 255)),
            new Color(90, "CornflowerBlue", new Discord.Color(100, 149, 237)),
            new Color(91, "MediumSlateBlue", new Discord.Color(123, 104, 238)),
            new Color(92, "RoyalBlue", new Discord.Color(65, 105, 225), Item.QUALITY.SILVER),
            new Color(93, "Blue", new Discord.Color(0, 0, 255)),
            new Color(94, "MediumBlue", new Discord.Color(0, 0, 205)),
            new Color(95, "DarkBlue", new Discord.Color(0, 0, 139)),
            new Color(96, "Navy", new Discord.Color(0, 0, 128)),
            new Color(97, "MidnightBlue", new Discord.Color(25, 25, 112)),
            new Color(98, "Cornsilk", new Discord.Color(255, 248, 220)),
            new Color(99, "BlanchedAlmond", new Discord.Color(255, 235, 205)),
            new Color(100, "Bisque", new Discord.Color(255, 228, 196)),
            new Color(101, "NavajoWhite", new Discord.Color(255, 222, 173)),
            new Color(102, "Wheat", new Discord.Color(245, 222, 179)),
            new Color(103, "BurlyWood", new Discord.Color(222, 184, 135)),
            new Color(104, "Tan", new Discord.Color(210, 180, 140)),
            new Color(105, "RosyBrown", new Discord.Color(188, 143, 143), Item.QUALITY.GOLD),
            new Color(106, "SandyBrown", new Discord.Color(244, 164, 96)),
            new Color(107, "Goldenrod", new Discord.Color(218, 165, 32)),
            new Color(108, "DarkGoldenrod", new Discord.Color(184, 134, 11)),
            new Color(109, "Peru", new Discord.Color(205, 133, 63), Item.QUALITY.SILVER),
            new Color(110, "Chocolate", new Discord.Color(210, 105, 30), Item.QUALITY.SILVER),
            new Color(111, "SaddleBrown", new Discord.Color(139, 69, 19)),
            new Color(112, "Sienna", new Discord.Color(160, 82, 45)),
            new Color(113, "Brown", new Discord.Color(165, 42, 42)),
            new Color(114, "Maroon", new Discord.Color(128, 0, 0), Item.QUALITY.SILVER),
            new Color(115, "Snow", new Discord.Color(255, 250, 250)),
            new Color(116, "Honeydew", new Discord.Color(240, 255, 240)),
            new Color(117, "MintCream", new Discord.Color(245, 255, 250)),
            new Color(118, "Azure", new Discord.Color(240, 255, 255)),
            new Color(119, "AliceBlue", new Discord.Color(240, 248, 255)),
            new Color(120, "GhostWhite", new Discord.Color(248, 248, 255)),
            new Color(121, "WhiteSmoke", new Discord.Color(245, 245, 245)),
            new Color(122, "Seashell", new Discord.Color(255, 245, 238)),
            new Color(123, "Beige", new Discord.Color(245, 245, 220)),
            new Color(124, "OldLace", new Discord.Color(253, 245, 230)),
            new Color(125, "FloralWhite", new Discord.Color(255, 250, 240)),
            new Color(126, "Ivory", new Discord.Color(255, 255, 240)),
            new Color(127, "AntiqueWhite", new Discord.Color(250, 235, 215)),
            new Color(128, "Linen", new Discord.Color(250, 240, 230)),
            new Color(129, "LavenderBlush", new Discord.Color(255, 240, 245)),
            new Color(130, "MistyRose", new Discord.Color(255, 228, 225)),
            new Color(131, "Gainsboro", new Discord.Color(220, 220, 220)),
            new Color(132, "LightGrey", new Discord.Color(211, 211, 211)),
            new Color(133, "Silver", new Discord.Color(192, 192, 192)),
            new Color(134, "DarkGray", new Discord.Color(169, 169, 169)),
            new Color(135, "Gray", new Discord.Color(128, 128, 128)),
            new Color(136, "DimGray", new Discord.Color(105, 105, 105)),
            new Color(137, "LightSlateGray", new Discord.Color(119, 136, 153)),
            new Color(138, "SlateGray", new Discord.Color(112, 128, 144)),
            new Color(139, "DarkSlateGray", new Discord.Color(47, 79, 79), Item.QUALITY.SILVER),
            new Color(140, "InvisibleChat", new Discord.Color(54, 57, 62), Item.QUALITY.GOLD),
        };

        [Command("c-creator")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CreateRolesIfDoesntExist()
        {
            var user = (SocketGuildUser)Context.User;
            foreach (var color in List)
            {
                var role = user.Guild.Roles.FirstOrDefault(x => x.Name == color.Name);
                if (role is null) await user.Guild.CreateRoleAsync(color.Name, color: color.RightColor);
            }
        }

        [Command("updatewear")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task UpdateWearOfAllUsers()
        {
            var user = (SocketGuildUser)Context.User;
            foreach (var u in user.Guild.Users) await UpdateWearOf(u);
        }

        private static async Task UpdateWearOf(SocketGuildUser user)
        {
            var account = UserAccounts.GetAccount(user);
            var needToAddRole = true;
            foreach (var c in account.Inventory.Colors)
            {
                if (!Checker.IsUserRankOwner(user, c.Name)) continue;
                if (c.ID == List[account.ActuallyColor].ID)
                {
                    needToAddRole = false;
                    continue;
                }

                var roleToDelete = user.Guild.Roles.FirstOrDefault(x => x.Name == c.Name);
                await user.RemoveRoleAsync(roleToDelete);
            }

            if (needToAddRole)
            {
                var role = user.Guild.Roles.FirstOrDefault(x => x.Name == List[account.ActuallyColor].Name);
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
                await Context.Channel.SendMessageAsync(":warning: Juz masz to zalozone!");
                return;
            }

            if (List.Exists(x => x.ID == id) == false)
            {
                await Context.Channel.SendMessageAsync(":warning: Nieprawidlowe ID!");
                return;
            }

            if (account.Inventory.Colors.Exists(x => x.ID == id))
            {
                account.ActuallyColor = id;
                await UpdateWearOf((SocketGuildUser)Context.User);
            }
            else
                await Context.Channel.SendMessageAsync(":warning: Nie posiadasz tego przedmiotu!");
        }

        [Command("pokaz")]
        public async Task ShowColor(int id)
        {
            if (List.Exists(x => x.ID == id) == false)
            {
                await Context.Channel.SendMessageAsync(":warning: Nie ma takiego koloru.");
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(List[id].RightColor);
                embed.WithTitle($"Probka koloru {List[id].Name} za **{List[id].Price}**");
                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }
    }
}
