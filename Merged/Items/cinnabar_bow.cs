using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Items;
using ArchaeaMod.Merged.Items.Materials;
using Terraria.DataStructures;
namespace ArchaeaMod.Merged.Items
{
    public class cinnabar_bow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cinnabar Bow");
            Tooltip.SetDefault("Transforms wooden"
                + "\narrows into mercury arrows");
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 50;
            Item.scale = 1f;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useStyle = 5;
            Item.damage = 24;
            Item.crit = 9;
            Item.knockBack = 1.5f;
            Item.shootSpeed = 7.5f;
            Item.value = 2500;
            Item.rare = 1;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = false;
            Item.consumable = false;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.useAmmo = AmmoID.Arrow;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<magno_bar>(), 10)
                .AddIngredient(ModContent.ItemType<cinnabar_crystal>(), 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<Projectiles.cinnabar_arrow>();
                var proj = ModContent.GetModProjectile(type);
                int i = Projectile.NewProjectile(Projectile.GetSource_None(), position, velocity, type, (int)(proj.Projectile.damage + Item.damage * player.GetDamage(DamageClass.Ranged).Multiplicative), knockback, player.whoAmI);
                if (Main.netMode != NetmodeID.SinglePlayer)
                    Projectiles.cinnabar_arrow.SyncProj(Main.projectile[i]);
                return false;
            }
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
    }
}
