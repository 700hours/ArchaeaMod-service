using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items.Tiles
{
    public class cinnabar_ore : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cinnabar Ore");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.scale = 1f;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.value = 1000;
            Item.rare = 1;
            Item.maxStack = 250;
            Item.createTile = ModContent.TileType<Merged.Tiles.c_ore>();
        }
    }
}
