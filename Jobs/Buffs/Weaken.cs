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
    internal class Weaken : ModBuff
	{
		public const int MaxTime = 900;
		int OldNPCdmg = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Weaken");
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip = "Half damage";
			rare = 2;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            int buffTime = npc.buffTime[buffIndex];
			if (buffTime == MaxTime - 1)
			{
				NPCEffectsStart(npc, buffIndex, Type, 0);
			}
			if (buffTime < MaxTime && buffTime > 1)
			{
				NPCEffects(npc, buffIndex, Type, 0);
			}
			if (buffTime == 1)
			{
				NPCEffectsEnd(npc, buffIndex, Type, 0);
			}
        }
        public void NPCEffectsStart(NPC N,int buffIndex,int buffType,int buffTime)
		{
			buffTime = 600;
			buffType = -1;
			OldNPCdmg = N.damage;
			N.netUpdate = true;
		}
		public void NPCEffects(NPC N,int buffIndex,int buffType,int buffTime)
		{
			N.damage /= 2;
			Color color = new Color(0, 230, 200, 190);
			N.color = color;
		}
		public void NPCEffectsEnd(NPC N,int buffIndex,int buffType,int buffTime)
		{
			N.color = default(Color);
			N.damage = OldNPCdmg;
            N.netUpdate = true;
        }
	}
}