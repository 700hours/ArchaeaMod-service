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
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;

namespace ArchaeaMod.NPCs
{
    public class Slime_Mercurial : Slime
    {
        public override bool IsLoadingEnabled(Mod mod) => true;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mercurial Slime");
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 48;
            NPC.height = 32;
            NPC.lifeMax = 50;
            NPC.defense = 10;
            NPC.damage = 10;
            NPC.lavaImmune = true;
            NPC.DeathSound = SoundID.NPCDeath1;
        }
        private int count;
        private float compensateY;
        private bool preAI;
        public override bool PreAI()
        {
            if (NPC.wet)
                NPC.velocity.Y = 0.3f;
            preAI = SlimeAI();
            if (preAI)
            {
                if (NPC.velocity.Y != 0f)
                    NPC.velocity.X = velX;
            }
            return preAI;
        }
        public override void AI()
        {
            if (FacingWall())
                if (timer % interval / 4 == 0)
                    if (count++ > 3)
                    {
                        oldLife = NPC.life;
                        pattern = Pattern.Active;
                        count = 0;
                    }
            base.AI();
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.mercury>(), 450);
            NetMessage.SendData(MessageID.AddPlayerBuff, target.whoAmI, -1, null, ModContent.BuffType<Buffs.mercury>());
        }
        #region slime methods
        public override bool JustSpawned()
        {
            flip = Main.rand.Next(2) == 0;
            SyncNPC();
            return true;
        }
        public override void DefaultActions(int interval = 180, bool moveX = false)
        {
            if (timer % interval == 0 && timer != 0)
            {
                SlimeJump(jumpHeight(), moveX, speedX(), flip);
                flip = !flip;
            }
        }
        public override void Active(int interval = 120)
        {
            if (timer % interval == 0 && timer != 0)
            {
                SlimeJump(jumpHeight(FacingWall()), true, speedX(), flip);
                if (count++ > 3)
                {
                    flip = !flip;
                    count = 0;
                }
            }
            FadeTo(0, false);
        }
        public override void Attack()
        {
            pattern = Pattern.Attack;
            if (timer % 120 == 0 && timer != 0)
                SlimeJump(jumpHeight(FacingWall()), true, speedX(), target.position.X > NPC.position.X);
            FadeTo(100, true);
            if (!target.active || target.dead)
                pattern = Pattern.Idle;
        }
        public override void SlimeJump(float speedY, bool horizontal = false, float speedX = 0, bool direction = true)
        {
            NPC.velocity.Y -= speedY * 1.2f;
            if (horizontal)
            {
                velX = direction ? speedX / 2f : speedX / 2f * -1;
                SyncNPC();
            }
        }
        #endregion
        public override void SyncNPC()
        {
            if (Main.netMode == 2)
                NPC.netUpdate = true;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(flip);
            writer.Write(velX);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            flip = reader.ReadBoolean();
            velX = reader.ReadSingle();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Merged.Items.Materials.magno_core>(), 3));
            npcLoot.Add(ItemDropRule.ByCondition(new Items.ArchaeaModeDrop(), ModContent.ItemType<Merged.Items.Armors.ancient_shockhelmet>(), 13));
            npcLoot.Add(ItemDropRule.ByCondition(new Items.ArchaeaModeDrop(), ModContent.ItemType<Merged.Items.Armors.ancient_shockplate>(), 13));
            npcLoot.Add(ItemDropRule.ByCondition(new Items.ArchaeaModeDrop(), ModContent.ItemType<Merged.Items.Armors.ancient_shockgreaves>(), 13));
        }

        private bool elapsed;
        private int time;
        private int time2;
        private int frame;
        private int height;
        public override void FindFrame(int frameHeight)
        {
            if (!Main.dedServ)
                frameHeight = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
            height = frameHeight;
            if (NPC.velocity.Y != 0f)
            {
                if (time++ % 5 == 0)
                    elapsed = !elapsed;
                frame = elapsed ? 1 : 0;
            }
            else
            {
                if (pattern == Pattern.Attack)
                {
                    if (time2++ % 6 == 0)
                        elapsed = !elapsed;
                }
                else if (time2++ % 8 == 0)
                    elapsed = !elapsed;
                frame = elapsed ? 1 : 0;
            }
            NPC.frame.Y = frame * frameHeight;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool magnoBiome = spawnInfo.Player.GetModPlayer<ArchaeaPlayer>().MagnoBiome;
            bool downedMagno = ModContent.GetInstance<ArchaeaWorld>().downedMagno;
            return magnoBiome && downedMagno ? 0.4f : 0f;
        }
    }
}
