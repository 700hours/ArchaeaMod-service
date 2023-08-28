using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Merged;
namespace ArchaeaMod.Merged.Items.Materials
{
    public class magno_bar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rubidium Bar");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;
            Item.value = 3500;
            Item.maxStack = 99;
            Item.rare = 2;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<ArchaeaMod.Merged.Tiles.m_bar>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Tiles.magno_ore>(), 4)
                .AddTile(TileID.Furnaces)
//            recipe.SetResult(this, 1);
                .Register();
        }
    }
}
