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

using ArchaeaMod.NPCs;

namespace ArchaeaMod.NPCs
{
    public class Sky_3 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gargoyle");
            Main.npcFrameCount[NPC.type] = 10;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 48;
            NPC.height = 48;
            NPC.lifeMax = 200;
            NPC.defense = 10;
            NPC.damage = 55;
            NPC.value = 150;
            NPC.alpha = 255;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;
        }
        private bool attack;
        private bool init;
        private bool firstTarget;
        
        private const float rushSpeed = 12f;
        private const float slowRate = 0.1f;
        private const float rotateSpeed = 0.05f;
        private Vector2 tracking;
        private Vector2 newPosition;
        private int time
        {
            get { return (int)NPC.ai[0]; }
            set { NPC.ai[0] = value; }
        }
        private int ai
        {
            get { return (int)NPC.ai[1]; }
            set { NPC.ai[1] = value; }
        }
        private const int 
            Idle = 0,
            Activated = 1,
            Attack = 2;
        private Player npcTarget
        {
            get { return Main.player[NPC.target]; }
        }
        public Player target()
        {
            Player player = ArchaeaNPC.FindClosest(NPC, firstTarget, 800);
            firstTarget = false;
            if (player != null && player.active && !player.dead)
            {
                NPC.target = player.whoAmI;
                NPC.netUpdate = true;
                return player;
            }
            else return Main.player[NPC.target];
        }
        public override void AI()
        {
            time++;
            if (!init)
            {
                int i = (int)NPC.position.X / 16;
                int j = (int)NPC.position.Y / 16;
                if (ArchaeaWorld.Inbounds(i, j) && Main.tile[i, j].WallType != ArchaeaWorld.skyBrickWall)
                {
                    newPosition = ArchaeaNPC.FindAny(NPC, ArchaeaNPC.FindClosest(NPC, true), true, 800);
                    if (newPosition != Vector2.Zero)
                    {
                        NPC.netUpdate = true;
                        init = true;
                    }
                }
                return;
            }
            if (!Main.tile[(int)(newPosition.X + 8) / 16, (int)(newPosition.Y + NPC.height + 8) / 16].HasTile || !Main.tileSolid[Main.tile[(int)(newPosition.X + 8) / 16, (int)(newPosition.Y + NPC.height + 8) / 16].TileType] ||
                !Main.tile[(int)(newPosition.X + 24) / 16, (int)(newPosition.Y + NPC.height + 8) / 16].HasTile || !Main.tileSolid[Main.tile[(int)(newPosition.X + 24) / 16, (int)(newPosition.Y + NPC.height + 8) / 16].TileType])
            {
                newPosition.Y++;
                return;
            }
            if (newPosition != Vector2.Zero && ai == Idle)
                NPC.position = newPosition;
            if (time > 150)
            {
                if (NPC.alpha > 12)
                    NPC.alpha -= 12;
                else NPC.alpha = 0;
            }
            if (NPC.life < NPC.lifeMax)
            {
                NPC.TargetClosest();
                ai = Activated;
            }
            if (ai == Idle && Main.player.Where(t => t.Distance(NPC.Center) < 64f).Count() > 0f)
            {
                ArchaeaNPC.DustSpread(NPC.position, NPC.width, NPC.height, DustID.Stone, 5, 1.2f);
                NPC.TargetClosest();
                ai = Activated;
                NPC.netUpdate = true;
            }
            if (ai == Idle) 
                return;
            NPC.spriteDirection = Main.player[NPC.target].Center.X < NPC.Center.X ? 1 : -1;
            if (time > 300)
            {
                time = 0;
                ai = Attack;
            }
            if (time % 120 == 0)
                tracking = npcTarget.Center;
            if (ai == Attack)
            {
                NPC.velocity = ArchaeaNPC.AngleToSpeed(NPC.AngleTo(tracking), rushSpeed);
                if (time % 10 == 0)
                    ai = Activated;
                NPC.netUpdate = true;
            }
            ArchaeaNPC.SlowDown(ref NPC.velocity, 0.2f);
            if (NPC.velocity.X >= NPC.oldVelocity.X || NPC.velocity.X < NPC.oldVelocity.X || NPC.velocity.Y >= NPC.oldVelocity.Y || NPC.velocity.Y < NPC.oldVelocity.Y)
                NPC.netUpdate = true;
            if (NPC.Center.X <= npcTarget.Center.X)
            {
                float angle = (float)Math.Round(Math.PI * 0.2f, 1);
                if (NPC.rotation > angle)
                NPC.rotation -= rotateSpeed;
                else NPC.rotation += rotateSpeed;
            }
            else
            {
                float angle = (float)Math.Round((Math.PI * 0.2f) * -1, 1);
                if (NPC.rotation > angle)
                NPC.rotation -= rotateSpeed;
                else NPC.rotation += rotateSpeed;
            }
        }

        public override void OnKill()
        {
            int rand = Main.rand.Next(10);
            switch (rand)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    Item.NewItem(Item.GetSource_None(), NPC.Center, ModContent.ItemType<Items.Materials.r_plate>(), Main.rand.Next(1, 4));
                    break;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Darkness, 480);
            if (Main.netMode == 2)
                NetMessage.SendData(MessageID.AddPlayerBuff, target.whoAmI, -1, null);
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return NPC.alpha == 0;
        }
        private int frame;
        private int frameCount;
        private int frameTime = 6;
        public override void FindFrame(int frameHeight)
        {
            if (!Main.dedServ)
            {
                frameCount = Main.npcFrameCount[NPC.type];
                frameHeight = TextureAssets.Npc[NPC.type].Value.Height / frameCount;
            }
            if (ai == Idle)
            {
                if (Main.player.Where(t => t.Distance(NPC.Center) < 300f).Count() > 0)
                {
                    frame = 1;
                }
                else frame = 0;
            }
            if (ai > 0)
            {
                if (time % frameTime == 0 && time != 0)
                {
                    if (frame < frameCount - 1)
                        frame++;
                    else frame = 2;
                }
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(newPosition);
            writer.WriteVector2(tracking);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            newPosition = reader.ReadVector2();
            tracking = reader.ReadVector2();
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool SkyFort = spawnInfo.Player.GetModPlayer<ArchaeaPlayer>().SkyFort;
            return SkyFort ? 0.4f : 0f;
        }
    }
}