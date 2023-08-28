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
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class Caster : ModNPC
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 48;
            NPC.height = 48;
            NPC.lifeMax = 50;
            NPC.defense = 10;
            NPC.damage = 20;
            NPC.value = 5000;
            NPC.alpha = 255;
            NPC.lavaImmune = true;
        }
        
        public bool hasAttacked;
        public int timer
        {
            get { return (int)NPC.ai[0]; }
            set { NPC.ai[0] = value; }
        }
        public int elapse = 180;
        public int attacks
        {
            get { return (int)NPC.ai[1]; }
            set { NPC.ai[1] = value; }
        }
        public int pattern
        {
            get { return (int)NPC.ai[2]; }
            set { NPC.ai[2] = value; }
        }
        public int maxAttacks
        {
            get { return Main.rand.Next(3, 6); }
        }
        public int DustType;
        public Vector2 move;
        public Player npcTarget
        {
            get { return Main.player[NPC.target]; }
        }
        public virtual Player nearbyPlayer()
        {
            Player player = ArchaeaNPC.FindClosest(NPC, false, 800);
            if (player != null && player.active && !player.dead)
            {
                NPC.target = player.whoAmI;
                return player;
            }
            else return npcTarget;
        }
        private bool init;
        private int oldPattern;
        private const int
            Attacking = 2;

        public override bool PreAI()
        {
            return SinglePlayerAI();
        }
        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            knockback = 0f;
        }
        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            knockback = 0f;
        }
        public override void AI()
        {
        }
        public virtual bool JustSpawned()
        {
            return true;
        }
        public virtual void Teleport()
        {

        }
        public virtual bool PreAttack()
        {
            return true;
        }
        public virtual void Attack()
        {
        }
        private void SyncNPC(float x, float y)
        {
            if (Main.netMode == 2)
            {
                NPC.position = new Vector2(x, y);
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI, NPC.position.X, NPC.position.Y);
                NPC.netUpdate = true;
            }
            if (Main.netMode == 0)
            {
                NPC.position = new Vector2(x, y);
            }
        }

        private bool SinglePlayerAI()
        {
            if (timer++ > elapse)
                timer = 0;
            ArchaeaNPC.SlowDown(ref NPC.velocity);
            switch (pattern)
            {
                case PatternID.JustSpawned:
                    NPC.target = ArchaeaNPC.FindClosest(NPC, true).whoAmI;
                    if (JustSpawned())
                        goto case PatternID.FadeIn;
                    break;
                case PatternID.FadeIn:
                    pattern = PatternID.FadeIn;
                    if (NPC.alpha > 0)
                    {
                        NPC.alpha -= 5;
                        break;
                    }
                    else goto case PatternID.Idle;
                case PatternID.FadeOut:
                    pattern = PatternID.FadeOut;
                    if (NPC.alpha < 255)
                    {
                        NPC.immortal = true;
                        NPC.alpha += 5;
                        break;
                    }
                    goto case PatternID.Teleport;
                case PatternID.Teleport:
                    pattern = PatternID.Teleport;
                    move = ArchaeaNPC.FindAny(NPC, npcTarget);
                    if (move != Vector2.Zero)
                    {
                        SyncNPC(move.X, move.Y);
                        Teleport();
                        hasAttacked = false;
                        goto case PatternID.FadeIn;
                    }
                    break;
                case PatternID.Idle:
                    pattern = PatternID.Idle;
                    NPC.immortal = false;
                    if (timer % elapse == 0 && Main.rand.Next(3) == 0)
                    {
                        if (!hasAttacked)
                        {
                            hasAttacked = true;
                            goto case PatternID.Attack;
                        }
                        else goto case PatternID.FadeOut;
                    }
                    return false;
                case PatternID.Attack:
                    pattern = PatternID.Attack;
                    if (PreAttack())
                    {
                        if (attacks > 0)
                            Attack();
                        attacks++;
                    }
                    if (attacks > maxAttacks)
                    {
                        pattern = PatternID.Idle;
                        attacks = 0;
                    }
                    return true;
            }
            if (oldPattern != pattern)
                SyncNPC(NPC.position.X, NPC.position.Y);
            oldPattern = pattern;
            return false;
        }
        private bool MPAI()
        {
            if (timer++ > elapse)
            {
                timer = 0;
                if (pattern < Attacking)
                    pattern++;
            }

            if (!init)
                init = JustSpawned();
            if (pattern == Attacking)
            {
                if (attacks < maxAttacks)
                {
                    if (timer % elapse == 0 && timer != 0)
                    {
                        attacks++;
                        Attack();
                    }
                }
                else
                {
                    if (NPC.alpha < 255)
                        NPC.alpha += 5;
                    else
                    {
                        move = ArchaeaNPC.FindAny(NPC, Main.player[NPC.target], true, 300);
                        if (move != Vector2.Zero)
                        {
                            pattern++;
                            Teleport();
                            SyncNPC(move.X, move.Y);
                        }
                    }
                }
                return true;
            }
            if (NPC.alpha > 0)
                NPC.alpha -= 5;
            else pattern = 0;
            return false;
        }
    }
}
