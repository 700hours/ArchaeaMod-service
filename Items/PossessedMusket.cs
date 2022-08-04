using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace ArchaeaMod.Items
{
    public class PossessedMusket : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possessed Musket");
            Tooltip.SetDefault("Leaves haunted impressions");
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 16;
            Item.value = 15000;
            Item.useTurn = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useAmmo = ItemID.MusketBall;
            Item.shoot = ModContent.ProjectileType<Projectiles.PossessedBullet>();
            Item.noMelee = true;
            Item.damage = 2;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.LightPurple;
            Item.DamageType = DamageClass.Ranged;
            //Item.UseSound = Shoot sound
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = Projectile.NewProjectile(source, position, velocity, Item.shoot, Item.damage, knockback, Item.playerIndexTheItemIsReservedFor);
            Main.projectile[index].rotation = player.AngleTo(Main.MouseWorld);
            Merged.Projectiles.cinnabar_arrow.SyncProj(Main.projectile[index]);
            return false;
        }
        public override Vector2? HoldoutOffset() => new Vector2(-3, 0);
    }
}
