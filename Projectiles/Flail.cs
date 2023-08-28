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
    public class Flail : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flail");
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
        }

        private int ai = -1;
        private int style
        {
            get { return (int)Projectile.ai[0]; }
        }
        public const int
            Fling = 1,
            Swing = 2;
        private Vector2 oldMouse;
        private Vector2 mouse;
        private Vector2 velocity;
        private Player owner
        {
            get { return Main.player[Projectile.owner]; }
        }
        public override bool PreAI()
        {
            switch (ai)
            {
                case -1:
                    int maxParts = 400 / 12;
                    int[] parts = new int[maxParts];
                    parts[0] = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<Chain>(), 0, 0f, Projectile.owner, Projectile.whoAmI, Projectile.whoAmI);
                    Main.npc[parts[0]].whoAmI = parts[0];
                    for (int i = 1; i < maxParts; i++)
                    {
                        parts[i] = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<Chain>(), 0, 0f, Projectile.owner, parts[i - 1], Projectile.whoAmI);
                        Main.npc[parts[i]].whoAmI = parts[i];
                    }
                    goto case 0;
                case 0:
                    ai = 0;
                    if (ArchaeaItem.Elapsed(5))
                        Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                    break;
            }
            return true;
        }
        private bool collide;
        private bool reach = true;
        private int time;
        private const int maxTime = 180;
        private float angle;
        private float speed = 1f;
        private float range = 200f;
        private float distance;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.StrikeNPC(Projectile.damage, Projectile.knockBack, target.position.X < Projectile.position.X ? -1 : 1, Main.rand.NextBool());
        }
        public override bool? CanHitNPC(NPC target)
        {
            return !target.townNPC;
        }
        public override void AI()
        {
            switch (style)
            {
                case Fling:
                    if (Projectile.Distance(owner.Center) > range)
                        collide = true;
                    break;
                case Swing:
                    if (!Main.mouseRight)
                        collide = true;
                    FloatyAI();
                    if (owner.controlUseItem)
                        Projectile.timeLeft = 50;
                    if (Projectile.Distance(owner.Center) > range)
                    {
                        velocity = Vector2.Zero;
                        angle = NPCs.ArchaeaNPC.AngleTo(Projectile.Center, owner.Center);
                        Vector2 reverse = NPCs.ArchaeaNPC.AngleToSpeed(angle, 4f);
                        Vector2.Add(ref Projectile.velocity, ref reverse, out Projectile.velocity);
                        if (reach)
                        {
                            velocity = NPCs.ArchaeaNPC.AngleToSpeed(angle + (float)Math.PI, 8f);
                            for (int i = 0; i < 8; i++)
                                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(Projectile.height)), velocity, ModContent.ProjectileType<Pixel>(), Projectile.damage, Projectile.knockBack, owner.whoAmI, Pixel.Fire, Pixel.Sword);
                            for (int j = 0; j < 6; j++)
                                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                            reach = false;
                        }
                        break;
                    }
                    else angle = NPCs.ArchaeaNPC.AngleTo(Projectile.Center, Main.MouseWorld);
                    if (Projectile.Center.Y < owner.Center.Y && !HitTile())
                        Projectile.velocity.Y += 0.665f * 3f;
                    else Projectile.velocity = Vector2.Zero;
                    velocity += NPCs.ArchaeaNPC.AngleToSpeed(angle, 0.5f);
                    NPCs.ArchaeaNPC.VelocityClamp(ref velocity, -6f, 6f);
                    Projectile.Center += velocity;
                    break;
            }
            if (collide)
            {
                angle = NPCs.ArchaeaNPC.AngleTo(Projectile.Center, owner.Center);
                Projectile.Center += NPCs.ArchaeaNPC.AngleToSpeed(angle, 2f * (speed += 0.25f));
                if (Projectile.Hitbox.Intersects(owner.Hitbox))
                    Projectile.active = false;
            }
            if (!reach)
            {
                if (time++ > maxTime)
                {
                    time = 0;
                    reach = true;
                }
            }
        }
        public Texture2D chain
        {
            get { return Mod.Assets.Request<Texture2D>("Gores/chain").Value; }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            
            return true;
        }

        private bool collision;
        public void FloatyAI()
        {
            switch (TileCollide())
            {
                case Collide.Bottom:
                    Projectile.velocity.Y = 0f;
                    goto default;
                case Collide.Left:
                    Projectile.velocity.X = 0f;
                    goto default;
                case Collide.Right:
                    Projectile.velocity.X = 0f;
                    goto default;
                case Collide.Top:
                    Projectile.velocity.Y = 0f;
                    goto default;
                default:
                    collision = true;
                    break;
            }
            if (collision)
            {
                if (TileCollide() != Collide.Bottom)
                {
                    Projectile.velocity.Y += 0.655f;
                }
            }
            NPCs.ArchaeaNPC.VelocityClamp(Projectile, -6f, 6f);
        }
        protected bool HitTile()
        {
            for (int l = -8; l < Projectile.height + 8; l++)
            {
                for (int k = -8; k < Projectile.width + 8; k++)
                {
                    int i = (int)Projectile.position.X / 16 + k;
                    int j = (int)Projectile.position.Y / 16 + l;
                    Tile tile = Main.tile[i, j];
                    if (tile.HasTile && Main.tileSolid[tile.TileType])
                        return true;
                }
            }
            return false;
        }
        protected Collide TileCollide()
        {
            int i = (int)Projectile.Center.X / 16;
            int j = (int)Projectile.Center.Y / 16;
            Tile top = Main.tile[i, j - 1];
            Tile left = Main.tile[i - 1, j];
            Tile bottom = Main.tile[i, j + 1];
            Tile right = Main.tile[i + 1, j];
            if (top.HasTile && Main.tileSolid[top.TileType])
                return Collide.Top;
            if (left.HasTile && Main.tileSolid[left.TileType])
                return Collide.Left;
            if (bottom.HasTile && Main.tileSolid[bottom.TileType])
                return Collide.Bottom;
            if (right.HasTile && Main.tileSolid[right.TileType])
                return Collide.Right;
            return Collide.None;
        }
        public enum Collide : byte
        {
            None = 0,
            Top = 1,
            Left = 2,
            Bottom = 3,
            Right = 4
        }
    }
}
