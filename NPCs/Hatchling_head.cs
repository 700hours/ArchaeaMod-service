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
            npc.aiStyle = -1;
            npc.width = 34;
            npc.height = 30;
            npc.lifeMax = 50;
            npc.defense = 10;
            npc.damage = 10;
            npc.value = 0;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.knockBackResist = 0f;
            bodyType = ModContent.NPCType<Hatchling_body>();
            tailType = ModContent.NPCType<Hatchling_tail>();
        }
        
        public override void AI()
        {
            WormAI();
        }

        public override void NPCLoot()
        {
            int rand = Main.rand.Next(10);
            switch (rand)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    Item.NewItem(npc.Center, ModContent.ItemType<Merged.Items.Materials.magno_core>(), Main.rand.Next(1, 4));
                    break;
            }
        }
    }
}
