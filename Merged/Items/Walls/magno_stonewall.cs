using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace ArchaeaMod.Merged.Items.Walls
{
    public class magno_stonewall : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Stone Wall");
            Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.createWall = ModContent.WallType<Merged.Walls.magno_stone>();
        }
        public override void AddRecipes()
        {
            var r = CreateRecipe()
                .AddIngredient(ModContent.ItemType<Tiles.magno_stone>())
                .AddTile(TileID.WorkBenches);
            r.ReplaceResult(Type, 4);
            r.Register();
        }
    }
}
