using ArchaeaMod.Items;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Projectiles
{
    internal static class FollowID
    {
        public const int
            Minion = 0,
            Follower = 1,
            Replace = 2;
    }
    internal class fake_npc : ModProjectile
    {
        public override string Texture => "ArchaeaMod/Gores/Null";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("fake_npc");
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 32;
            Projectile.height = 42;
            Projectile.damage = 0;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }
        int ownerIndex
        {
            get { return (int)Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
        int followerID
        {
            get { return (int)Projectile.ai[1]; }
            set { Projectile.ai[1] = value; }
        }
        int ownerType
        {
            get { return (int)Projectile.localAI[0]; }
            set { Projectile.localAI[0] = value; }
        }
        int followType
        {
            get { return (int)Projectile.localAI[1]; }
            set { Projectile.localAI[1] = value; }
        }
        bool init = false;
        bool beginMove = false;
        float rand = 0f;
        int ticks = 0;
        int ticks2 = 0;
        NPC owner => Main.npc[ownerIndex];
        IList<Vector2> oldVelocity = new List<Vector2>();
        IList<Vector2> oldVelocity2 = new List<Vector2>();
        public override bool? CanCutTiles() => false;
        public static int SetFollowType(Projectile projectile, int type)
        {
            return (int)(projectile.localAI[1] = type);
        }
        private bool PlayerNotControlMove(Player player)
        {
            return player != null && !player.controlUp && !player.controlRight && !player.controlDown && !player.controlLeft && !player.controlJump;
        }
        private bool PlayerMoving(Player player)
        {
            return player.velocity.X != 0f && player.velocity.Y != 0f;
        }
        private bool FollowerMoving(Projectile follower)
        {
            return follower.position.X <= follower.oldPosition.X || follower.position.X > follower.oldPosition.X || follower.position.Y <= follower.oldPosition.Y || follower.position.Y > follower.oldPosition.Y;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.townNPC || target.friendly || target.CountsAsACritter)
                return;
            if (ArchaeaItem.Elapsed(ref ticks2, 20))
            {
                ArchaeaNPC.StrikeNPC(target, Main.hardMode ? 40 : 20, 2f, target.Center.X < Projectile.Center.X ? -1 : 1, Main.rand.NextBool());
                ticks2 = 0;
            }
        }
        public override bool PreAI()
        {
            if (!init)
            {
                rand = Main.rand.Next(-50, 50) + 1;
                init = true;
                owner.friendly = true;
            }
            owner.velocity = Projectile.velocity;
            owner.position = Projectile.position;
            if (Projectile.position.X <= Projectile.oldPosition.X || Projectile.position.X > Projectile.oldPosition.X || Projectile.position.Y <= Projectile.oldPosition.Y || Projectile.position.Y > Projectile.oldPosition.Y)
            {
                Projectile.netUpdate = true;
                owner.netUpdate = true;
            }
            return true;
        }
        public override void AI()
        {
            owner.aiStyle = -1;
            if (owner.active)
            {
                if (followType != FollowID.Replace)
                { 
                    Projectile.timeLeft = 10;
                }
                Projectile.height = owner.height;
            }
            else
            {
                Projectile.active = false;
                return;
            }
            Player player = Main.LocalPlayer;
            owner.direction = player.direction;
            owner.position = player.position;
            owner.velocity = player.velocity;
            owner.knockBackResist = 1f;
            var follower = Main.projectile.Where(t => t.active && t.owner == player.whoAmI && t.type == Type && t.localAI[0] != ownerType).ToArray();
            if (followerID == 0)
            {
                Follow(player);
            }
            else
            {
                Follow(player, follower[followerID - 1]);
            }
        }
        public void Follow(Player player)
        {
            if (followType == FollowID.Replace)
            {
                if (owner.height < player.height)
                { 
                    owner.position = player.position + new Vector2(0, Math.Abs(player.height - owner.height));
                }
                else
                {
                    owner.position = player.position - new Vector2(0, Math.Abs(player.height - owner.height));
                }
                return;
            }
            if (!PlayerNotControlMove(player) || PlayerMoving(player))
            {
                oldVelocity.Add(player.position + new Vector2(rand, Math.Abs(player.height - owner.height)));
                if (!beginMove)
                {
                    if (ArchaeaItem.Elapsed(ref ticks, 60))
                    {
                        ticks = 0;
                        beginMove = true;
                    }
                    else
                    {
                        Projectile.position += ArchaeaNPC.AngleToSpeed(Projectile.AngleTo(oldVelocity[0]), player.moveSpeed);
                    }
                }
            }
            //  Reposition to remove floating when waiting
            if (!PlayerMoving(player))
            {
                while (!Collision.SolidTiles(Projectile.position, owner.width, owner.height + 1))
                {
                    Projectile.position.Y++;
                }
            }
            if (oldVelocity.Count > 0)
            {
                if (beginMove)
                {
                    owner.direction = player.Center.X < owner.Center.X ? -1 : 1;
                    Projectile.velocity = player.velocity;
                    Projectile.position = oldVelocity[0];
                    oldVelocity.RemoveAt(0);
                }
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                beginMove = false;
            }
            if (Projectile.velocity.Y < 0f)
            {
                int d = Dust.NewDust(Projectile.position + new Vector2(0, Projectile.height - 2), 1, 1, DustID.Torch, Scale: 2f);
                Main.dust[d].noLight = false;
                Main.dust[d].noGravity = true;
                int d2 = Dust.NewDust(Projectile.position + new Vector2(Projectile.width - 16, Projectile.height - 2), 1, 1, DustID.Torch, Scale: 2f);
                Main.dust[d2].noLight = false;
                Main.dust[d2].noGravity = true;
                if (ArchaeaItem.Elapsed(ref ticks2, 5))
                {
                    SoundEngine.PlaySound(SoundID.Item13, Projectile.Center);
                    ticks2 = 0;
                }
            }
        }
        public void Follow(Player player, Projectile npc)
        {
            if (FollowerMoving(npc))
            {
                oldVelocity2.Add(npc.position + new Vector2(rand, Math.Abs(npc.height - owner.height)));
                if (!beginMove)
                {
                    if (ArchaeaItem.Elapsed(ref ticks, 60))
                    {
                        ticks = 0;
                        beginMove = true;
                    }
                    else
                    {
                        Projectile.position += ArchaeaNPC.AngleToSpeed(Projectile.AngleTo(oldVelocity2[0]), player.moveSpeed);
                    }
                }
            }
            //  Reposition to remove floating when waiting
            if (!FollowerMoving(npc))
            {
                while (!Collision.SolidTiles(Projectile.position, owner.width, owner.height + 1))
                {
                    Projectile.position.Y++;
                }
            }
            if (oldVelocity2.Count > 0)
            {
                if (beginMove)
                {
                    owner.direction = npc.Center.X < owner.Center.X ? -1 : 1;
                    Projectile.velocity = npc.velocity;
                    Projectile.position = oldVelocity2[0];
                    oldVelocity2.RemoveAt(0);
                }
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                beginMove = false;
            }
            if (Projectile.velocity.Y < 0f)
            {
                int d = Dust.NewDust(Projectile.position + new Vector2(0, Projectile.height - 2), 1, 1, DustID.Torch, Scale: 2f);
                Main.dust[d].noLight = false;
                Main.dust[d].noGravity = true;
                int d2 = Dust.NewDust(Projectile.position + new Vector2(Projectile.width - 16, Projectile.height - 2), 1, 1, DustID.Torch, Scale: 2f);
                Main.dust[d2].noLight = false;
                Main.dust[d2].noGravity = true;
                if (ArchaeaItem.Elapsed(ref ticks2, 5))
                {
                    SoundEngine.PlaySound(SoundID.Item13, Projectile.Center);
                    ticks2 = 0;
                }
            }
        }
    }
}
