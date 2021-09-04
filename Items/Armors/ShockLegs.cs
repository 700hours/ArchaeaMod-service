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
            item.width = 22;
            item.height = 18;
            item.defense = 5;
            item.rare = ItemRarityID.Orange;
            item.value = 5000;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.Autohammer);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.r_plate>(), 10);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
