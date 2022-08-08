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
    public class Pixel : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shard");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.damage = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * alpha;
        }
        private bool direction;
        private int ai;
        public const int
            None = -1,
            Default = 0,
            Sword = 1,
            Active = 2,
            Gravity = 3,
            AntiGravity = 4;
        private float rotate;
        private float alpha;
        private Dust dust;
        private Player owner
        {
            get { return Main.player[Projectile.owner]; }
        }
        private float endY;
        public override bool PreAI()
        {
            switch (ai)
            {
                case 0:
                    direction = owner.direction == 1 ? true : false;
                    rotate = direction ? 0f : (float)Math.PI;
                    dust = SetDust();
                    endY = owner.position.Y;
                    goto case 1;
                case 1:
                    ai = 1;
                    break;
            }
            return true;
        }
        public void _AIType()
        {
            switch ((int)Projectile.ai[1])
            {
                case None:
                    Projectile.alpha = 0;
                    alpha = 1f;
                    Projectile.timeLeft = 100;
                    break;
                case Default:
                    dust.position = Projectile.position;
                    break;
                case Sword:
                    NPCs.ArchaeaNPC.RotateIncrement(true, ref rotate, (float)Math.PI / 2f, 0.15f, out rotate);
                    Projectile.velocity += NPCs.ArchaeaNPC.AngleToSpeed(rotate, 0.25f);
                    Projectile.tileCollide = Projectile.position.Y > endY;
                    dust.position = Projectile.position;
                    break;
                case Active:
                    dust = SetDust();
                    break;
                case AntiGravity:
                    if (alpha < 1f)
                    {
                        alpha += 0.02f;
                        Projectile.scale *= alpha;
                    }
                    Projectile.velocity.Y = -0.5f;
                    break;
            }
        }
        public override void AI()
        {
            _AIType();
        }
        public override void Kill(int timeLeft)
        {
            switch ((int)Projectile.ai[1])
            {
                case Default:
                    break;
                case Sword:
                    NPCs.ArchaeaNPC.DustSpread(Projectile.Center, 1, 1, 6, 4, 2f);
                    if (Projectile.ai[0] == Mercury)
                        Projectile.NewProjectileDirect(Projectile.GetSource_Death(), new Vector2(owner.position.X, owner.position.Y - 600f), Vector2.Zero, ModContent.ProjectileType<Mercury>(), 20, 4f, owner.whoAmI, Projectiles.Mercury.Falling, Projectile.position.X);
                    break;
            }
        }
        public const int
            Fire = 1,
            Dark = 2,
            Mercury = 3,
            Electric = 4;
        private Dust defaultDust
        {
            get { return Dust.NewDustDirect(Vector2.Zero, 1, 1, 0); }
        }
        public Dust SetDust()
        {
            switch ((int)Projectile.ai[0])
            {
                case 0:
                    break;
                case Fire:
                    return Dust.NewDustDirect(Projectile.Center, 2, 2, 6, 0f, 0f, 0, default(Color), Main.rand.NextFloat(1f, 3f));
                case Mercury:
                    return Dust.NewDustDirect(Projectile.Center, 2, 2, 6, 0f, 0f, 0, default(Color), Main.rand.NextFloat(1f, 3f));
                case Electric:
                    Dust dust = Dust.NewDustDirect(Projectile.Center, 2, 2, 6, 0f, 0f, 0, default(Color), Main.rand.NextFloat(1f, 3f));
                    dust.noGravity = true;
                    return dust;
            }
            return defaultDust;
        }
    }

}
