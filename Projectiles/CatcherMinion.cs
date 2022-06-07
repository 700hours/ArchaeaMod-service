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
    public class CatcherMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rusty Catcher");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 26;
            Projectile.damage = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

        private int ai = -1;
        private int rand;
        private int elapsed;
        private int tries;
        private float idleSpeed = 3f;
        private float roam = 90f;
        private float range = 400f;
        private float followSpeed = 6f;
        private Player owner
        {
            get { return Main.player[Projectile.owner]; }
        }
        private Target target;
        public override bool PreAI()
        {
            if (alpha && Projectile.alpha > 0)
            {
                Projectile.alpha -= 20;
            }
            else alpha = false;

            switch (ai)
            {
                case -1:
                    Projectile.position = owner.Center + new Vector2(32 * owner.direction, 0f);
                    goto case 0;
                case 0:
                    ai = 0;
                    break;
                case 1:
                    if (ArchaeaItem.Elapsed(60))
                        elapsed++;
                    if (elapsed == 5)
                        goto case 0;
                    float angle = NPCs.ArchaeaNPC.AngleTo(Projectile.Center, new Vector2(owner.Center.X, owner.position.Y + owner.height));
                    Projectile.velocity = NPCs.ArchaeaNPC.AngleToSpeed(angle, followSpeed);
                    if (Projectile.Hitbox.Intersects(owner.Hitbox))
                        goto case 2;
                    return false;
                case 2:
                    ai = 2;
                    Projectile.Center = owner.Center - new Vector2(owner.width / 2 * owner.direction, 16f);
                    if (target != null)
                        goto case 0;
                    break;
            }
            return true;
        }

        private int time;
        private Vector2 old;
        private Vector2 moveTo;
        private bool flag = false;
        private int num = 0;
        private bool alpha = false;
        public override void AI()
        {
            if (!owner.active || owner.dead || !owner.HasBuff(ModContent.BuffType<Buffs.buff_catcher>()))
                Projectile.active = false;
            if (target == null || target.npc.life <= 0 || !target.npc.active)
            {
                FindOwner();
                if (ArchaeaItem.Elapsed(180))
                {
                    target = Target.GetClosest(owner, Target.GetTargets(Projectile, 600f).Where(t => t != null).ToArray());
                    rand = Main.rand.Next(3);
                    old = Projectile.Center;
                    Vector2 speed;
                    switch (rand)
                    {
                        case 0:
                            speed = NPCs.ArchaeaNPC.AngleToSpeed(NPCs.ArchaeaNPC.RandAngle(), idleSpeed);
                            Projectile.velocity += speed;
                            break;
                        case 1:
                            speed = NPCs.ArchaeaNPC.AngleToSpeed((float)-Math.PI / 2f, idleSpeed);
                            moveTo = Projectile.Center + speed;
                            Tile ground = Main.tile[(int)moveTo.X / 16, (int)moveTo.Y / 16];
                            if (ground.HasTile)
                            {
                                Projectile.velocity += speed;
                                break;
                            }
                            else goto case 0;
                        case 2:
                            if (Main.rand.NextFloat() < 0.15f)
                                ai = 1;
                            break;
                    }
                }
                if (Projectile.Distance(old) > roam * 1.5f)
                    Projectile.velocity = Vector2.Zero;
            }
            else if (!FindOwner())
            {
                ai = 0;
                float angle = NPCs.ArchaeaNPC.AngleTo(Projectile.Center, target.npc.Center);
                if (ArchaeaItem.Elapsed(120) || !flag)
                {
                    Projectile.velocity = NPCs.ArchaeaNPC.AngleToSpeed(angle, 10f);
                    if (num++ >= 15)
                        flag = true;
                }
                else
                {
                    NPCs.ArchaeaNPC.VelocityClamp(ref Projectile.velocity, -6f, 6f);
                    if (num++ >= 100)
                    {
                        flag = false;
                        num = 0;
                    }
                }
            }
            FindGround(ai != 1);
        }
        protected bool FindGround(bool update = true)
        {
            if (update)
            {
                Tile ground = Main.tile[(int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16];
                if (!Main.tileSolid[ground.TileType])
                    Projectile.velocity.Y += 0.655f;
                return ground.HasTile;
            }
            return false;
        }
        protected bool FindOwner()
        {
            if (Projectile.Distance(owner.Center) > 400f)
            {
                Vector2 move = new Vector2(Main.rand.NextFloat(owner.position.X - 200f, owner.position.X + 200f), Main.rand.NextFloat(owner.position.Y - 200f, owner.position.Y + 200f));
                Tile ground = Main.tile[(int)move.X / 16, (int)move.Y / 16];
                if (Main.tileSolid[ground.TileType])
                {
                    if (CleanTeleport(move))
                    {
                        tries = 0;
                        ai = 0;
                        target = null;
                        alpha = true;
                    }
                }
                else
                {
                    ground = Main.tile[(int)move.X / 16, (int)move.Y / 16];
                }
                if (tries++ > 300)
                {
                    ai = 1;
                    tries = 0;
                }
                return true;
            }
            return false;
        }
        bool flag2 = false;
        bool flag3 = false;
        private bool CleanTeleport(Vector2 moveTo)
        {
            if (Projectile.alpha < 250)
            {
                Projectile.alpha += 20;
                return false;
            }
            else
            {
                Projectile.Center = moveTo;
                return true;
            }
        }
        public void FloatyAI()
        {
            if (target == null)
            {
                float angle = NPCs.ArchaeaNPC.AngleTo(Projectile.Center, owner.Center);
                if (owner.controlJump || Projectile.Distance(owner.Center) > 500f)
                    Projectile.velocity.X += NPCs.ArchaeaNPC.AngleToSpeed(angle, 1f).X;
                if (owner.controlLeft || owner.controlRight)
                {
                    Projectile.velocity.X += owner.velocity.X / Projectile.Distance(owner.Center);
                    if (Projectile.Center.X > owner.Center.X)
                        Projectile.velocity.X -= 0.2f;
                    else Projectile.velocity.X += 0.2f;
                }
                Projectile.velocity.Y = owner.velocity.Y;
                NPCs.ArchaeaNPC.VelocityClamp(Projectile, -8f, 8f);
            }
            else
            {

            }
        }
        public override bool PreKill(int timeLeft)
        {
            timeLeft = 36000;
            return false;
        }
    }
}
