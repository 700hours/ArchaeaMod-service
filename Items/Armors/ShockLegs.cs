using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace ArchaeaMod.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class ShockLegs : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shock Greaves");
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.defense = 8;
            Item.rare = ItemRarityID.Orange;
            Item.value = 5000;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.Autohammer)
                .AddIngredient(ModContent.ItemType<Items.Materials.r_plate>(), 10)
//            recipe.SetResult(this, 1);
                .Register();
        }
    }
}
