using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace ArchaeaMod.Items.Tiles
{
    public class factory_brick_1 : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //tooltips.Add(new TooltipLine(Mod, "ItemName", "Factory Brick"));
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
            Item.maxStack = 999;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ArchaeaMod.Tiles.factory_brick_1>();
        }
        public override void AddRecipes()
        {
            var r = CreateRecipe()
                .AddIngredient(Item.type)
                .AddTile(TileID.WorkBenches);
            r.ReplaceResult(ModContent.ItemType<Walls.factory_brickwall_1>(), 4);
            r.Register();
        }
    }
}
