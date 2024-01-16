using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.TakerylProject.Projectiles
{
	public class Meteor : ModProjectile
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
			Projectile.timeLeft = 150;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.scale = 0.75f;
		}
		private bool init;
		public override bool PreAI()
		{
			if (!init)
			{
				Projectile.rotation = (float)Math.PI / 4f;
				Projectile.scale = Main.rand.NextFloat(0.5f, 0.8f);
				init = true;
			}
			return init;
		}
		public override void AI()
		{
			if (Main.rand.Next(40) == 0)
			{
				var f = Dust.NewDust(Projectile.Center, 1, 2, 6, 0f, 0f, 0, default(Color), 1.5f);
				Main.dust[f].noGravity = true;
			}
			if (Main.rand.Next(60) == 0)
			{
				var d = Dust.NewDust(Projectile.Center, 1, 2, DustID.Smoke, 0f, 0f, 0, default(Color), 2f);
				Main.dust[d].noGravity = true;
			}
		}
	}
}