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
        public bool LockClassSelect = true;
        public bool LockClassStats = true; 
        public static Locks Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public override void OnLoaded()
        {
            Instance = this;
        }
    }
}
