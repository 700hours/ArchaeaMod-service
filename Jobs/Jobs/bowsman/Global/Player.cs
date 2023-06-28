using Terraria.ID;
using Terraria;

namespace ArchaeaMod.Jobs.Global
{
    public class bowsman
	{ 
		public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(ItemID.SilverBow);
			player.inventory[1].SetDefaults(ItemID.CopperShortsword);
			player.inventory[2].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[3].SetDefaults(ItemID.CopperAxe);
			for(int i = 0; i < 4; i++)
			{
				player.inventory[i].stack = 1;
				player.inventory[i].UpdateItem(1);
			}
			player.inventory[45].SetDefaults(ItemID.WoodenArrow);
			player.inventory[45].stack = 250;
			player.inventory[45].UpdateItem(1);
		}
	}
}