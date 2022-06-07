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

namespace ArchaeaMod.NPCs.Legacy
{
    public class Sky_ground : ModNPC
    {
        public override bool IsLoadingEnabled(Mod mod) => false;

        public bool Hurt()
        {
            return NPC.life < NPC.lifeMax && NPC.life > 0 && oldLife != NPC.life;
        }
        private bool findClosest;
        public virtual bool aggroChance(int chance)
        {
            return Main.rand.Next(chance) == 0;
        }
        public bool canSee()
        {
            Vector2 line;
            for (float k = 0; k < NPC.Distance(target().position); k += 0.5f)
            {
                line = NPC.Center + ArchaeaNPC.AngleToSpeed(ArchaeaNPC.AngleTo(NPC, target()), k);
                int i = (int)line.X / 16;
                int j = (int)line.Y / 16;
                Tile tile = Main.tile[i, j];
                if (tile.HasTile && Main.tileSolid[tile.TileType])
                    return false;
            }
            return true;
        }
        public int oldLife;
        public float range = 400f;
        public virtual Rectangle Range
        {
            get { return new Rectangle((int)NPC.position.X - 200, (int)NPC.position.Y - 150, 400, 300); }
        }
        internal Pattern pattern = Pattern.JustSpawned;
        public Player target()
        {
            Player player = ArchaeaNPC.FindClosest(NPC, findClosest);
            if (player != null && NPC.Distance(player.position) < range)
            {
                NPC.target = player.whoAmI;
                return player;
            }
            else return Main.player[NPC.target];
        }
        public override bool PreAI()
        {
            if (!target().active || target().dead)
            {
                pattern = Pattern.Idle;
                return false;
            }
            switch (pattern)
            {
                case Pattern.JustSpawned:
                    if (JustSpawned())
                    {
                        oldLife = NPC.life;
                        if (aggroChance(80))
                            pattern = Pattern.Attack;
                        goto case Pattern.Idle;
                    }
                    return false;
                case Pattern.Idle:
                    pattern = Pattern.Idle;
                    DefaultAI();
                    if (Hurt())
                    {
                        findClosest = true;
                        goto case Pattern.Attack;
                    }
                    if (target().Distance(NPC.position) < range * 1.2f)
                        goto case Pattern.Active;
                    return true;
                case Pattern.Active:
                    pattern = Pattern.Active;
                    if (Hurt() || aggroChance(1600))
                        goto case Pattern.Attack;
                    Active();
                    return true;
                case Pattern.Attack:
                    pattern = Pattern.Attack;
                    AttackPattern();
                    return true;
            }
            if (!target().active || target().dead || NPC.Distance(target().position) > range)
                DefaultAI();
            if (NPC.Distance(target().position) < range && pattern != Pattern.Attack)
                if (NPC.alpha != 0)
                    NPC.alpha -= 5;
            return false;
        }
        public virtual bool JustSpawned()
        {
            attackRate = 180;
            moveRate = 150;
            range = 300f;
            return true;
        }
        private int ai;
        protected bool DefaultAI()
        {
            reData = true;
            switch (ai)
            {
                case 0:
                    ai = 0;
                    move = Vector2.Zero;
                    if (target() != null && target().Distance(NPC.position) < range * 3f)
                        move = ArchaeaNPC.FastMove(target());
                    if (move != Vector2.Zero)
                        goto case 1;
                    return false;
                case 1:
                    ai = 1;
                    if (NPC.alpha < 250)
                        NPC.alpha += 25;
                    else
                    {
                        NPC.position = move;
                        goto case 2;
                    }
                    return true;
                case 2:
                    ai = 2;
                    if (NPC.alpha > 0)
                        NPC.alpha -= 25;
                    else goto case 0;
                    return true;
            }
            return false;
        }

        public bool reData = true;
        public int type = -1;
        public int index;
        public int rand = 0;
        public int moveRate;
        public float oldX;
        public float amount;
        public double degree;
        public Vector2 idle;
        public Vector2[] points = new Vector2[5];
        protected void Active()
        {
            if (reData)
            {
                oldX = NPC.position.X;
                idle = NPC.position;
                reData = false;
                type = -1;
            }
            switch (type)
            {
                case -1:
                    amount = BeginInRange();
                    if (amount == 1f)
                    {
                        type = 0;
                        index = 0;
                        goto case 0;
                    }
                    break;
                case 0:
                    if (ActiveMovement())
                    {
                        index = 0;
                        type = 1;
                        goto case 1;
                    }
                    break;
                case 1:
                    if (PostActive())
                    {
                        type = 0;
                        goto case 0;
                    }
                    break;
            }
        }
        public virtual float BeginInRange()
        {
            return 1f;
        }
        public virtual bool ActiveMovement()
        {
            return true;
        }
        public virtual bool PostActive()
        {
            return true;
        }

        public bool preAttack;
        public const int
            Teleport = 0,
            Attack_ = 1,
            Movement = 2,
            FadeOut = 5,
            FadeIn = 6;
        public int attack = -1;
        public int attacks;
        public int attackRate;
        public int totalAttacks = 2;
        public Vector2 move;
        protected void AttackPattern()
        {
            if (preAttack)
            {
                move = Vector2.Zero;
                attacks = 0;
                attack = -1;
                preAttack = false;
                reData = true;
            }
            if (attack != FadeOut)
                NPC.velocity = Vector2.Zero;
            AttackMovement();
            switch (attack)
            {
                case -1:
                    goto case Teleport;
                case Attack_:
                    if (index++ % attackRate == 0 && index != 0)
                    {
                        Attack();
                        attacks++;
                        if (attacks > totalAttacks)
                        {
                            pattern = Pattern.Active;
                            preAttack = true;
                            break;
                        }
                        goto case Teleport;
                    }
                    break;
                case Teleport:
                    attack = Teleport;
                    if (BeginTeleport())
                    {
                        Vector2 v = ArchaeaNPC.FastMove(target());
                        if (v != Vector2.Zero)
                        {
                            move = v;
                            goto case FadeOut;
                        }
                    }
                    break;
                case FadeOut:
                    attack = FadeOut;
                    if (NPC.alpha < 200)
                        NPC.alpha += 5;
                    else
                    {
                        if (NPC.Distance(target().position) < 800f)
                            NPC.position = move;
                        goto case FadeIn;
                    }
                    break;
                case FadeIn:
                    attack = FadeIn;
                    if (NPC.alpha > 0)
                        NPC.alpha -= 5;
                    else goto case Attack_;
                    break;
            }
        }
        public virtual void AttackMovement()
        {
        }
        public virtual void Attack()
        {
        }
        public virtual bool BeginTeleport()
        {
            return true;
        }
    }
}
