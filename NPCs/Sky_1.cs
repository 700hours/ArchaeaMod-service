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
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ArchaeaMod.NPCs
{
    public class Sky_1 : Sky_air
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Resentful Spirit");
            Main.npcFrameCount[NPC.type] = 4;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 24;
            NPC.height = 48;
            NPC.lifeMax = 50;
            NPC.defense = 10;
            NPC.knockBackResist = 0f;
            NPC.damage = 45;
            NPC.value = 4000;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
        }

        private bool init;
        private bool chosenTexture;
        private int time
        {
            get { return (int)NPC.ai[3]; }
            set { NPC.ai[3] = value; }
        }
        private float upperPoint;
        private float oldX;
        private Vector2 idle;
        private Vector2 upper;
        private Vector2 newPosition;
        public override bool PreAI()
        {
            if (time++ > frameCount * frameTime)
                time = 0;
            if (!chosenTexture)
            {
                int rand = Main.rand.Next(4);
                switch (rand)
                {
                    case 0:
                        break;
                    case 1:
                        NPC.defense = 30;
                        NPC.netUpdate = true;
                        break;
                    case 2:
                        TextureAssets.Npc[NPC.type] = Mod.Assets.Request<Texture2D>("Gores/Sky_1_1");
                        NPC.defense = 20;
                        NPC.netUpdate = true;
                        break;
                    case 3:
                        TextureAssets.Npc[NPC.type] = Mod.Assets.Request<Texture2D>("Gores/Sky_1_2");
                        NPC.defense = 25;
                        NPC.netUpdate = true;
                        break;
                    case 4:
                        TextureAssets.Npc[NPC.type] = Mod.Assets.Request<Texture2D>("Gores/Sky_1_3");
                        NPC.defense = 25;
                        NPC.netUpdate = true;
                        break;
                }
                chosenTexture = true;
            }
            if (!init)
            {
                int i = (int)NPC.position.X / 16;
                int j = (int)NPC.position.Y / 16;
                if (ArchaeaWorld.Inbounds(i, j) && Main.tile[i, j].WallType != ArchaeaWorld.skyBrickWall)
                {
                    newPosition = ArchaeaNPC.FindAny(NPC, target(), true, 800);
                    if (newPosition != Vector2.Zero)
                    {
                        NPC.netUpdate = true;
                        if (Main.netMode == 0)
                            NPC.position = newPosition;
                    }
                    else return false;
                }
                oldX = NPC.position.X;
                upperPoint = NPC.position.Y - 50f;
                idle = NPC.position;
                upper = new Vector2(oldX, upperPoint);
                init = true;
            }
            return PreSkyAI() && BeginActive();
        }
        private bool fade;
        private bool attack = true;
        private bool findNewTarget;
        private Vector2 move;
        public override void AI()
        {
            SkyAI();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.r_plate>(), 3, 1, 4));
        }

        private bool BeginActive()
        {
            if (amount < 1f)
            {
                amount += 0.02f;
                degree = 5d * amount;
                NPC.position.Y = Vector2.Lerp(idle, upper, amount).Y;
                NPC.position.X += (float)Math.Cos(degree);
                return false;
            }
            else return true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return NPC.alpha == 0;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Darkness, 480);
            if (Main.netMode == 2)
                NetMessage.SendData(MessageID.AddPlayerBuff, target.whoAmI, -1, null);
        }
        private int frame;
        private int frameCount;
        private int frameTime;
        public override void FindFrame(int frameHeight)
        {
            frameTime = 7;
            if (!Main.dedServ)
            {
                frameCount = Main.npcFrameCount[NPC.type];
                frameHeight = TextureAssets.Npc[NPC.type].Value.Height / frameCount;
            }
            if (time % frameTime == 0 && time != 0)
            {
                if (frame < frameCount - 1)
                    frame++;
                else frame = 0;
            }
            NPC.frame.Y = frame * frameHeight;
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
        private bool foundWall;
        public override void SendExtraAI(BinaryWriter writer)
        {
            if (!foundWall)
                writer.WriteVector2(newPosition);
            writer.WriteVector2(move);
            writer.Write(attack);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            if (!foundWall)
            {
                NPC.position = reader.ReadVector2();
                foundWall = true;
            }
            move = reader.ReadVector2();
            attack = reader.ReadBoolean();
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool SkyFort = spawnInfo.Player.GetModPlayer<ArchaeaPlayer>().SkyFort;
            return SkyFort ? SpawnCondition.Sky.Chance * 0.6f : 0f;
        }
    }
}
