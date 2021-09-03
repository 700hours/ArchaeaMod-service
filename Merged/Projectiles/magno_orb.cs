using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ArchaeaMod.Items;
using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Merged.Projectiles
{
    public class magno_orb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Orb");
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.scale = 1.2f;
            projectile.aiStyle = -1;
            projectile.timeLeft = 3600;
            projectile.damage = 10;
            projectile.knockBack = 7.5f;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.ownerHitCheck = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.magic = true;
            projectile.netImportant = true;
        }

        public void Initialize()
        {
            oldAngle = (float)Math.Atan2(Main.MouseWorld.Y - projectile.position.Y, Main.MouseWorld.X - projectile.position.X);
            projectile.velocity *= 2f;
        }
        bool init = false;
        int ticks = 0;
        int dustType;
        float oldAngle;
        Player player;
        public override void AI()
        {
            if (!init)
            {
                Initialize();
                init = true;
            }
            Player player = Main.player[projectile.owner];

            float Angle = (float)Math.Atan2(Main.MouseWorld.Y - projectile.position.Y, Main.MouseWorld.X - projectile.position.X);
            if (Main.mouseLeft && Vector2.Distance(Main.MouseWorld - projectile.position, Vector2.Zero) < 128)
            {
                projectile.rotation = Angle;
                Rectangle mouseBox = new Rectangle((int)Main.MouseWorld.X - 8, (int)Main.MouseWorld.Y - 8, 16, 16);
                if (!mouseBox.Intersects(projectile.Hitbox))
                    projectile.velocity = Distance(null, Angle, 8f);
                else projectile.velocity = Vector2.Zero;

                oldAngle = (float)Math.Atan2(Main.MouseWorld.Y - projectile.Center.Y, Main.MouseWorld.X - projectile.Center.X);
            }
            else
            {
                projectile.velocity = Distance(null, oldAngle, 16f);
                projectile.rotation = oldAngle;
            }
            if (Vector2.Distance(player.position - projectile.position, Vector2.Zero) > 1024)
            {
                projectile.Kill();
            }
            dustType = mod.DustType("magno_dust");
            for (int k = 0; k < 1; k++)
            {
                int orbDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 0, default(Color), 1f);
                Main.dust[orbDust].noGravity = true;
            }
            if (projectile.velocity.X <= projectile.oldVelocity.X || projectile.velocity.X > projectile.oldVelocity.X || projectile.velocity.Y <= projectile.oldVelocity.Y || projectile.velocity.Y > projectile.oldVelocity.Y)
                projectile.netUpdate = true;
        }
        public override void Kill(int timeLeft)
        {
            int num = 48 * 3;
            int num2 = 24 * 3;
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(projectile.Center - new Vector2(num2, num2), num, num, ModContent.DustType<Dusts.magno_dust>(), 0, 0, 0, default, 2);
                Dust.NewDust(projectile.Center - new Vector2(num2, num2), num, num, DustID.Smoke, 0, 0, 0, default, 2f);
            }
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(projectile.Center) < num)
                {
                    npc.StrikeNPC(projectile.damage, projectile.knockBack, npc.position.X < projectile.position.X ? -1 : 1, Main.rand.NextBool());
                }
            }
            //for (float k = 0; k < MathHelper.ToRadians(360); k += 0.017f * 9)
            //{
            //    int Proj1 = Projectile.NewProjectile(projectile.position + new Vector2(projectile.width / 2, projectile.height / 2), Distance(null, k, 16f), mod.ProjectileType("dust_diffusion"), projectile.damage, 7.5f, projectile.owner, projectile.Center.X, projectile.Center.Y);
            //    //  if (Main.netMode == 1) NetMessage.SendData(27, -1, -1, null, Proj1);
            //}
            Main.PlaySound(2, projectile.position, 14);
        }

        public void SyncProj(int netID)
        {
            if (Main.netMode == netID)
            {
            }
        }

        public Vector2 AB(int rise, int run)
        {
            return new Vector2(rise, run);
        }
        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }
    }
}
