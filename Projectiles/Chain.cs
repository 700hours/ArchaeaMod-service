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
    public class Chain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flail");
        }
        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 6;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.friendly = true;
        }
        private int header
        {
            get { return (int)projectile.ai[1]; }
        }
        private int lead
        {
            get { return (int)projectile.ai[0]; }
        }
        private int spacing = 3;
        private float chaseSpeed = 5f;
        private int ai = -1;
        public override bool PreAI()
        {
            switch (ai)
            {
                case -1:
                    goto case 1;
                case 1:
                    ai = 1;
                    break;
            }
            return true;
        }
        public override void AI()
        {
            Projectile leader = Main.projectile[lead];
            Projectile head = Main.projectile[header];

            projectile.rotation = projectile.AngleTo(leader.Center) + MathHelper.ToRadians(90f);
            if (projectile.Distance(leader.Center) >= projectile.width + projectile.width / spacing)
            {
                chaseSpeed += 0.2f;
                float angle = projectile.AngleTo(leader.Center);
                float cos = (float)(chaseSpeed * Math.Cos(angle));
                float sine = (float)(chaseSpeed * Math.Sin(angle));
                projectile.velocity = new Vector2(cos, sine);
            }
            else
            {
                projectile.velocity = Vector2.Zero;
                chaseSpeed = 5f;
            }
            if (!head.active || !leader.active)
                projectile.active = false;
        }
    }
}
