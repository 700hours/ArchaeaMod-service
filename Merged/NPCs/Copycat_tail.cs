using System;
using System.Collections.Generic;
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
    public class Copycat_tail : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Copycat");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 38;
            NPC.height = 36;
            NPC.lifeMax = 50;
            NPC.defense = 10;
            NPC.damage = 10;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
        }

        private NPC leader
        {
            get { return Main.npc[(int)NPC.ai[0]]; }
        }
        private NPC head
        {
            get { return Main.npc[(int)NPC.ai[1]]; }
        }
        private int spacing = 4;
        private float chaseSpeed = 5f;
        public override void AI()
        {
            NPC.rotation = NPC.AngleTo(leader.Center);
            if (NPC.Distance(leader.Center) >= NPC.width - spacing)
            {
                chaseSpeed += 0.2f;
                float angle = NPC.AngleTo(leader.Center);
                float cos = (float)(chaseSpeed * Math.Cos(angle));
                float sine = (float)(chaseSpeed * Math.Sin(angle));
                NPC.velocity = new Vector2(cos, sine);
            }
            else
            {
                NPC.velocity = Vector2.Zero;
                chaseSpeed = 5f;
            }
            if (!head.active || head.life <= 0)
                NPC.active = false;
        }
        public override bool CheckActive()
        {
            return head.active;
        }
    }
}
