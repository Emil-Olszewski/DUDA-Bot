using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_BOT.Misc.Modules.UpgradingSystem
{
    public interface IUpgradable
    {
        int BaseID { get; set; }
        string BaseName { get; set; }
        int Level { get; set; }
        int MaxLevel { get; set; }

        int GetID();
        void SetName();
        void SetID();
        void SetLevel();
        void SetPrice();
        void Upgrade();
    }
}
