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
using Terraria.ModLoader.Utilities;

namespace ArchaeaMod.NPCs
{
    public class Hatchling_head : Digger
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hatchling");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 34;
            NPC.height = 30;
            NPC.lifeMax = 50;
            NPC.defense = 10;
            NPC.damage = 10;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
            bodyType = ModContent.NPCType<Hatchling_body>();
            tailType = ModContent.NPCType<Hatchling_tail>();
        }
        
        public override void AI()
        {
            WormAI();
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
                    Item.NewItem(Item.GetSource_NaturalSpawn(), NPC.Center, ModContent.ItemType<Merged.Items.Materials.magno_core>(), Main.rand.Next(1, 4));
                    break;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool MagnoBiome = spawnInfo.Player.GetModPlayer<ArchaeaPlayer>().MagnoBiome;
            return MagnoBiome ? SpawnCondition.Cavern.Chance * 0.3f : 0f;
        }
    }
}
