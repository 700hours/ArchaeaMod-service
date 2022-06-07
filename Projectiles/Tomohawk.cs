using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ArchaeaMod.Items;

namespace ArchaeaMod.Projectiles
{
    public class Tomohawk : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tomohawk");
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 50;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.damage = 24;
        }

        private int ai = -1;
        private int direction;
        private float yOffset = 16f;
        private Player owner
        {
            get { return Main.player[Projectile.owner]; }
        }
        public override bool PreAI()
        {
            switch (ai)
            {
                case -1:
                    Projectile.position.Y -= yOffset;
                    direction = owner.direction;
                    goto case 0;
                case 0:
                    ai = 0;
                    break;
            }
            return true;
        }

        public override void AI()
        {
            Projectile.rotation -= -Draw.radian * 5f * direction;
            Dusts(3);
            if (Projectile.timeLeft < 10)
            {
                if ((Projectile.alpha += 20) < 200)
                    Projectile.timeLeft = 10;
            }
        }
        public override void Kill(int timeLeft)
        {
            Dusts(8, true);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.StrikeNPC(Projectile.damage, Projectile.knockBack, target.position.X < Projectile.position.X ? -1 : 1, Main.rand.NextBool());
        }
        public override bool? CanHitNPC(NPC target)
        {
            return !target.townNPC;
        }
        protected void Dusts(int amount, bool noGravity = false)
        {
            for (int i = 0; i < amount; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X, Projectile.velocity.Y);
                dust.noGravity = noGravity;
            }
        }
    }
}
