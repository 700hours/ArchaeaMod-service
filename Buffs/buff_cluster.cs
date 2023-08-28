﻿using System;
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
    public class buff_cluster : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cluster Count");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            int stacks = Main.LocalPlayer.GetModPlayer<Items.AccPlayer>().stack;
            tip = stacks > 1 ? stacks +" stacks" : "1 stack";
        }
    }
}
