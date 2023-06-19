using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Buffs
{
    public class stun : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stunned");
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.boss) 
                return;
            npc.velocity = Vector2.Zero;
            npc.color = Color.LightGray;
        }
    }
}
