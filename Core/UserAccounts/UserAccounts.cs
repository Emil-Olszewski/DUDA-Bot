using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord_BOT.Modules.ColorsManagment;

namespace Discord_BOT.Core.UserAccounts
{
    public static class UserAccounts
    {
        private static List<UserAccount> accounts;
        private static string accountsFile = "Resources/accounts.json";

        static UserAccounts()
        {   
            if(DataStorage.SaveExist(accountsFile))
            {
                accounts = DataStorage.LoadUserAccounts(accountsFile).ToList();
            }
            else
            {
                accounts = new List<UserAccount>();
                SaveAccounts();
            }
        }

        public static void SaveAccounts()
        {
            DataStorage.SaveUserAccounts(accounts, accountsFile);
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
            var result = from a in accounts where a.ID == id select a;
            var account = result.FirstOrDefault();
            if (account is null) account = CreateUserAccount(id);
            
            return account;
        }

        private static UserAccount CreateUserAccount(ulong id)
        {
            var newAccount = new UserAccount()
            {
                ID = id,
                Cash = 0,
                XP = 0,
                Level = 0,
                NumberOfKeys = 0,
                NumberOfCases = 0,
                OpenedCases = 0,
                CasesProfit = 0,
                ActuallyColor = 0
            };

            newAccount.Equipment = new List<Color>
            {
                Colors.AvalibleColors[0]
            };

            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}
