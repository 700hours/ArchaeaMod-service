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
    public class debug_tile : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug Tile");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.value = 0;
            Item.rare = 1;
            Item.maxStack = 1;
            Item.noMelee = true;
            Item.createTile = ModContent.TileType<ArchaeaMod.Tiles.m_plants_small>();
        }
    }
}
