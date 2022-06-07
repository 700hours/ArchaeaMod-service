using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Buffs
{
    public class buff_catcher : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rusty Catcher minion");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip = "Junky Rust";
        }
    }
}
