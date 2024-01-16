using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.TakerylProject.Projectiles
{
	public class d_Bog : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Bog");
		}
		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
            Projectile.alpha = 255;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1800;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.scale = 1f;
		}
        private bool[] effect = new bool[256];
        private int time = 0;
        public override void AI()
        {
            if (time++ > 600)
                time = 0;
            float dist = 200f;
            if (time % 8 == 0)
            {
                float randX = Projectile.Center.X + Main.rand.NextFloat(dist * -1, dist);
                float randY = Projectile.Center.Y + Main.rand.NextFloat(dist * -1, dist);
                var d = Dust.NewDust(new Vector2(randX, randY), 1, 1, DustID.Smoke, 0f, 0f, 0, default(Color), 3f);
                Main.dust[d].noGravity = true;
                if (Main.rand.NextBool())
                {
                    var g = Dust.NewDust(new Vector2(randX, randY), 1, 1, DustID.Grass, 0f, 0f, 0, default(Color), 2f);
                    Main.dust[g].noGravity = true;
                }
            }
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.life > 0 && !npc.boss && npc.Distance(Projectile.Center) < dist)
                {
                    npc.AddBuff(BuffID.Slow, 120);
                    npc.AddBuff(BuffID.Venom, 120);
                }
            }
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].active && !effect[i] && !Main.player[i].dead && Main.player[i].Distance(Projectile.position) <= dist)
                {
                    Main.player[i].AddBuff(BuffID.Slow, 90);
                    Main.player[i].AddBuff(BuffID.Venom, 90);
                }
            }
        }
    }
}