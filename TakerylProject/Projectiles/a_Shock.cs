using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.TakerylProject.Projectiles
{
	public class a_Shock : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Shock Nova");
		}
		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
            Projectile.alpha = 255;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 100;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.scale = 1f;
		}
        private int ItemType => (int)Projectile.ai[0];
        private bool[] beenHit = new bool[256];
        private bool[] npcHit = new bool[Main.npc.Length];
        private float dist = 45f;
        public override void AI()
        {
            int dust = 0;
            dist += 3f;
            for (float r = 0; r < Math.PI * 2f; r += radians(dist))
            {
                if (dust++ % 8 == 0 && Main.rand.Next(3) == 0)
                {
                    float cos = (float)(dist * Math.Cos(r));
                    float sine = (float)(dist * Math.Sin(r));
                    var d = Dust.NewDust(new Vector2(Projectile.position.X + cos, Projectile.position.Y + sine), 1, 1, DustID.WitherLightning, cos / dist, sine / dist);
                    Main.dust[d].noGravity = true;
                }
            }
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npcHit[npc.whoAmI] && npc.life > 0 && !npc.boss && npc.Distance(Projectile.Center) < dist)
                {
                    npc.AddBuff(BuffID.Webbed, 210);
                    npcHit[npc.whoAmI] = true;
                }
            }
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].active && i != Projectile.owner && !beenHit[i] && Main.player[i].hostile && Main.player[i].team != Main.player[Projectile.owner].team && !Main.player[i].dead && Main.player[i].Distance(Projectile.position) < dist)
                {
                    Main.player[i].AddBuff(BuffID.Webbed, 180);
                    Main.player[i].Hurt(PlayerDeathReason.ByPlayerItem(Projectile.owner, Main.player[Projectile.owner].HeldItem), 50, Main.player[i].position.X < Projectile.position.X ? -1 : 1, true);
                    beenHit[i] = true;
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