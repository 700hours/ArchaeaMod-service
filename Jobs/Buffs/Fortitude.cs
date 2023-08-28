using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

using static System.Formats.Asn1.AsnWriter;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ArchaeaMod.Jobs.Buffs
{
    internal class Fortitude : ModBuff
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fortitude");
            Main.buffNoSave[Type] = true;
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            tip = "x3 melee damage";
            rare = 3;
        }
        public const int MaxTime = 7200;
        public override bool RightClick(int buffIndex)
        {
            Main.LocalPlayer.statDefense -= 20;
            return true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] == MaxTime)
            { 
                player.statDefense += 20;
            }
            else if (player.buffTime[buffIndex] == 1)
            { 
                player.statDefense -= 20;
            }
            if (Main.rand.NextBool(30))
			{
				Color newColor = default(Color);
				int a = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, DustID.Stone, 0f, 0f, 100, newColor, 2f);
				Main.dust[a].noGravity = true;
			}
		}
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.buffTime[buffIndex] == MaxTime)
            {
                npc.damage *= 3;
                npc.defense += 20;
            }
            else if (npc.buffTime[buffIndex] == 1)
            {
                npc.damage /= 3;
                npc.defense -= 20;
            }
            if (Main.rand.NextBool(30))
            {
                Color newColor = default(Color);
                int a = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Stone, 0f, 0f, 100, newColor, 2f);
                Main.dust[a].noGravity = true;
            }
        }
    }
}