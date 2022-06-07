using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Projectiles
{
    public class magno_minionexplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Minion Explosion");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;
            Projectile.scale = 1f;
            Projectile.damage = 0;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 60;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Lighting.AddLight(new Vector2(Projectile.position.X / 16, Projectile.position.Y / 16), new Vector3(0.4f, 0.5f, 0.25f));

            Projectile.frameCounter++;
            if(Projectile.frameCounter > 3)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if(Projectile.frame > 3)
            {
                Projectile.Kill();
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = Color.PaleGoldenrod;
        }
    }
}
