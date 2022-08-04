using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchaeaMod.Projectiles
{
    public class ghostly_chains : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ghostly Chains");
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.damage = 0;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.stepSpeed = 8f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 60;
        }
        public override bool? CanCutTiles() => false;
        int target
        {
            get { return (int)Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
        int ai
        {
            get { return (int)Projectile.ai[1]; }
            set { Projectile.ai[1] = value; }
        }
        SpriteBatch sb => Main.spriteBatch;
        NPC npc => Main.npc[(int)Projectile.ai[0]];
        Vector2[] ground = new Vector2[3];
        const float MaxDistance = 300f;
        public override bool PreAI()
        {
            switch (ai)
            {
                case 0:
                    Projectile.penetrate = -1;
                    var n = NPCs.ArchaeaNPC.FindCloseNPCs(Projectile)[0];
                    if (n == default)
                        return false;
                    target = n.whoAmI;
                    goto case 1;
                case 1:
                    ai = 1;
                    Projectile.velocity = NPCs.ArchaeaNPC.AngleToSpeed(Projectile.Center.AngleTo(npc.Center), 5f);
                    if (Projectile.Hitbox.Intersects(npc.Hitbox))
                    {
                        npc.velocity = Vector2.Zero;
                        goto case 3;
                    }
                    break;
                case 2:     //  Unused index
                    ai = 2;
                    goto case 3;
                case 3:
                    ai = 3;
                    for (int i = 0; i < ground.Length; i++)
                    {
                        ground[i] = NPCs.ArchaeaNPC.AllSolidFloorsV2(npc);
                    }
                    goto case 4;
                case 4:
                    ai = 4;
                    Projectile.Center = npc.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
                    return true;
            }
            return false;
        }
        public override void AI()
        {
            if (!npc.active || npc.life <= 0)
                Projectile.Kill();
            else Projectile.timeLeft = 60;
            Vector2 longest = ground.OrderBy(t => t.Distance(npc.Center)).ToArray()[0];
            float distance = Vector2.Distance(longest, npc.Center);
            if (distance > 150f)
            {
                int directionX = npc.Center.X > longest.X ? 1 : -1;
                int directionY = npc.Center.Y > longest.Y ? 1 : -1;
                if (directionX == -1 && npc.velocity.X < 0f || directionX == 1 && npc.velocity.X > 0f)
                {
                    npc.velocity.X = -directionX * 2f;
                }
                if (directionY == -1 && npc.velocity.Y < 0f || directionY == 1 && npc.velocity.Y > 0f)
                {
                    npc.velocity.Y = -directionY * 2f;
                }
            }
            if (Projectile.velocity.X < 0f && Projectile.oldVelocity.X >= 0f || Projectile.velocity.X > 0f && Projectile.oldVelocity.X <= 0f || Projectile.velocity.Y < 0f && Projectile.oldVelocity.Y >= 0f || Projectile.velocity.Y > 0f && Projectile.oldVelocity.Y <= 0f)
                Projectile.netUpdate = true;
        }
        public override void PostDraw(Color lightColor)
        {
            if (!Projectile.Hitbox.Intersects(npc.Hitbox))
                return;
            for (int i = 0; i < ground.Length; i++)
            {
                for (int n = 0; n < ground[i].Distance(npc.Center); n += 12)
                {
                    double f = 1f;
                    if (n > ground[i].Distance(npc.Center) - 12) 
                        f = (ground[i].Distance(npc.Center) - n) / 12;
                    float angle = ground[i].AngleTo(npc.Center);
                    double cos  = ground[i].X + n * Math.Cos(angle);
                    double sine = ground[i].Y + n * Math.Sin(angle);
                    sb.Draw(Mod.Assets.Request<Texture2D>("Gores/chain").Value, 
                        new Vector2((float)cos, (float)sine) - Main.screenPosition, 
                        new Rectangle(0, 0, 12, (int)(12 * f)), Color.White, angle - Draw.radian * 90f, Vector2.Zero, 
                        1f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
