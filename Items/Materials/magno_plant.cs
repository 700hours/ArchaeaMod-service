using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items.Materials
{
    public class magno_plant : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Plant");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 24;
            item.maxStack = 999;
            item.value = 1000;
        }
    }
}