using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs.Bosses
{
    public class Magnoliac_head : Digger
    {
        public override bool IsLoadingEnabled(Mod mod) => true;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno");
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 32;
            NPC.height = 32;
            NPC.lifeMax = 2000;
            NPC.defense = 10;
            NPC.knockBackResist = 0.5f;
            NPC.damage = 20;
            NPC.value = 45000;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 10f;
            //bossBag = ModContent.ItemType<Merged.Items.magno_treasurebag>();
            if (!Main.dedServ) {
                Music = MusicLoader.GetMusicSlot(Mod, "ArchaeaMod/Sounds/Muics/The_Undying_Flare");
            }
        }
        public override int maxParts
        {
            get { return 45; }
        }
        private int cycle
        {
            get { return (int)NPC.ai[1]; }
            set { NPC.ai[1] = value; }
        }
        private const int spawnMinions = 30;
        private int ai = -1;
        public override bool PreAI()
        {
            switch (ai)
            {
                case -1:
                    bodyType = ModContent.NPCType<Magnoliac_body>();
                    tailType = ModContent.NPCType<Magnoliac_tail>();
                    NPC.lifeMax = maxParts / 2 * NPC.life;
                    NPC.life = NPC.lifeMax;
                    goto case 0;
                case 0:
                    ai = 0;
                    return true;
            }
            return false;
        }
        public override void AI()
        {
            if (timer % 60 == 0 && timer != 0)
                cycle++;
            WormAI();
        }
        public override bool PreAttack()
        {
            bool patch = true;
            if (cycle == spawnMinions && !patch)
            {
                for (int i = 0; i < Math.Min((NPC.life * -1 + NPC.lifeMax) * 0.0001d, 5); i++)
                {
                    int n = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<Merged.NPCs.Copycat_head>());
                    Main.npc[n].whoAmI = n;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
                }
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                cycle = 0;
            }
            return true;
        }

        public override void Digging()
        {
            if (projs == null || projCenter == null || attack == null)
                return;
            attack.Update(NPC, target());
            for (int j = 0; j < projs[1].Length; j++)
            {
                for (int i = 0; i < max; i++)
                {
                    projs[i][j].Stationary(j, NPC.width);
                    if (timer % maxTime / 2 == 0 && timer != 0)
                    {
                        Vector2 v = ArchaeaNPC.FindEmptyRegion(target(), ArchaeaNPC.defaultBounds(target()));
                        if (v != Vector2.Zero)
                            projs[i][j].position = v;
                    }
                }
            }
        }
        private bool start = true;
        private int index;
        private int max = 5;
        private float rotate;
        private Vector2[] projCenter;
        private Attack[][] projs;
        private Attack attack;
        public override void Attacking()
        {
            if (timer % maxTime == 0 && timer != 0)
            {
                if (projs != null)
                {
                    for (int j = 0; j < projs.GetLength(0); j++)
                        foreach (Attack sets in projs[j])
                            sets.proj.active = false;
                }
                attack = new Attack(Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), NPC.Center, Vector2.Zero, ProjectileID.Fireball, 20, 4f));
                attack.proj.tileCollide = false;
                attack.proj.ignoreWater = true;
                max = Math.Max(8 / NPC.life, 3);
                projCenter = new Vector2[max];
                projs = new Attack[max][];
                for (int i = 0; i < projs.GetLength(0); i++)
                {
                    projs[i] = new Attack[6];
                    index = 0;
                    for (double r = 0d; r < Math.PI * 2d; r += Math.PI / 3d)
                    {
                        if (index < 6)
                        {
                            projs[i][index] = new Attack(Projectile.NewProjectileDirect(Projectile.GetSource_None(), ArchaeaNPC.AngleBased(NPC.Center, (float)r, NPC.width * 4f), Vector2.Zero, ProjectileID.Fireball, 20, 4f), (float)r);
                            projs[i][index].proj.timeLeft = maxTime;
                            projs[i][index].proj.rotation = (float)r;
                            projs[i][index].proj.tileCollide = false;
                            projs[i][index].proj.ignoreWater = true;
                            Vector2 v = Vector2.Zero;
                            do
                            {
                                v = ArchaeaNPC.FindEmptyRegion(target(), ArchaeaNPC.defaultBounds(target()));
                                projs[i][index].position = v;
                            } while (v == Vector2.Zero);
                            index++;
                        }
                    }
                }
                start = false;
                index = 0;
            }
        }
        public override void BossHeadSlot(ref int index)
        {
            index = NPCHeadLoader.GetBossHeadSlot(ArchaeaMain.magnoHead);
        }
        Vector2 lastHit = Vector2.Zero;
        public override void HitEffect(int hitDirection, double damage)
        {
            lastHit = NPC.Center;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            ModContent.GetInstance<ArchaeaWorld>().downedMagno = true;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Merged.Items.magno_treasurebag>()));
        }
    }

    public class Attack
    {
        public static float variance;
        public float rotation;
        public Projectile proj;
        private Vector2 focus;
        public Vector2 position;
        public Attack(Projectile proj)
        {
            this.proj = proj;
            position = proj.position;
        }
        public Attack(Projectile proj, float rotation)
        {
            this.proj = proj;
            this.rotation = rotation + (variance += 0.2f);
            position = proj.position;
            if (Main.netMode == 2)
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
        }
        public void Stationary(int j, int radius)
        {
            rotation += 0.017f;
            proj.timeLeft = 100;
            proj.Center = ArchaeaNPC.AngleBased(position, (float)Math.PI / 3f * j, radius * 4f * (float)Math.Cos(rotation));
        }
        public void Update(NPC npc, Player target)
        {
            proj.timeLeft = 100;
            if (npc.Distance(target.Center) < 800)
                focus = target.Center;
            else focus = npc.Center;
            ArchaeaNPC.RotateIncrement(proj.Center.X > focus.X, ref proj.rotation, ArchaeaNPC.AngleTo(focus, proj.Center), 0.5f, out proj.rotation);
            proj.velocity += ArchaeaNPC.AngleToSpeed(npc.rotation, 0.4f);
            VelClamp(ref proj.velocity, -5f, 5f, out proj.velocity);
            if (proj.velocity.X < 0f && proj.oldVelocity.X >= 0f || proj.velocity.X > 0f && proj.oldVelocity.X <= 0f || proj.velocity.Y < 0f && proj.oldVelocity.Y >= 0f || proj.velocity.Y > 0f && proj.oldVelocity.Y <= 0f)
                proj.netUpdate = true;
        }
        private void VelClamp(ref Vector2 input, float min, float max, out Vector2 result)
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
