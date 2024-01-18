using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Buffs;

using ArchaeaMod.Jobs.Projectiles;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Runtime.Intrinsics.X86;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Buffs
{
    internal class Soft : ModBuff
	{
		public const int MaxTime = 600;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Soft");
            Main.pvpBuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
			int buffTime = npc.buffTime[buffIndex];
            if (buffTime == MaxTime - 1)
			{
				NPCEffectsStart(npc, buffIndex, Type, 0);
			}
			else if (buffTime < MaxTime && buffTime > 1)
			{
				NPCEffects(npc, buffIndex, Type, 0);
			}
			else if (buffTime == 1)
			{
				NPCEffectsEnd(npc, buffIndex, Type, 0);
			}
			float radius = (float)(buffTime / MaxTime) * npc.width;
        }
        int OldNPCdef = 0;
		public void NPCEffectsStart(NPC N,int buffIndex,int buffType,int buffTime)
		{
			buffTime = 600;
			buffType = -1;
			OldNPCdef = N.defense;
			N.netUpdate = true;
		}
		public void NPCEffects(NPC N,int buffIndex,int buffType,int buffTime)
		{
			N.defense /= 2;
			Color color = new Color(0, 255, 200, 220);
			N.color = color;
            N.netUpdate = true;
        }
		public void NPCEffectsEnd(NPC N,int buffIndex,int buffType,int buffTime)
		{
			N.color = default(Color);
			N.defense = OldNPCdef;
            N.netUpdate = true;
        }
	}
}