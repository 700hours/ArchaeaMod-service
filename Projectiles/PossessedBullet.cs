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
    public class PossessedBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possessed Bullet");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 2;
            Projectile.damage = 6;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.stepSpeed = 6f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
        }
        Vector2 impact = Vector2.Zero;
        int npc = 0;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            impact = ArchaeaMain.Impact(Projectile, target.Hitbox);
            npc = target.whoAmI;
            NPC.NewNPC(NPC.GetSource_None(), (int)impact.X, (int)impact.Y, ModContent.NPCType<NPCs.PossessedBullet>(), Target: npc);
        }
        public override void AI()
        {
            if (Projectile.velocity.X < 0f && Projectile.oldVelocity.X >= 0f || Projectile.velocity.X > 0f && Projectile.oldVelocity.X <= 0f || Projectile.velocity.Y < 0f && Projectile.oldVelocity.Y >= 0f || Projectile.velocity.Y > 0f && Projectile.oldVelocity.Y <= 0f)
                Projectile.netUpdate = true;
        }
        public override void Kill(int timeLeft)
        {
        }
    }
}
