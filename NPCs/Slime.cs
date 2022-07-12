using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class Slime : ModNPC
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 48;
            NPC.height = 32;
            NPC.lifeMax = 50;
            NPC.defense = 6;
            NPC.damage = 10;
            NPC.value = 500;
        }

        public bool flip;
        public bool moveX;
        public bool inRange;
        public virtual bool Hurt()
        {
            return NPC.life < NPC.lifeMax && NPC.life > 0 && oldLife != NPC.life;
        }
        public bool FacingWall()
        {
            if (Collision.SolidCollision(NPC.position + new Vector2(-8f, 0f), NPC.width + 16, NPC.height))
                return true;
            else return false;
        }
        public int timer
        {
            get { return (int)NPC.ai[0]; }
            set { NPC.ai[0] = value; }
        }
        public int counter;
        public int oldLife;
        public const int maxAggro = 2;
        public const int interval = 600;
        public virtual float jumpHeight(bool facingWall = false, bool inRange = false)
        {
            if (facingWall)
                return 2.5f * multi;
            if (!inRange)
                return 1.8f * multi;
            else return 2.2f * multi;
        }
        public virtual float speedX()
        {
            return 2f * multi;
        }
        public float multi
        {
            get { return Main.rand.NextFloat(2f, 4f); }
        }
        public float velX;
        public Pattern pattern = Pattern.JustSpawned;
        public Player target;
        public bool SlimeAI()
        {
            if (timer++ > interval)
                timer = 0;
            
            if (NPC.velocity.Y == 0f && !NPC.wet && Collision.SolidCollision(NPC.position, NPC.width, NPC.height + 8))
            { 
                NPC.velocity = Vector2.Zero;
                if (Main.tile[(int)NPC.position.X / 16, (int)(NPC.position.Y + NPC.height - 15) / 16 + 1].TileType == TileID.Platforms)
                    NPC.velocity.X = 0f;
            }
            target = ArchaeaNPC.FindClosest(NPC, true);
            if (NPC.wet)
            {
                NPC.velocity.Y -= 0.3f;
                ArchaeaNPC.VelClampY(NPC, 0f, 2f);
            }
            if (target == null)
            {
                DefaultActions();
                return false;
            }
            inRange = ArchaeaNPC.WithinRange(target.position, ArchaeaNPC.defaultBounds(NPC));
            switch (pattern)
            {
                case Pattern.JustSpawned:
                    if (JustSpawned())
                        goto case Pattern.Idle;
                    return false;
                case Pattern.Idle:
                    pattern = Pattern.Idle;
                    if (inRange)
                        goto case Pattern.Active;
                    DefaultActions(150, flip);
                    return true;
                case Pattern.Active:
                    if (inRange)
                    {
                        if (Hurt())
                            goto case Pattern.Attack;
                    }
                    else if (timer % interval / 4 == 0)
                        counter++;
                    if (counter > maxAggro)
                    {
                        counter = 0;
                        goto case Pattern.Idle;
                    }
                    Active();
                    return true;
                case Pattern.Attack:
                    Attack();
                    return true;
                default:
                    return false;
            }
        }
        public virtual bool JustSpawned()
        {
            return true;
        }
        public virtual void DefaultActions(int interval = 180, bool moveX = false)
        {
        }
        public virtual void Active(int interval = 120)
        {
        }
        public virtual void Attack()
        {
        }
        public virtual void SlimeJump(float speedY, bool horizontal = false, float speedX = 0f, bool direction = true)
        {
        }
        public void FadeTo(int alpha, bool fadeOut = true)
        {
            if (fadeOut)
            {
                if (NPC.alpha < alpha)
                    NPC.alpha++;
            }
            else if (NPC.alpha > 0)
                NPC.alpha--;
        }
        public virtual void SyncNPC()
        {
        }
    }
}
