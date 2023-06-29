using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Buffs;
using ArchaeaMod.Jobs.Global;
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
    internal class diffusion : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Diffusion");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1.6f;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 200;
            Projectile.friendly = true;
            Projectile.penetrate = 500;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
        }
        private const float Gravity = 0.0612f;
        int dustType => (int)Projectile.ai[0];
        int buffType => (int)Projectile.ai[1];
        int buffTime => (int)Projectile.localAI[0];
        bool gravity => (int)Projectile.localAI[1] == 1;
        public override void AI()
        {
            if (gravity)
            { 
			    Projectile.velocity.Y += Gravity;
            }
			Color color = new Color();
			int dust = Dust.NewDust(new Vector2((float) Projectile.position.X, (float) Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0, 0, 100, color, 3.0f);
			Main.dust[dust].noGravity = true;
            if (ArchaeaProjectiles.IsNotOldPosition(Projectile))
            {
                Projectile.netUpdate = true;
            }
			foreach(NPC N in Main.npc)
			{
				if(!N.active) continue;
				if(N.life <= 0) continue;
				if(N.friendly) continue;
				if(N.dontTakeDamage) continue;
				Rectangle MB = new Rectangle((int)Projectile.position.X+(int)Projectile.velocity.X,(int)Projectile.position.Y+(int)Projectile.velocity.Y,Projectile.width,Projectile.height);
				Rectangle NB = new Rectangle((int)N.position.X,(int)N.position.Y,N.width,N.height);
				if (MB.Intersects(NB))
				{
					N.AddBuff(buffType, buffTime, Main.netMode == 0);
				}
			}
		}
	}
}