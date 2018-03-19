using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Discord_BOT.Core.UserAccounts;

namespace Discord_BOT.Misc.Modules.Investment
{
    public static class InvestmentsManager
    {
        private static List<Investment> investments;
        private static string investmentsPath = "Resources/investments.json";

        static InvestmentsManager()
        {
            if(SaveExist())
            {
                investments = LoadInvestments().ToList();
            }
            else
            {
                investments = new List<Investment>();
                SaveInvestments();
            }
        }
        
        private static bool SaveExist()
        {
            return File.Exists(investmentsPath);
        }

        private static IEnumerable<Investment> LoadInvestments()
        {
            if (!File.Exists(investmentsPath)) return null;
            string json = File.ReadAllText(investmentsPath);
            return JsonConvert.DeserializeObject<List<Investment>>(json);
        }

        private static void SaveInvestments()
        {
            string json = JsonConvert.SerializeObject(investments);
            File.WriteAllText(investmentsPath, json);
        }

        public static List<Investment> GetInvestments(UserAccount account)
        {
            return investments.FindAll(x => x.CustomerID == account.ID);
        }

        public static void AddInvestment(Investment investment)
        {
            investments.Add(investment);
            SaveInvestments();
        }

        public static void DeleteInvestment(uint id)
        {
            investments.Remove(investments.Find(x => x.ID == id));
            SaveInvestments();
        }
    }
}
