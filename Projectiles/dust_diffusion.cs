using System;
using System.IO;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ArchaeaMod.Items;
using ArchaeaMod.Projectiles;
namespace ArchaeaMod.Projectiles
{
    public class dust_diffusion : ModProjectile
    {
        public override string Texture => "ArchaeaMod/Gores/Null";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blast wave");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.damage = 10;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.hide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
        }
        public override bool? CanCutTiles() => false;
        int dustType
        {
            get { return (int)Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
        int owner
        {
            get { return Projectile.owner; }
        }
        float orbit = 0;
        public override void AI()
        {
            if ((int)Projectile.localAI[0] == 10)
            {
                orbit -= Draw.radian * 2f;
                double cos  = Projectile.localAI[1] * Math.Cos(orbit);
                double sine = Projectile.localAI[1] * Math.Sin(orbit);
                Projectile.position = Main.player[Projectile.owner].Center + new Vector2((float)cos, (float)sine);
                if (Projectile.position.X <= Projectile.oldPosition.X || Projectile.position.X > Projectile.oldPosition.X || Projectile.position.Y <= Projectile.oldPosition.Y || Projectile.position.Y > Projectile.oldPosition.Y)
                {
                    Projectile.netUpdate = true;
                }
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                return;
            }
            var dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
            Main.dust[dust].noGravity = true;
        }
    }
}
