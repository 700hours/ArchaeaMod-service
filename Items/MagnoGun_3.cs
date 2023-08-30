using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace ArchaeaMod.Items
{
    public class MagnoGun_3 : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Rubidium Gun"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip0", "Lasers and phasers"));
        }
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 20;
            Item.damage = 18;
            Item.knockBack = 1.5f;
            Item.value = 3500;
            Item.crit = 8;
            Item.rare = 3;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 6f;
            Item.useAmmo = ItemID.MusketBall;
            Item.reuseDelay = 10;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.MagnoBullet>();
            Item.DamageType = DamageClass.Ranged;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextFloat() < 0.5f)
            {
                type = ModContent.ProjectileType<Projectiles.MagnoHomingBullet>();
                var proj = ModContent.GetModProjectile(type);
                int i = Projectile.NewProjectile(source, position, velocity, type, (int)(proj.Projectile.damage + Item.damage * player.GetDamage(DamageClass.Ranged).Multiplicative), knockback, player.whoAmI);
                Main.projectile[i].rotation = player.AngleTo(Main.MouseWorld);
                ArchaeaItem.SyncProj(Main.projectile[i]);
            }
            else
            {
                var proj = ModContent.GetModProjectile(Item.shoot);
                int i = Projectile.NewProjectile(source, position, velocity, Item.shoot, (int)(proj.Projectile.damage + Item.damage * player.GetDamage(DamageClass.Ranged).Multiplicative), knockback, player.whoAmI);
                Main.projectile[i].rotation = player.AngleTo(Main.MouseWorld);
                ArchaeaItem.SyncProj(Main.projectile[i]);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Merged.Items.Materials.magno_bar>(), 8)
                .AddIngredient(ModContent.ItemType<Merged.Items.Materials.cinnabar_crystal>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
    }
}
