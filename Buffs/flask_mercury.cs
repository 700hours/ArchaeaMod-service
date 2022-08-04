﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Buffs
{
    public class flask_mercury : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mercury Edge");
            BuffID.Sets.IsAFlaskBuff[Type] = true;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
        }
    }
}
