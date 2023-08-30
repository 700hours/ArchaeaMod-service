using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace ArchaeaMod.Items.Walls
{
    public class sky_brickwall : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Fortress Brick Wall"));
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 999;
            Item.rare = 1;
            Item.value = 0;
            Item.useTime = 7;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.consumable = true;
            Item.createWall = ModContent.WallType<ArchaeaMod.Walls.sky_brickwall>();
        }
        public override void AddRecipes()
        {
            var r = CreateRecipe()
                .AddIngredient(Item.type, 4)
                .AddTile(TileID.WorkBenches);
            r.ReplaceResult(ModContent.ItemType<Tiles.sky_brick>());
            r.Register();
        }
    }
}
