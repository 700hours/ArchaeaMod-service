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
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blast wave");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.damage = 10;
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
        public override void AI()
        {
            if ((int)Projectile.localAI[0] == 10)
            {
                float orbit = Projectile.localAI[1] -= Draw.radian * 2f;
                double cos  = Projectile.ai[1] * Math.Cos(orbit);
                double sine = Projectile.ai[1] * Math.Sin(orbit);
                Projectile.position = Main.player[Projectile.owner].Center + new Vector2((float)cos, (float)sine);
                if (Projectile.position != Projectile.oldPosition)
                    Projectile.netUpdate = true;
            }
            var dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
            Main.dust[dust].noGravity = true;
        }
    }
}
