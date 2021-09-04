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

using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items
{
    public class r_Javelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rusty Spearhead");
            Tooltip.SetDefault("Rusty but still useful");
        }
        public override void SetDefaults()
        {
            item.width = 48;
            item.height = 48;
            item.damage = 0;
            item.knockBack = 0f;
            item.value = 100;
            item.rare = 1;
            item.useTime = 30;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.maxStack = 250;
            item.consumable = true;
            item.shoot = ModContent.ProjectileType<Javelin>();
            item.shootSpeed = 6f;
            item.thrown = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.r_plate>(), 1);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}
