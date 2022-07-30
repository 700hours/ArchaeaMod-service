using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Merged.Items.Materials;
namespace ArchaeaMod.Merged.Items.Tiles
{
    public class magno_chest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnoliac Chest");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.value = 0;
            Item.rare = 1;
            Item.maxStack = 999;
            Item.createTile = ModContent.TileType<Merged.Tiles.m_chest>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<magno_bar>(), 2)
                .AddIngredient(9, 8)
                .AddTile(TileID.WorkBenches)
//            recipe.SetResult(this, 1);
                .Register();
        }
    }
}
