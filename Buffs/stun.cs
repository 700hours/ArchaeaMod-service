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
        bool init = false;
        Color oldColor = default;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stunned");
            Main.pvpBuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (!init)
            { 
                oldColor = npc.color;
            }
            npc.velocity = Vector2.Zero;
            npc.color = Color.LightGray;
            if (npc.buffTime[buffIndex] == 2)
            {
                npc.color = oldColor;
            }
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (!init)
            {
                oldColor = player.skinColor;
            }
            player.velocity = Vector2.Zero;
            player.skinColor = Color.LightGray;
            if (player.buffTime[buffIndex] == 2)
            {
                player.skinColor = oldColor;
            }
        }
    }
}
