using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Map;
using Terraria;

namespace ArchaeaMod.Jobs.Buffs
{
    internal static class BuffHelper
    {
        public static bool NBuffInit(NPC npc, int buffIndex, int maxTime)
        {
            return npc.buffTime[buffIndex] == maxTime - 2;
        }
        public static bool PBuffInit(Player player, int buffIndex, int maxTime)
        {
            return player.buffTime[buffIndex] == maxTime - 2;
        }
    }
}
