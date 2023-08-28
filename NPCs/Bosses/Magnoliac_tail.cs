﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs.Bosses
{
    public class Magnoliac_tail : Hatchling_tail
    {
        public override bool IsLoadingEnabled(Mod mod) => true;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Magno");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 32;
            NPC.height = 32;
            NPC.lifeMax = 5000;
            NPC.defense = 10;
            NPC.knockBackResist = 0f;
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
            if (NPC.Distance(leader.Center) >= NPC.width + NPC.width / spacing)
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
            NPC.realLife = head.whoAmI;
        }
        public override bool CheckActive()
        {
            return !head.active;
        }
    }
}
