using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Buffs;

using ArchaeaMod.NPCs;
using ArchaeaMod.NPCs.Bosses;
using ArchaeaMod.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Projectiles
{
    internal class t_effect : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("t_effect");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1;
            Projectile.timeLeft = 400;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
        }
        int ticks    = 0;
        int ai       => (int)Projectile.ai[0];
        int target   => (int)Projectile.ai[1];
        int buffType => (int)Projectile.localAI[0];
        int dustType => (int)Projectile.localAI[1];
        bool init    = false;
        NPC N        => Main.npc[target];
        public override bool PreAI()
        {
            if (!init)
            {
                Projectile.velocity.X += ArchaeaNPC.RandAngle() * 2f;
                Projectile.velocity.Y += ArchaeaNPC.RandAngle() * 2f;
                init = true;
            }
            return init;
        }
        public override void AI()
        {
            if (ticks++ > 10)
            { 
                Projectile.velocity = ArchaeaNPC.AngleToSpeed(Projectile.AngleTo(N.Center), 4f);
            }
            //ArchaeaNPC.VelocityClamp(Projectile, 0f, 4f);
            Dust.NewDust(Projectile.position, 2, 2, dustType);
            if (N.active && N.life > 0 && !N.friendly && !N.dontTakeDamage)
            { 
                Rectangle MB = new Rectangle((int)Projectile.position.X+(int)Projectile.velocity.X,(int)Projectile.position.Y+(int)Projectile.velocity.Y,Projectile.width,Projectile.height);
				Rectangle NB = new Rectangle((int)N.position.X,(int)N.position.Y,N.width,N.height);
				if (MB.Intersects(NB))
				{
					N.AddBuff(buffType, 600, Main.netMode == 0);
				}
            }
		}
	}
}