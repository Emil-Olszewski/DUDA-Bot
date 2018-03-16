using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_BOT.Core.UserAccounts;

namespace Discord_BOT
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        public async Task Echo()
        {
            await Context.Channel.SendMessageAsync("Hello world!");
        }

        [Command("komendy")]
        public async Task Commands()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Dostepne komendy DudoBota");
            embed.WithColor(new Color(0, 255, 255));
            embed.WithDescription("[created by watashi wa]");
            embed.AddField("sekret", "Scisle strzezona tajemnica panstwowa.");
            embed.AddField("500+", "Darmowe pinionzki, **500** dudow co 24h!");
            embed.AddField("kierowniku", "Dla dobrego obywatela, **10 dudow** co 10 minut!");
            embed.AddField("opisz @user", "Dowiedz sie ile masz dudow, i jaki posiadasz status spoleczny.");
            embed.AddField("przelew @user", "Oddaj pinionzki biednym i zyskaj troche statusu spolecznego");
            embed.AddField("ruletka <kwota>", "Pobaw sie w hazard, dokladne reguly w komendzie **ruletka-opis**.");
            embed.AddField("ilebrakuje", "Dowiedz sie ile punktow spolecznych brakuje ci do kolejnego poziomu.");
            embed.AddField("ilena <poziom>", "Dowiedz sie ile punktow spolecznych potrzeba na okreslony poziom.");

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command ("ruletka-opis")]
        public async Task RouletteDescription()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Ruletka");
            embed.WithColor(new Color(0, 255, 255));
            embed.WithDescription("Szansa na wygrana zwieksza sie wraz ze stawiana kwota.");
            embed.AddField("Od 0% do 50% posiadanych dudow", "masz stale 50% szans na zwyciestwo");
            embed.AddField("za kazde 2% posiadanych dudow powyzej 50%", "dostajesz dodatkowy 1% szans na zwyciestwo");

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("sekret")]
        public async Task RevealSecret([Remainder]string arg = "")
        {
            if (!IsUserRankOwner((SocketGuildUser)Context.User, "Sekretna Ranga"))
            {
                await Context.Channel.SendMessageAsync("Jeszcze nie dorosłeś do tej wiedzy, młokosie.");
                return;
            }

            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("SECRET"));
        }
        
        [Command("opisz")]
        public async Task DescribeUser([Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var a = UserAccounts.GetAccount(target);

            var embed = new EmbedBuilder();
            embed.WithTitle($"{target.Username}");
            embed.WithColor(new Color(0, 255, 255));
            embed.AddInlineField("Poziom spoleczny", a.Level);
            embed.AddInlineField("Punkty statusu spolecznego", a.XP);
            embed.AddField("Dudy", a.Cash);
            embed.AddInlineField("Skrzynek", a.NumberOfCases);
            embed.AddInlineField("Kluczy", a.NumberOfKeys);

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("500+")]
        public async Task Give500Cash()
        {
            var account = UserAccounts.GetAccount(Context.User);
            TimeSpan requiredTime = new TimeSpan(0, Int32.Parse(Config.bot.time500), 0);

            if (WasRequiredTimeElapsed(account.last500Use, requiredTime))
            {
                account.Cash += 500;
                await Context.Channel.SendMessageAsync($"Gratuluje dziecka! Dostales **500** dudow.");

                account.last500Use = DateTime.Now;
                UserAccounts.SaveAccounts();
            }
            else
            {
                TimeSpan waitingTime = account.last500Use + requiredTime - DateTime.Now;
                await Context.Channel.SendMessageAsync($"Hola hola, musisz poczekac jeszcze " +
                    $"`{waitingTime.Hours}h:{waitingTime.Minutes}m` " +
                    $"zanim bedziesz mogl pobrac kolejne swiadczenie!.");
            }
        }

        [Command("kierowniku")]
        public async Task Give10Cash()
        {
            var account = UserAccounts.GetAccount(Context.User);

            TimeSpan requiredTime = new TimeSpan(0, Int32.Parse(Config.bot.time10), 0);
            if (WasRequiredTimeElapsed(account.last10Use, requiredTime))
            {
                account.Cash += 10;
                await Context.Channel.SendMessageAsync($"**10** dudow dla ksieciunia!");

                account.last10Use = DateTime.Now;
                UserAccounts.SaveAccounts();
            }
            else
            {
                TimeSpan waitingTime = account.last10Use + requiredTime - DateTime.Now;
                await Context.Channel.SendMessageAsync($"Ejze, Menelaosie. Poczekaj ze " +
                    $"`{waitingTime.Minutes}m:{waitingTime.Seconds}s` na kolejny datek!");
            }
        }

        [Command("przelew")]
        public async Task GiveCashOtherUser(int howMuch, [Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var a1 = UserAccounts.GetAccount(Context.User);
            var a2 = UserAccounts.GetAccount(target);

            TimeSpan requiredTime = new TimeSpan(0, Int32.Parse(Config.bot.timetransfer), 0);

            if(!WasRequiredTimeElapsed(a1.lastTransfer, requiredTime))
            {
                TimeSpan waitingTime = a1.lastTransfer + requiredTime - DateTime.Now;
                await Context.Channel.SendMessageAsync($"Kolejny przelew dostepny za " +
                    $"`{waitingTime.Minutes}m:{waitingTime.Seconds}s`.");
                return;
            }

            if (howMuch <= 0)
            {
                await Context.Channel.SendMessageAsync($"Nie bardzo.");
                return;
            }

            if (a1.Cash >= howMuch)
            {
                double tax = howMuch * 0.15;
                UserAccounts.AddCash(a1, -howMuch);
                UserAccounts.AddCash(a2, howMuch - (int)tax);
                UserAccounts.AddCash(UserAccounts.GetAccount(421676491345100800), (int)tax);
                UserAccounts.AddXP(a1, howMuch / 10);
                UserAccounts.AddXP(UserAccounts.GetAccount(421676491345100800), 1);

                await Context.Channel.SendMessageAsync($"Przelew udany. Dostales **{howMuch / 10}** punktow statusu spolecznego. " +
                    $"Prowizja: **{tax}** dudow.");

                a1.lastTransfer = DateTime.Now;
                UserAccounts.SaveAccounts();
            }
            else
                await Context.Channel.SendMessageAsync($"Tfu, biedaku robaku. Nie masz tyle hajsu.");
        }

        [Command("ilebrakuje")]
        public async Task HowMuch()
        {
            var account = UserAccounts.GetAccount(Context.User);
            int howMuch = (int)(2 * Math.Pow(account.Level + 1, 3)) + 50 - account.XP;
            await Context.Channel.SendMessageAsync($"Do kolejnego poziomu **{account.Level+1}** brakuje ci **{howMuch}** expa");
        }

        [Command("ilena")]
        public async Task HowMuch(int level)
        {
            int howMuch = (int)(2 * Math.Pow(level, 3)) + 50;
            await Context.Channel.SendMessageAsync($"Na poziom **{level}** jest wymagane **{howMuch}** punktow spolecznych.");
        }

        public static bool IsUserRankOwner(SocketGuildUser user, string targetRoleName)
        {
            var result = from r in user.Guild.Roles where r.Name == targetRoleName select r.Id;
            ulong roleID = result.FirstOrDefault();
            if (roleID == 0) return false;
            var targetRole = user.Guild.GetRole(roleID);
            return user.Roles.Contains(targetRole);
        }

        public static bool WasRequiredTimeElapsed(DateTime date, TimeSpan time)
        {
            DateTime requiredDate = date.Add(time);
            if (DateTime.Now >= requiredDate) return true;
            return false;
        }
    }
}
