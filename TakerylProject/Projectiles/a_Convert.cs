using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.TakerylProject.Projectiles
{
	public class a_Convert : ModProjectile
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
        private bool[] effect = new bool[256];
        public override void AI()
        {
            int dust = 0;
            float dist = 90f;
            for (float r = 0; r < Math.PI * 2f; r += radians(dist))
            {
                if (dust++ % 16 == 0 && Main.rand.Next(4) == 0)
                {
                    float cos = Projectile.position.X + (float)(dist * Math.Cos(r));
                    float sine = Projectile.position.Y + (float)(dist * Math.Sin(r));
                    var d = Dust.NewDust(new Vector2(cos, sine), 1, 1, DustID.AncientLight);
                    Main.dust[d].noGravity = true;
                }
            }
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.life > 0 && !npc.boss && npc.Distance(Projectile.Center) < dist)
                {
                    npc.friendly = true;
                }
            }
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].active && !effect[i] && !Main.player[i].dead && Main.player[i].Distance(Projectile.position) <= dist)
                {
                    Main.player[i].hostile = false;
                    effect[i] = true;
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