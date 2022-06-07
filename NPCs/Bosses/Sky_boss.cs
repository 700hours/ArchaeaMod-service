using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs.Bosses
{
    public class Sky_boss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Necrosis");
        }
        public override void SetDefaults()
        {
            NPC.width = 176;
            NPC.height = 192;
            NPC.lifeMax = 150000;
            NPC.defense = 10;
            NPC.damage = 20;
            NPC.value = 45000;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.alpha = 255;
        }

        public bool Hurt()
        {
            bool hurt = NPC.life < NPC.lifeMax && NPC.life > 0 && oldLife != NPC.life;
            oldLife = NPC.life;
            return hurt;
        }
        private int timer
        {
            get { return (int)NPC.ai[0]; }
            set { NPC.ai[0] = value; }
        }
        private int npcCounter
        {
            get { return (int)NPC.ai[1]; }
            set { NPC.ai[1] = value; }
        }
        private int counter
        {
            get { return (int)NPC.ai[2]; }
            set { NPC.ai[2] = value; }
        }
        private bool fade;
        private bool firstTarget = true;
        private int oldLife;
        private Vector2 move;
        private Player target()
        {
            Player player = ArchaeaNPC.FindClosest(NPC, true);
            if (player != null && player.active && !player.dead)
            {
                NPC.target = player.whoAmI;
                return player;
            }
            return Main.player[NPC.target];
        }
        private bool init;
        private bool attack;
        private int index;
        private int sendIndex;
        private float angle;
        private Projectile[] orbs = new Projectile[7];
        private Projectile[] flames = new Projectile[6];
        public override bool PreAI()
        {
            if (!init)
                init = true;
            return init;
        }
        public override void AI()
        {
            NPC.spriteDirection = 1;
            if (timer++ > 900)
            {
                npcCounter++;
                timer = 0;
            }
            if (timer % 600 == 0 && timer != 0)
            {
                move = Vector2.Zero;
                do
                {
                    move = ArchaeaNPC.FindEmptyRegion(target(), ArchaeaNPC.defaultBounds(target()));
                } while (move == Vector2.Zero);
                SyncNPC(move.X, move.Y);
                fade = true;
            }
            if (timer % 300 == 0 && timer != 0)
            {
                if (timer != 600)
                {
                    move = target().Center;
                    SyncNPC(true, true);
                }
            }
            if (npcCounter > 1)
            {
                Vector2 newPosition = ArchaeaNPC.FindAny(NPC, target(), false, 300);
                int n = NPC.NewNPC(NPC.GetSource_FromAI(), (int)newPosition.X, (int)newPosition.Y, ModContent.NPCType<Sky_1>(), 0, 0f, 0f, 0f, 0f, NPC.target);
                if (Main.netMode == 2)
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
                npcCounter = 0;
                SyncNPC();
            }
            if (fade)
            {
                NPC.velocity = Vector2.Zero;
                if (NPC.alpha < 255)
                {
                    NPC.scale -= 1f / 90f;
                    NPC.alpha += 255 / 15;
                }
                else 
                {
                    NPC.position = move;
                    if (FlameBurst())
                    {
                        NPC.scale = 1f;
                        fade = false;
                    }
                }
            }
            else
            {
                if (NPC.alpha > 0)
                    NPC.alpha -= 255 / 60;
                if (timer < 600)
                {
                    if (timer % 150 == 0)
                        move = ArchaeaNPC.FindAny(NPC, target(), false);
                    float angle = NPC.AngleTo(move);
                    float cos = (float)(0.2f * Math.Cos(angle));
                    float sine = (float)(0.2f * Math.Sin(angle));
                    NPC.velocity += new Vector2(cos, sine);
                    ArchaeaNPC.VelocityClamp(ref NPC.velocity, -4f, 4f);
                }
            }
            if (attack)
            {
                if (counter++ % 90 == 0)
                {
                    angle += (float)Math.PI / 3f;
                    float cos = (float)(NPC.Center.X + NPC.width * 3f * Math.Cos(angle));
                    float sine = (float)(NPC.Center.Y + NPC.height * 3f * Math.Sin(angle));
                    int t = Projectile.NewProjectile(Projectile.GetSource_None(), new Vector2(cos, sine), Vector2.Zero, ModContent.ProjectileType<Orb>(), 12, 2f, 255, 0f, target().whoAmI);
                    Main.projectile[t].whoAmI = t;
                    if (Main.netMode == 2)
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, t);
                    index++;
                }
                if (index == 6)
                    attack = false;
            }
            else
            {
                index = 0;
                angle = ArchaeaNPC.RandAngle();
                SyncNPC(false, false);
            }
            
            if (fade && NPC.position != NPC.oldPosition || NPC.velocity.X < 0f && NPC.oldVelocity.X >= 0f || NPC.velocity.X > 0f && NPC.oldVelocity.X <= 0f || NPC.velocity.Y < 0f && NPC.oldVelocity.Y >= 0f || NPC.velocity.Y > 0f && NPC.oldVelocity.Y <= 0f)
                SyncNPC();
        }
        public override void BossHeadSlot(ref int index)
        {
            index = NPCHeadLoader.GetBossHeadSlot(ArchaeaMain.skyHead);
        }

        private bool FlameBurst()
        {
            float angle = ArchaeaNPC.RandAngle();
            for (int i = 0; i < flames.Length; i++)
            {
                flames[i] = Projectile.NewProjectileDirect(Projectile.GetSource_None(), NPC.Center, ArchaeaNPC.AngleToSpeed(angle), ModContent.ProjectileType<Flame>(), 20, 3f, 255, 1f, NPC.target);
                angle += (float)Math.PI / 3f;
                if (Main.netMode == 2)
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, flames[i].whoAmI);
            }
            return true;
        }

        Vector2 lastHit = Vector2.Zero;
        public override void HitEffect(int hitDirection, double damage)
        {
            lastHit = NPC.Center;
        }
        public override void OnKill()
        {
            Item.NewItem(Item.GetSource_NaturalSpawn(), lastHit, ModContent.ItemType<Items.n_Staff>());
        }

        private void SyncNPC()
        {
            if (Main.netMode == 2)
                NPC.netUpdate = true;
        }
        private void SyncNPC(float x, float y)
        {
            move = new Vector2(x, y);
            if (Main.netMode == 2)
                NPC.netUpdate = true;
        }
        private void SyncNPC(bool attack, bool immortal)
        {
            this.attack = attack;
            NPC.immortal = immortal;
            if (Main.netMode == 2)
                NPC.netUpdate = true;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(move);
            writer.Write(attack);
            writer.Write(NPC.immortal);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            move = reader.ReadVector2();
            attack = reader.ReadBoolean();
            NPC.immortal = reader.ReadBoolean();
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Bosses/Sky_boss").Value, NPC.Hitbox, Lighting.GetColor((int)NPC.Center.X, (int)NPC.Center.Y, drawColor));
        }
    }

    internal class Orb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orb");
            Main.projFrames[Projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.damage = 10;
            Projectile.knockBack = 2f;
            Projectile.timeLeft = 360;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        private int timer
        {
            get { return (int)Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
        private Player target
        {
            get { return Main.player[(int)Projectile.ai[1]]; }
        }
        private NPC boss
        {
            get { return Main.npc[(int)Projectile.localAI[0]]; }
        }
        private bool init;
        public override void AI()
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 255 / 60;
            else Projectile.alpha = 0;
            float maxSpeed = Math.Max(((boss.lifeMax + 1 - boss.life) / boss.lifeMax) * 5f, 2f);
            float angle;
            if (timer++ > 90)
            {
                if (target.active && !target.dead)
                    angle = Projectile.AngleTo(target.Center);
                else angle = Projectile.AngleFrom(target.Center);
                Projectile.velocity += ArchaeaNPC.AngleToSpeed(angle, 0.5f);
                ArchaeaNPC.VelocityClamp(ref Projectile.velocity, maxSpeed * -1, maxSpeed);
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.netMode == 2 && (Projectile.velocity.X < 0f && Projectile.oldVelocity.X >= 0f || Projectile.velocity.X > 0f && Projectile.oldVelocity.X <= 0f || Projectile.velocity.Y < 0f && Projectile.oldVelocity.Y >= 0f || Projectile.velocity.Y > 0f && Projectile.oldVelocity.Y <= 0f))
                Projectile.netUpdate = true;
            if (Projectile.scale == 1f)
            {
                for (int k = 0; k < 4; k++)
                {
                    int t = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 0, default(Color), 2f);
                    Main.dust[t].noGravity = true;
                }
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter == 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 6)
            {
                Projectile.frame = 0;
            }
        }
        public override void Kill(int timeLeft)
        {
            ArchaeaNPC.DustSpread(Projectile.position, Projectile.width, Projectile.height, 6, 6, 2f);
        }
    }


    internal class Flame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flame");
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.damage = 20;
            Projectile.knockBack = 2f;
            Projectile.timeLeft = 450;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        private int i;
        private int j;
        private Player player
        {
            get { return Main.player[(int)Projectile.ai[1]]; }
        }
        public override void AI()
        {
            if (Projectile.Distance(player.Center) > 2048)
                Projectile.active = false;
            if (Projectile.ai[0] != 1f)
                Projectile.timeLeft = 90;
            i = (int)Projectile.position.X / 16;
            j = (int)Projectile.position.Y / 16;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (TileLeft() || TileRight())
                Projectile.velocity.X *= -1;
            if (TileTop() || TileBottom())
                Projectile.velocity.Y *= -1;
            for (int k = 0; k < 3; k++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
            if (Main.netMode == 2 && (Projectile.velocity.X < 0f && Projectile.oldVelocity.X >= 0f || Projectile.velocity.X > 0f && Projectile.oldVelocity.X <= 0f || Projectile.velocity.Y < 0f && Projectile.oldVelocity.Y >= 0f || Projectile.velocity.Y > 0f && Projectile.oldVelocity.Y <= 0f))
                Projectile.netUpdate = true;
            Projectile.velocity.Y += 0.0917f;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.ShadowFlame, 300);
            if (Main.netMode == 2)
                NetMessage.SendData(MessageID.AddPlayerBuff, target.whoAmI, -1, null, BuffID.ShadowFlame, 300);
        }
        public override bool PreKill(int timeLeft)
        {
            if (Projectile.scale > 0.1f)
            {
                Projectile.scale -= 1f / 60f;
                return false;
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            ArchaeaNPC.DustSpread(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 10, 2f);
        }
        private bool TileLeft()
        {
            if (Main.tile[i - 1, j].HasTile && Main.tileSolid[Main.tile[i - 1, j].TileType])
            {
                Projectile.position.X += 18f;
                return true;
            }
            return false;
        }
        private bool TileRight()
        {
            if (Main.tile[i + 1, j].HasTile && Main.tileSolid[Main.tile[i + 1, j].TileType])
            {
                Projectile.position.X -= 18f;
                return true;
            }
            return false;
        }
        private bool TileTop()
        {
            if (Main.tile[i, j - 1].HasTile && Main.tileSolid[Main.tile[i, j - 1].TileType])
            {
                Projectile.position.Y += 18f;
                return true;
            }
            return false;
        }
        private bool TileBottom()
        {
            if (Main.tile[i, j + 1].HasTile && Main.tileSolid[Main.tile[i, j + 1].TileType])
            {
                Projectile.position.X -= 18f;
                return true;
            }
            return false;
        }
    }

    internal class Energy
    {
        private int time;
        private int elapsed = 30;
        public int total;
        public int max = 6;
        public float radius;
        private float oldRadius;
        public float rotation;
        private float scale;
        private float variance;
        private float rotate;
        private static float r;
        public Vector2 center;
        private Color color;
        private Dust[] dust = new Dust[400];
        public NPC npc;
        public Energy(NPC npc, float radius, float rotation)
        {
            this.npc = npc;
            this.radius = radius;
            oldRadius = radius;
            this.rotation = rotation;
            color = Main.rand.Next(2) == 0 ? Color.Yellow : Color.Blue;
            scale = Main.rand.NextFloat(1.5f, 4f);
            rotate = r += 0.5f;
        }
        public void Reset()
        {
            variance = 0f;
            radius = oldRadius;
            time = 0;
            total = 0;
        }
        public bool Absorb(float range, Action action)
        {
            variance += Main.rand.NextFloat(0.5f, 3f);
            if (time % elapsed * 5 * rotate == 0)
            {
                center = ArchaeaNPC.AngleBased(npc.Center, rotation + variance, range);
                dust[total] = Dust.NewDustDirect(center, 1, 1, 6, 0f, 0f, 0, color, scale);
                dust[total].noGravity = true;
                total++;
            }
            foreach (Dust d in Main.dust)
                if (d != null)
                {
                    if (Vector2.Distance(d.position - npc.position, Vector2.Zero) < range + 32)
                    {
                        d.velocity = ArchaeaNPC.AngleToSpeed(ArchaeaNPC.AngleTo(d.position, npc.Center), 3f);
                        Target.VelClamp(ref d.velocity, -3f, 3f, out d.velocity);
                    }
                }
            action.Invoke();
            if (range < npc.width || total > elapsed * 12)
            {
                Reset();
                return true;
            }
            return false;
        }
    }
    internal class Target
    {
        private static int time;
        private static int elapsed = 60;
        public static int type;
        private const int
            Melee = 0,
            Range = 1,
            Magic = 2;
        public static float range;
        private static float rotation;
        private static float rotate;
        private static int index;
        public static NPC npc;
        private static Energy[] energy = new Energy[3000];
        public static void BeingAttacked()
        {
            foreach (Player target in Main.player.Where(t => t.Distance(npc.Center) < range))
            {
                if (time++ % elapsed * 2 == 0 && time != 0)
                {
                    if (target != null)
                    {
                        if (npc.Distance(target.Center) < range)
                        {
                            switch (type)
                            {
                                case Melee:
                                    target.Hurt(PlayerDeathReason.ByNPC(npc.whoAmI), 10, 0);
                                    NetMessage.SendData(MessageID.HurtPlayer, -1, -1, null, target.whoAmI);
                                    break;
                                case Range:
                                    target.velocity += ArchaeaNPC.AngleToSpeed(ArchaeaNPC.AngleTo(target.Center, npc.Center), 0.5f);
                                    VelClamp(ref target.velocity, -2f, 2f, out target.velocity);
                                    break;
                                case Magic:
                                    if (target.statMana > 5)
                                    {
                                        target.statMana -= 5;
                                        target.manaRegenDelay = 180;
                                        NetMessage.SendData(MessageID.PlayerMana, -1, -1, null, target.whoAmI, target.statMana);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public static void VelClamp(ref Vector2 input, float min, float max, out Vector2 result)
        {
            if (input.X < min)
                input.X = min;
            if (input.X > max)
                input.X = max;
            if (input.Y < min)
                input.Y = min;
            if (input.Y > max)
                input.Y = max;
            result = input;
        }
    }
}
