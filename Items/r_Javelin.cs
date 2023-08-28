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
            // DisplayName.SetDefault("Rusty Spearhead");
            // Tooltip.SetDefault("Rusty but still useful");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 38;
            Item.knockBack = 0f;
            Item.value = 100;
            Item.rare = 1;
            Item.crit = 14;
            Item.useTime = 30;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.shoot = ModContent.ProjectileType<Javelin>();
            Item.shootSpeed = 6f;
            Item.DamageType = DamageClass.Throwing;
        }
        public override void AddRecipes()
        {
            var r = CreateRecipe()
                .AddTile(TileID.MythrilAnvil)
                .AddIngredient(ModContent.ItemType<Items.Materials.r_plate>(), 1);
            r.ReplaceResult(Type, 5);
            r.Register();
        }
    }
}
