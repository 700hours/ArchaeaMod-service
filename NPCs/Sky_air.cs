using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArchaeaMod.NPCs
{
    public class Sky_air : ModNPC
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public int timer
        {
            get { return (int)NPC.ai[0]; }
            set { NPC.ai[0] = value; }
        }
        private bool firstTarget = true;
        public Player target()
        {
            Player player = ArchaeaNPC.FindClosest(NPC, firstTarget);
            firstTarget = false;
            if (player != null && player.active && !player.dead)
            {
                NPC.target = player.whoAmI;
                return player;
            }
            else return Main.player[NPC.target];
        }
        private int oldLife;
        public bool Hurt()
        {
            bool hurt = NPC.life < NPC.lifeMax && NPC.life > 0 && oldLife != NPC.life;
            oldLife = NPC.life;
            return hurt;
        }
        private bool init;
        private float upperPoint;
        private float oldX;
        public float amount;
        public double degree;
        private Vector2 idle;
        private Vector2 upper;
        public virtual bool PreSkyAI()
        {
            if (!init && JustSpawned())
            {
                oldX = NPC.position.X;
                upperPoint = NPC.position.Y - 50f;
                idle = NPC.position;
                upper = new Vector2(oldX, upperPoint);
                init = true;
            }
            return init;
        }
        private bool fade;
        private bool attack = true;
        private bool findNewTarget;
        private Vector2 move;
        internal void SkyAI()
        {
            if (timer++ > 900)
            {
                SyncNPC(true);
                timer = 0;
            }
            if (timer % 300 == 0 && timer != 0)
                fade = true;
            findNewTarget = target() == null || !target().active || target().dead;
            if (!findNewTarget)
                MaintainProximity(300f);
            if (fade)
            {
                if (NPC.alpha < 255 && PreFadeOut())
                    NPC.alpha += 255 / 60;
                else if (timer % 90 == 0)
                {
                    if (!findNewTarget)
                    {
                        NPC.position = ArchaeaNPC.FindAny(NPC, target(), false, 300);
                        SyncNPC();
                    }
                    fade = false;
                }
            }
            else
            {
                if (NPC.alpha > 0)
                    NPC.alpha -= 255 / 60;
            }
            if (!fade && NPC.alpha == 0)
            {
                if (timer % 150 == 0)
                {
                    if (!findNewTarget)
                    {
                        if (!attack)
                        {
                            move = ArchaeaNPC.FindAny(NPC, target(), false, 300);
                            SyncNPC(move.X, move.Y);
                        }
                        else if (PreAttack())
                        {
                            move = target().Center;
                            SyncNPC(move.X, move.Y);
                        }
                    }
                }
                if (move != Vector2.Zero && (NPC.position.X > move.X || NPC.position.X < move.X || NPC.position.Y > move.Y || NPC.position.Y < move.Y))
                {
                    float angle = NPC.AngleTo(move);
                    float cos = (float)(0.25f * Math.Cos(angle));
                    float sine = (float)(0.25f * Math.Sin(angle));
                    NPC.velocity += new Vector2(cos, sine);
                    ArchaeaNPC.VelocityClamp(ref NPC.velocity, -3f, 3f);
                    if (NPC.velocity.X < 0f && NPC.oldVelocity.X >= 0f || NPC.velocity.X > 0f && NPC.oldVelocity.X <= 0f || NPC.velocity.Y < 0f && NPC.oldVelocity.Y >= 0f || NPC.velocity.Y > 0f && NPC.oldVelocity.Y <= 0f)
                        SyncNPC();
                }
            }
        }

        private void MaintainProximity(float range)
        {
            if (!findNewTarget && !attack && NPC.Distance(target().Center) > range)
            {
                move = ArchaeaNPC.FindAny(NPC, target(), false, 200);
                SyncNPC(move.X, move.Y);
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return NPC.alpha == 0;
        }

        public virtual bool JustSpawned()
        {
            return true;
        }
        public virtual bool PreAttack()
        {
            return true;
        }
        public virtual bool PreFadeOut()
        {
            return true;
        }
        public virtual bool PostTeleport()
        {
            return true;
        }

        private void SyncNPC()
        {
            if (Main.netMode == 2)
                NPC.netUpdate = true;
        }
        private void SyncNPC(float x, float y)
        {
            if (Main.netMode == 2)
            {
                NPC.netUpdate = true;
                move = new Vector2(x, y);
            }
        }
        private void SyncNPC(bool attack)
        {
            if (Main.netMode == 2)
                NPC.netUpdate = true;
            this.attack = attack;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(move);
            writer.Write(attack);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            move = reader.ReadVector2();
            attack = reader.ReadBoolean();
        }
    }
}
