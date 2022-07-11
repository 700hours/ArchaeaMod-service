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
    public class r_Tomohawk : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rusty Tomohawk");
            Tooltip.SetDefault("Was once shiny");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 30;
            Item.knockBack = 2f;
            Item.value = 100;
            Item.rare = 2;
            Item.maxStack = 999;
            Item.useTime = 30;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ModContent.ProjectileType<Tomohawk>();
            Item.shootSpeed = 7f;
            Item.DamageType = DamageClass.Throwing;
            Item.consumable = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            var r = CreateRecipe()
                .AddTile(TileID.MythrilAnvil)
                .AddIngredient(ModContent.ItemType<Items.Materials.r_plate>(), 1);
            r.ReplaceResult(Type, 7);
            r.Register();
        }
    }
}
