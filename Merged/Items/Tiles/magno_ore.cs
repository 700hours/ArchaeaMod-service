using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items.Tiles
{
    public class magno_ore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rubidium Ore");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.scale = 1f;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.value = 500;
            Item.rare = 1;
            Item.maxStack = 999;
            Item.createTile = ModContent.TileType<Merged.Tiles.m_ore>();
        }
    }
}
