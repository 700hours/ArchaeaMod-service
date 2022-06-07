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
    public class Javelin : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Javelin");
        }
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.damage = 28;
            Projectile.knockBack = 0f;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
        }
        private int ai = -1;
        private Player owner
        {
            get { return Main.player[Projectile.owner]; }
        }
        public override bool PreAI()
        {
            switch (ai)
            {
                case -1:
                    Projectile.rotation = NPCs.ArchaeaNPC.AngleTo(owner.Center, Main.MouseWorld) + (float)(Math.PI / 4f);
                    rotate = Projectile.rotation;
                    rotate += (float)Math.PI / 4f;
                    Projectile.position = new Vector2(ArchaeaItem.StartThrowX(owner), Projectile.position.Y - 16f);
                    goto case 0;
                case 0:
                    ai = 0;
                    break;
            }
            return true;
        }
        private int time = 90;
        private float rotate;
        private float x;
        private float y;
        private Vector2 hit;
        private Target target;
        private NPC npc;
        public override void AI()
        {
            if (ArchaeaItem.Elapsed(5))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 0, default(Color), 2f);
        }
    }
}
