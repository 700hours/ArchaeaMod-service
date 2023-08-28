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
        public override string Texture => "ArchaeaMod/Gores/MagnoGun_3";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Possessed Musket");
            // Tooltip.SetDefault("Leaves haunted impressions");
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
            Item.shootSpeed = 8f;
            //Item.UseSound = Shoot sound
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = Projectile.NewProjectile(source, position, velocity, Item.shoot, Item.damage, knockback, Item.playerIndexTheItemIsReservedFor);
            Main.projectile[index].rotation = player.AngleTo(Main.MouseWorld);
            ArchaeaItem.SyncProj(Main.projectile[index]);
            return false;
        }
        public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Gores/MagnoGun_3").Value;
            sb.Draw(tex, position, null, Color.SkyBlue * 0.9f, 0f, default, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Gores/MagnoGun_3").Value;
            sb.Draw(tex, Item.position - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex.Height), Color.SkyBlue * 0.9f, 0f, default, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override Vector2? HoldoutOffset() => new Vector2(-3, 0);
    }
}
