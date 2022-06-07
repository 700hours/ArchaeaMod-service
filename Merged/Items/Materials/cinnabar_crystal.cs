using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items.Materials
{
    public class cinnabar_crystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cinnabar Crystal");
            Tooltip.SetDefault("Glows with a crimson aura");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;
            Item.value = 1000;
            Item.maxStack = 99;
            Item.rare = 2;
            Item.createTile = ModContent.TileType<ArchaeaMod.Tiles.c_crystal_block>();
        }
    }
}
