using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static IL.Terraria.WorldBuilding.Searches;
using static System.Formats.Asn1.AsnWriter;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ArchaeaMod.Jobs.Buffs
{
    internal class unused_Frozen : ModBuff
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen");
			
        }
        int OldNPCai = -1;
		int OldNPCdmg = 0;
		public void NPCEffectsStart(NPC N,int buffIndex,int buffType,int buffTime)
		{
			buffTime = 600;
			buffType = -1;
			OldNPCai = N.aiStyle;
			OldNPCdmg = N.damage;
			if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", N.whoAmI, 0f, 0f, 0f, 0);
		}
		public void NPCEffects(NPC N,int buffIndex,int buffType,int buffTime)
		{
			N.velocity.X = N.velocity.X * 0;
			N.velocity.Y = N.velocity.Y * 0;
			N.damage = 0;
			N.aiStyle = 0;
			N.frame.Y = 0;
			Color color = new Color(0, 144, 255, 100);
			N.color = color;
			if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", N.whoAmI, 0f, 0f, 0f, 0);
		}
		public void NPCEffectsEnd(NPC N,int buffIndex,int buffType,int buffTime)
		{
			N.color = default(Color);
			N.aiStyle = OldNPCai;
			N.damage = OldNPCdmg;
			if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", N.whoAmI, 0f, 0f, 0f, 0);
		}
	}
}