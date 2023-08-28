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


namespace ArchaeaMod.Merged.NPCs
{
    public class Copycat_head : ArchaeaMod.NPCs.Digger
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Copycat");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 26;
            NPC.height = 38;
            NPC.lifeMax = 50;
            NPC.defense = 10;
            NPC.damage = 10;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
            bodyType = ModContent.NPCType<Copycat_body>();
            tailType = ModContent.NPCType<Copycat_tail>();
        }
        
        public override void AI()
        {
            WormAI();
        }
    }
}
