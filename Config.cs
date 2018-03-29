using System.IO;
using Newtonsoft.Json;

namespace Discord_BOT
{
    internal class Config
    {
        private const string ConfigFolder = "Resources";
        private const string ConfigFile = "config.json";
        public static Bot Bot;

        static Config()
        {
            if (!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);

            if (!File.Exists(ConfigFolder + "/" + ConfigFile))
            {
                Bot = new Bot();
                string json = JsonConvert.SerializeObject(Bot, Formatting.Indented);
                File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
            }
            else
            {
                string json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
                Bot = JsonConvert.DeserializeObject<Bot>(json);
            }
        }
    }

    public struct Bot
    {
        public ulong ServerId;
        public ulong GiveawayChannelId;
        public string Token;
        public string CmdPrefix;
        public ulong Id;
        public int BigDotationTime;
        public int BigDotationAmount;
        public int LittleDotationTime;
        public int LittleDotationAmount;
        public int KitVipTime;
        public int TransferTime;
        public int MinimalRoulette;
        public int MinimalTransfer;
        public int Cash5Min;
        public int MinimalInvestment;
        public int MinimalInvestmentTime;
        public int InvestmentTimeToDoubleMoney;
    }
}
