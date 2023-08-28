using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ArchaeaMod.Projectiles
{
    public class MagnoHomingBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rubidium Homing Bullet");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 2;
            Projectile.damage = 10;
            Projectile.knockBack = 0.2f;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.stepSpeed = 5f;
        }
        private int target
        {
            get { return (int)Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
        
        private NPC npc => Main.npc[target];
        public override bool PreAI()
        {
            var nPC = NPCs.ArchaeaNPC.FindClosestNPC(Projectile);
            if (nPC != default)
            {
                target = nPC.whoAmI;
                return true;
            }
            return false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.width = (int)(20f * (Projectile.velocity.Length() / Projectile.stepSpeed));
            if (Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.Center, npc.width, npc.height)) 
            {
                Projectile.velocity = Projectile.velocity.MoveTowards(npc.Center, Projectile.stepSpeed);
            }
        }
    }
}
