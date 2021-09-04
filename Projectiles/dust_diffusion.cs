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
            projectile.width = 10;
            projectile.height = 10;
            projectile.damage = 10;
            projectile.friendly = true;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.hide = true;
        }
        float distance = 0;
        int Type
        {
             get { return (int)projectile.ai[0]; }
        }
        public override void AI()
        {
            var dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, Type);
            Main.dust[dust].noGravity = true;
        }
    }
}
