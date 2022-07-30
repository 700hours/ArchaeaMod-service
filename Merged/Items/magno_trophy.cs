using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items
{
    public class magno_trophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnoliac Trophy");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.scale = 1f;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.value = 5000;
            Item.rare = 3;
            Item.maxStack = 99;
            Item.createTile = ModContent.TileType<Merged.Tiles.m_trophy>();
        }
    }
}
