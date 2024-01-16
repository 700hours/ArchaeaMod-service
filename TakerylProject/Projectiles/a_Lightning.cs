using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.TakerylProject.Projectiles
{
	public class a_Lightning : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Lightning Bolt");
		}
		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
            Projectile.alpha = 255;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.scale = 1f;
		}

        private bool init;
        private int next = 1, casts;
        private float weight;
        private Vector2[] connect = new Vector2[8];
        private Vector2 dest, start;
        internal bool GeneratePath()
        {
            if (!init)
            {
                float height = 800f;
                dest = Projectile.position;
                Projectile.position.Y -= height;
                Projectile.position.X += Main.rand.NextFloat(-100, 100);
                for (int i = 0; i < connect.Length - 1; i++)
                {
                    connect[i] = new Vector2(dest.X + Main.rand.NextFloat(-100, 100), Projectile.position.Y + (height / connect.Length) * i);
                }
                connect[connect.Length - 1] = dest;
                init = true;
            }
            return init;
        }
        public override bool PreAI()
        {
            return GeneratePath();
        }
        public override void AI()
        {
            if (Projectile.position.Y < connect[next].Y)
            {
                for (float i = 0; i < 1f; i += 0.025f)
                {
                    var d = Dust.NewDust(Projectile.Center, 1, 1, DustID.WitherLightning);
                    Main.dust[d].noGravity = true;
                    Projectile.position = Vector2.Lerp(connect[next - 1], connect[next], i);
                }
            }
            else if (next < connect.Length)
                next++;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Webbed, 150);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {   
            target.AddBuff(BuffID.Webbed, 210);
        }
    }
}