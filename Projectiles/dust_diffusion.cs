using System;
using System.IO;
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
        }
        float distance = 0;
        int Type
        {
             get { return (int)Projectile.ai[0]; }
        }
        public override void AI()
        {
            var dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Type);
            Main.dust[dust].noGravity = true;
        }
    }
}
