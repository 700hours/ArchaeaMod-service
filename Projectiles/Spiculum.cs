using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchaeaMod.Projectiles
{
    public class Spiculum : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spiculum");
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 10;
            Projectile.knockBack = 1f;
            Projectile.damage = 50;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.stepSpeed = 6f;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }
        int whoAmI
        { 
            get { return (int)Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
        float speed
        {
            get { return Projectile.ai[1]; }
            set { Projectile.ai[1] = value; }
        }
        NPC[] target;
        public override bool PreAI()
        {
            if (speed == 0f)
            { 
                speed = 6f;
            }
            return true;
        }
        public override void AI()
        {
            if (target == null)
            {
                target = NPCs.ArchaeaNPC.FindCloseNPCs(Projectile);
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (!target[whoAmI].active || target[whoAmI].life <= 0 || target[whoAmI].friendly)
            { 
                if (whoAmI < Main.npc.Length && target[whoAmI].Distance(Projectile.Center) < 1000f)
                { 
                    whoAmI++;
                }
                else
                {
                    Projectile.velocity = Projectile.velocity.MoveTowards(Main.player[Projectile.owner].position, speed);
                    if (Projectile.Hitbox.Intersects(Main.player[Projectile.owner].Hitbox))
                    {
                        Projectile.timeLeft = 600;
                        Projectile.Kill();
                    }
                }
            }
            if (target[whoAmI].Distance(Projectile.Center) < 1000f)
            {
                Projectile.penetrate = -1;
                Projectile.velocity = NPCs.ArchaeaNPC.AngleToSpeed(Projectile.Center.AngleTo(target[whoAmI].position), speed);
                if (!Projectile.Hitbox.Intersects(target[whoAmI].Hitbox))
                {   
                    Projectile.timeLeft = 2;
                }
            }
            else 
            { 
                Projectile.timeLeft = 600;
                Projectile.Kill();
            }
            if (Projectile.velocity.X < 0f && Projectile.oldVelocity.X >= 0f || Projectile.velocity.X > 0f && Projectile.oldVelocity.X <= 0f || Projectile.velocity.Y < 0f && Projectile.oldVelocity.Y >= 0f || Projectile.velocity.Y > 0f && Projectile.oldVelocity.Y <= 0f)
                Projectile.netUpdate = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.Kill();
        }
        public override bool PreKill(int timeLeft)
        {
            if (timeLeft < 300)
            { 
                speed *= 1.34f;
                int index = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position, Vector2.Zero, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, ++whoAmI, speed);
                Merged.Projectiles.cinnabar_arrow.SyncProj(Main.projectile[index]);
            }
            return true;
        }
    }
}
