using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Discord_BOT.Core.UserAccounts;

namespace Discord_BOT.Misc.Modules.Investment
{
    public static class InvestmentsManager
    {
        private static readonly List<Investment> Investments;
        private const string InvestmentsPath = "Resources/investments.json";

        static InvestmentsManager()
        {
            if (SaveExist())
            {
                Investments = LoadInvestments().ToList();
            }
            else
            {
                Investments = new List<Investment>();
                SaveInvestments();
            }
        }

        private static bool SaveExist()
        {
            return File.Exists(InvestmentsPath);
        }

        private static IEnumerable<Investment> LoadInvestments()
        {
            if (!File.Exists(InvestmentsPath)) return null;
            string json = File.ReadAllText(InvestmentsPath);
            return JsonConvert.DeserializeObject<List<Investment>>(json);
        }

        private static void SaveInvestments()
        {
            string json = JsonConvert.SerializeObject(Investments);
            File.WriteAllText(InvestmentsPath, json);
        }

        public static List<Investment> GetInvestments(UserAccount account)
        {
            return Investments.FindAll(x => x.CustomerId == account.Id);
        }

        public static void AddInvestment(Investment investment)
        {
            Investments.Add(investment);
            SaveInvestments();
        }

        public static void DeleteInvestment(uint id)
        {
            Investments.Remove(Investments.Find(x => x.Id == id));
            SaveInvestments();
        }
    }
}
