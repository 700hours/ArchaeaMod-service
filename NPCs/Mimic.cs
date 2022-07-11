using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ArchaeaMod.NPCs
{
    public class Mimic : Slime
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mimic");
            Main.npcFrameCount[NPC.type] = 6;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 48;
            NPC.height = 32;
            NPC.lifeMax = 650;
            NPC.defense = 10;
            NPC.damage = 20;
            NPC.knockBackResist = 1f;
            NPC.value = Item.sellPrice(0, 1, 50, 0);
            NPC.lavaImmune = true;
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

        private bool init;
        private bool activated;
        public override bool JustSpawned()
        {
            if (!init)
            {
                flip = Main.rand.Next(2) == 0;
                SyncNPC();
                init = true;
            }
            if (NPC.life < NPC.lifeMax || (Main.mouseRight && NPC.Hitbox.Contains(Main.MouseWorld.ToPoint()) && ArchaeaNPC.WithinRange(target.position, new Rectangle(NPC.Hitbox.X - 75, NPC.Hitbox.Y - 75, 150, 150))))
            {
                pattern = Pattern.Attack;
                activated = true;
                SyncNPC();
            }
            return activated;
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
        }
        public override void Attack()
        {
            if (timer % 120 == 0 && timer != 0)
                SlimeJump(jumpHeight(FacingWall()), true, speedX(), target.position.X > NPC.position.X);
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

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            float force = NPC.knockBackResist * knockback;
            velX = player.Center.X > NPC.Center.X ? force * -1 : force;
            SyncNPC();
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            float force = NPC.knockBackResist * knockback;
            velX = projectile.position.X > NPC.Center.X ? force * -1 : force;
            SyncNPC();
        }

        public override void SyncNPC()
        {
            if (Main.netMode == 2)
                NPC.netUpdate = true;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(activated);
            writer.Write(flip);
            writer.Write(velX);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            activated = reader.ReadBoolean();
            flip = reader.ReadBoolean();
            velX = reader.ReadSingle();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new Items.HardModeDrop(), ModContent.ItemType<Items.dream_catcher>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Merged.Items.Materials.magno_core>(), 10));
        }
        public override void OnKill()
        {
            return;
            int rand = Main.rand.Next(10);
            switch (rand)
            {
                case 0:
                    if (Main.hardMode)
                        Item.NewItem(Item.GetSource_NaturalSpawn(), NPC.Center, ModContent.ItemType<Items.dream_catcher>());
                    else 
                        Item.NewItem(Item.GetSource_NaturalSpawn(), NPC.Center, ModContent.ItemType<Merged.Items.Materials.magno_core>());
                    break;
                case 1:
                case 2:
                case 3:
                    Item.NewItem(Item.GetSource_NaturalSpawn(), NPC.Center, ModContent.ItemType<Merged.Items.magno_summonstaff>());
                    break;
                case 4:
                case 5:
                case 6:
                    Item.NewItem(Item.GetSource_NaturalSpawn(), NPC.Center, ModContent.ItemType<Merged.Items.magno_yoyo>());
                    break;
                case 7:
                case 8:
                case 9:
                    Item.NewItem(Item.GetSource_NaturalSpawn(), NPC.Center, ModContent.ItemType<Merged.Items.magno_book>());
                    break;
                default:
                    goto case 0;
            }
        }

        private int time;
        private int frame;
        public override void FindFrame(int frameHeight)
        {
            if (pattern == Pattern.JustSpawned)
                frame = 0;
            if (!Main.dedServ)
                frameHeight = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
            if (frame < 5 && timer++ % 3 == 0)
            {
                if (NPC.velocity.Y < 0f)
                    frame++;
            }
            if (NPC.velocity.Y == 0f && frame > 1 && timer++ % 3 == 0)
            {
                NPC.spriteDirection = NPC.position.X < target.position.X ? 1 : -1;
                frame--;
            }
            NPC.frame.Y = frame * frameHeight;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool MagnoBiome = spawnInfo.Player.GetModPlayer<ArchaeaPlayer>().MagnoBiome;
            return MagnoBiome && Main.hardMode ? SpawnCondition.Cavern.Chance * 0.1f : 0f;
        }
    }
    
}
