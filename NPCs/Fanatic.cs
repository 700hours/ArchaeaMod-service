using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class Fanatic : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fanatical Caster");
            Main.npcFrameCount[NPC.type] = 3;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 48;
            NPC.height = 58;
            NPC.lifeMax = 100;
            NPC.defense = 10;
            NPC.knockBackResist = 1f;
            NPC.damage = 10;
            NPC.value = 1000;
            NPC.lavaImmune = true;
        }
        public int timer
        {
            get { return (int)NPC.ai[0]; }
            set { NPC.ai[0] = value; }
        }
        public int maxAttacks
        {
            get { return 4; }
        }
        public int DustType;
        public Vector2 move;
        public Player npcTarget
        {
            get { return Main.player[NPC.target]; }
        }
        private bool init;
        private float compensate
        {
            get { return (float)(npcTarget.velocity.Y * (0.017d * 2.5d)); }
        }
        private bool fade;
        public override void AI()
        {
            int attackTime = 180 + 90 * maxAttacks;
            if (timer++ > 60 + attackTime)
                timer = 0;
            ArchaeaNPC.SlowDown(ref NPC.velocity, 0.1f);
            if (!init)
            {
                NPC.target = ArchaeaNPC.FindClosest(NPC, true).whoAmI;
                SyncNPC(NPC.position.X, NPC.position.Y);
                DustType = 6;
                var dusts = ArchaeaNPC.DustSpread(NPC.Center, 1, 1, DustType, 10);
                foreach (Dust d in dusts)
                    d.noGravity = true;
                init = true;
            }
            if (!fade)
            {
                if (NPC.alpha > 0)
                    NPC.alpha -= 255 / 60;
            }
            else
            {
                if (NPC.alpha < 255)
                    NPC.alpha += 255 / 50;
                else
                {
                    timer = attackTime + 50;
                    move = ArchaeaNPC.FindAny(NPC, npcTarget, true);
                    if (move != Vector2.Zero)
                    {
                        SyncNPC(move.X, move.Y);
                        var dusts = ArchaeaNPC.DustSpread(NPC.Center - new Vector2(NPC.width / 4, NPC.height / 4), NPC.width / 2, NPC.height / 2, DustType, 10, 2.4f);
                        foreach (Dust d in dusts)
                            d.noGravity = true;
                        fade = false;
                        timer = 0;
                    }
                }
            }
            if (timer > 180 && timer <= attackTime)
            {
                OrbGrow();
                if (timer >= 180 + 60 && timer % 90 == 0)
                    Attack();
            }
            if (timer >= attackTime)
                fade = true;
            else fade = false;
        }
        private float scale = 0.2f;
        private float weight;
        private Dust energy;
        public void Attack()
        {
            int proj = Projectile.NewProjectile(Projectile.GetSource_None(), NPC.Center + new Vector2(NPC.width * 0.35f * NPC.direction, -4f), ArchaeaNPC.AngleToSpeed(ArchaeaNPC.AngleTo(NPC, npcTarget) + compensate, 4f), ProjectileID.Fireball, 10, 1f);
            Main.projectile[proj].timeLeft = 300;
            Main.projectile[proj].friendly = false;
            Main.projectile[proj].tileCollide = false;
            scale = 0.2f;
        }
        public void OrbGrow()
        {
            NPC.direction = NPC.position.X < npcTarget.position.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            scale += 0.03f;
            energy = Dust.NewDustDirect(NPC.Center + new Vector2(NPC.width * 0.35f * NPC.direction, -4f), 3, 3, DustType, 0f, -0.2f, 0, default(Color), scale);
            energy.noGravity = true;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return NPC.alpha < 20;
        }

        public void SyncNPC(float x, float y)
        {
            if (Main.netMode != 0)
                NPC.netUpdate = true;
            else
            {
                NPC.position = new Vector2(x, y);
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(move);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.position = reader.ReadVector2();
        }

        private int frame;
        public override void FindFrame(int frameHeight)
        {
            int attackPhase = 90 * maxAttacks;
            if (timer < 180 || timer >= 180 + attackPhase)
                frame = 0;
            if (timer > 180 && timer < 180 + attackPhase && timer % 30 == 0)
                frame++;
            if (!Main.dedServ)
                frameHeight = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
            if (frame < 3)
                NPC.frame.Y = frame * frameHeight;
            else frame = 0;
        }

        public override void OnKill()
        {
            int rand = Main.rand.Next(12);
            switch (rand)
            {
                case 0:
                case 1:
                case 2:
                    Item.NewItem(Item.GetSource_NaturalSpawn(), NPC.Center, ModContent.ItemType<Merged.Items.Materials.magno_core>());
                    break;
                case 10:
                    int rand2 = Main.rand.Next(3);
                    switch (rand2)
                    {
                        case 0:
                            Item.NewItem(Item.GetSource_NaturalSpawn(), NPC.Center, ModContent.ItemType<Merged.Items.Armors.ancient_shockhelmet>());
                            break;
                        case 1:
                            Item.NewItem(Item.GetSource_NaturalSpawn(), NPC.Center, ModContent.ItemType<Merged.Items.Armors.ancient_shockplate>());
                            break;
                        case 2:
                            Item.NewItem(Item.GetSource_NaturalSpawn(), NPC.Center, ModContent.ItemType<Merged.Items.Armors.ancient_shockgreaves>());
                            break;
                    }
                    break;
            }
        }
    }
}
