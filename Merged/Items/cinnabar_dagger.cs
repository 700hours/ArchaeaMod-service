using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod;
using ArchaeaMod.Merged.Items.Materials;
namespace ArchaeaMod.Merged.Items
{
    public class cinnabar_dagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cinnabar Dagger");
            Tooltip.SetDefault("Mercury-tipped blade");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = 1;
            Item.damage = 20;
            Item.crit = 9;
            Item.knockBack = 4f;
            Item.shootSpeed = 11f;
            Item.value = 1500;
            Item.rare = 1;
            Item.maxStack = 999;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Throwing;
            Item.shoot = Mod.Find<ModProjectile>("cinnabar_dagger").Type;
        }
        public override void AddRecipes()
        {
            var r = CreateRecipe()
                .AddIngredient(ModContent.ItemType<magno_bar>(), 2)
                .AddIngredient(ModContent.ItemType<cinnabar_crystal>(), 2)
                .AddTile(TileID.Anvils);
            r.ReplaceResult(Type, 10);
            r.Register();
        }
    }
}
