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
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
        }
        private int header
        {
            get { return (int)Projectile.ai[1]; }
        }
        private int lead
        {
            get { return (int)Projectile.ai[0]; }
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

            Projectile.rotation = Projectile.AngleTo(leader.Center) + MathHelper.ToRadians(90f);
            if (Projectile.Distance(leader.Center) >= Projectile.width + Projectile.width / spacing)
            {
                chaseSpeed += 0.2f;
                float angle = Projectile.AngleTo(leader.Center);
                float cos = (float)(chaseSpeed * Math.Cos(angle));
                float sine = (float)(chaseSpeed * Math.Sin(angle));
                Projectile.velocity = new Vector2(cos, sine);
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                chaseSpeed = 5f;
            }
            if (!head.active || !leader.active)
                Projectile.active = false;
        }
    }
}
