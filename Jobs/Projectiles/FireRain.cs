using ArchaeaMod.Effects;

using ArchaeaMod.NPCs;
using ArchaeaMod.NPCs.Bosses;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using rail;
using Steamworks;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Projectiles
{ 
	public class FireRain : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fire Rain");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
			Projectile.height = 68;
			Projectile.timeLeft = 600;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.DamageType = DamageClass.Ranged;
        }
        private Rectangle plrB, prjB;
		private static int ticks = 0;
		public override void AI()
		{
			ticks++;
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
			Lighting.AddLight((int)(Projectile.position.X + Projectile.width/2)/16, (int)(Projectile.position.Y + Projectile.height)/16, 0.7f, 0.2f, 0.1f);
			//Color newColor = default(Color);
			//int c = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + Projectile.height - 4f), 10, 10, 6, 0f, 0f, 100, newColor, 1.5f);
			//Main.dust[c].noGravity = true;
			if(ticks%30 == 0) {
				SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, Projectile.Center);
			}
			if(ticks%15 == 0)
			{	
				Projectile.netUpdate = true;
			}
			foreach(NPC N in Main.npc)
			{
				if(!N.active) continue;
				if(N.life <= 0) continue;
				if(N.friendly) continue;
				if(N.dontTakeDamage) continue;
				if(N.boss) continue;
				Rectangle MB = new Rectangle((int)Projectile.position.X+(int)Projectile.velocity.X,(int)Projectile.position.Y+(int)Projectile.velocity.Y,Projectile.width,Projectile.height);
				Rectangle NB = new Rectangle((int)N.position.X,(int)N.position.Y,N.width,N.height);
				if(MB.Intersects(NB))
				{
					N.AddBuff(24,600,false);
					ArchaeaNPC.StrikeNPC(N, 45, 0f, 0, false);
				}
			}
		}
        public override bool PreKill(int timeLeft)
        {
            Projectile P = Projectile;
			for(int i = (int)(P.position.X - (6f*Projectile.scale)); i < (int)(P.position.X + 6f + (6f*Projectile.scale)); i++)
			for(int j = (int)(P.position.Y + P.height - (6f*Projectile.scale)); j < (int)(P.position.Y + P.height + (6f*Projectile.scale)); j++)
			{
				if(i%2 == 0 && j%2 == 0) 
				{
					Color newColor = default(Color);
					int a = Dust.NewDust(new Vector2(i, j), 10, 10, 6, 0f, 0f, 100, newColor, 2.0f);
					int b = Dust.NewDust(new Vector2(i, j), 20, 20, 55, 0f, 0f, 100, newColor, 1.0f);
					Main.dust[a].noGravity = true;
					Main.dust[b].noGravity = true;
				}
			}
			if(Main.rand.NextBool(4)) 
			{ 
				SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, Projectile.Center);
			}
			return true;
		}
	}
}