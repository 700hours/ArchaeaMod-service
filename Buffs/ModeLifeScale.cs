using ArchaeaMod.Mode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Map;
using Terraria.ModLoader;

namespace ArchaeaMod.Buffs
{
    internal class ModeLifeScale : ModBuff
    {
        bool init = false;
        const int MaxTime = 36000;
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.buffTime[buffIndex] < MaxTime - 1)
            {
                int lifeMax = ArchaeaMode.ModeScaling(ArchaeaMode.StatWho.NPC, ArchaeaMode.Stat.Life, npc.lifeMax, ModContent.GetInstance<ModeToggle>().healthScale, npc.defense, DamageClass.Default);
                npc.lifeMax = lifeMax;
                npc.life = lifeMax;
            }
            npc.buffTime[buffIndex] = MaxTime;
        }
    }
}
