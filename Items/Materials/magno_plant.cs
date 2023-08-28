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
            // DisplayName.SetDefault("Magno Plant");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = 1000;
        }
    }
}