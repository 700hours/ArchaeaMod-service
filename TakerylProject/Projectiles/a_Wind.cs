using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.TakerylProject.Projectiles
{
	public class a_Wind : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Wind");
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

        private int Direction()
        {
            return Projectile.oldPosition.X < Projectile.position.X ? -1 : 1;
        }
        public override void AI()
        {
            Dust.NewDust(Projectile.position, 1, 1, DustID.Smoke, Projectile.velocity.X / 1.2f, Projectile.velocity.Y / 1.2f);
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].active && i != Projectile.owner && Main.player[i].hostile && Main.player[i].team != Main.player[Projectile.owner].team && !Main.player[i].dead && Main.player[i].Hitbox.Intersects(Projectile.Hitbox))
                {
                    Main.player[i].velocity *= 6f * Direction();
                }
            }
        }
    }
}