using Discord.WebSocket;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord_BOT.Misc.Modules.ColorsManagment;
using Discord_BOT.Misc.Modules.InventorySystem;
using Newtonsoft.Json;

namespace Discord_BOT.Core.UserAccounts
{
    public static class UserAccounts
    {
        private static readonly List<UserAccount> Accounts;
        private const string AccountsFile = "Resources/accounts.json";

        static UserAccounts()
        {
            if (SaveExist())
                Accounts = LoadUserAccounts().ToList();
            else
            {
                Accounts = new List<UserAccount>();
                SaveAccounts();
            }
        }

        public static IEnumerable<UserAccount> LoadUserAccounts()
        {
            if (!File.Exists(AccountsFile)) return null;
            string json = File.ReadAllText(AccountsFile);
            return JsonConvert.DeserializeObject<List<UserAccount>>(json);
        }

        public static void SaveAccounts()
        {
            string json = JsonConvert.SerializeObject(Accounts);
            File.WriteAllText(AccountsFile, json);
        }

        public static bool SaveExist()
        {
            return File.Exists(AccountsFile);
        }

        public static UserAccount GetAccount(SocketUser user)
        {
            return GetOrCreateAccount(user.Id);
        }

        public static UserAccount GetAccount(ulong id)
        {
            return GetOrCreateAccount(id);
        }

        private static UserAccount GetOrCreateAccount(ulong id)
        {
            var account = Accounts.Find(x => x.Id == id) ?? CreateUserAccount(id);
            return account;
        }

        private static UserAccount CreateUserAccount(ulong id)
        {
            var newAccount = new UserAccount
            {
                Id = id,
                Cash = 0,
                Xp = 0,
                Level = 0,
                OpenedCases = 0,
                CasesProfit = 0,
                ActuallyColor = 0,
                Vip = false,
                Inventory = new Inventory()
            };

            newAccount.Inventory.Colors.Add(Colors.List[0]);

            Accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}
