using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items
{
    public class debug : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.useTime = 10;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.value = 0;
            item.rare = 1;
            item.maxStack = 1;
            item.noMelee = true;
            item.createTile = ModContent.TileType<ArchaeaMod.Tiles.m_plants_large>();
        }
    }
}
