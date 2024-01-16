using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.TakerylProject.Projectiles
{
	public class a_Aura : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Meteor");
		}
		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
            Projectile.alpha = 255;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 300;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.scale = 1f;
		}
        public override void AI()
        {
            int dust = 0;
            float dist = 120f;
            for (float r = 0; r < Math.PI * 2f; r += radians(dist))
            {
                if (dust++ % 16 == 0 && Main.rand.Next(4) == 0)
                {
                    float cos = Projectile.position.X + (float)(dist * Math.Cos(r));
                    float sine = Projectile.position.Y + (float)(dist * Math.Sin(r));
                    var d = Dust.NewDust(new Vector2(cos, sine), 1, 1, DustID.CrystalPulse);
                    Main.dust[d].noGravity = true;
                }
            }
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead && Main.player[i].Distance(Projectile.position) <= dist)
                {
                    if ((int)Main.time % 6 == 0)
                    {
                        if (Main.player[i].statLife < Main.player[i].statLifeMax)
                            Main.player[i].statLife++;
                        if (Main.player[i].statMana < Main.player[i].statManaMax)
                            Main.player[i].statMana++;
                    }
                }
            }
        }
        public const float radian = 0.017f;
        public float radians(float distance)
        {
            return radian * (45f / distance);
        }
    }
}