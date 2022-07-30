using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Projectiles
{
    public class magno_yoyoprojectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mango Yoyo");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 8.5f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 240f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 13.5f;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = false;
            Projectile.scale = 1f;
            Projectile.CountsAsClass(DamageClass.Melee);
            Projectile.extraUpdates = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            bool random = Main.rand.Next(5) == 0;
            if (random)
            {
                for (float k = 0; k < MathHelper.ToRadians(360); k += 0.017f * 9)
                {
                    int Proj1 = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.position + new Vector2(Projectile.width / 2, Projectile.height / 2), Distance(null, k, 16f), ModContent.ProjectileType<dust_diffusion>(), Projectile.damage, 4f, Projectile.owner, ModContent.DustType<Dusts.magno_dust>());
                    if (Main.netMode == 1) NetMessage.SendData(27, -1, -1, null, Proj1);
                    //custom sound
                    //Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/IceBeamChargeShot"), projectile.position);
                    //vanilla sound
                    SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                }
            }
        }

        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }
    }
}
