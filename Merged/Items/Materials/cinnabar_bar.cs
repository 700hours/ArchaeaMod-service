using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace ArchaeaMod.Merged.Items.Materials
{
    public class cinnabar_bar : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cinnabar Bar");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;
            Item.value = 4500;
            Item.maxStack = 99;
            Item.rare = 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Tiles.magno_ore>(), 4)
                .AddIngredient(ModContent.ItemType<Materials.cinnabar_crystal>(), 1)
                .AddTile(TileID.Furnaces)
//            recipe.SetResult(this, 1);
                .Register();
        }
    }
}
