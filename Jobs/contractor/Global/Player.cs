using Terraria.ID;
using Terraria;

namespace ArchaeaMod.Jobs.Global
{
    public class contractor
	{
		public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(ItemID.CopperShortsword);
			player.inventory[1].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[2].SetDefaults(ItemID.CopperAxe);
			player.inventory[3].SetDefaults(ItemID.SilverHammer);
			//player.inventory[4].SetDefaults("House Blueprint");
			//player.inventory[5].SetDefaults("Church Blueprint");
			//player.inventory[6].SetDefaults("Tower Blueprint");
			//player.inventory[6].stack = 2;
			for(int i = 0; i < 4; i++)
			{
				player.inventory[i].stack = 1;
				player.inventory[i].UpdateItem(1);
			}
		}
	}
}