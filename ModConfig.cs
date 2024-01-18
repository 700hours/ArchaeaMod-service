using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ArchaeaMod
{
    internal class Locks : ModConfig
    {
        public bool OverrideClassSelect = false;
        public ClassOverride ForceClass = ClassOverride.All;
        //public bool LockClassStats = true; 
        //public bool LockArchaeaMode = true;
        public static Locks Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public override void OnLoaded()
        {
            Instance = this;
        }
    }
    public enum ClassOverride : int
    {
        Melee = 0,
        Ranged = 1,
        Magic = 2,
        Summoner = 3,
        All = 4
    }
}
