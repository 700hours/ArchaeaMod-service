using Terraria.ID;
using Terraria;

namespace ArchaeaMod.Jobs.Global
{
    public class business_man
	{
		public static void GiveGear(Player player)
		{
			player.QuickSpawnItem(player.GetSource_DropAsItem(), ItemID.SilverCoin, 50);
		}
		public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(ItemID.CopperShortsword);
			player.inventory[1].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[2].SetDefaults(ItemID.CopperAxe);
			for(int i = 0; i < 3; i++)
			{
				player.inventory[i].stack = 1;
				player.inventory[i].UpdateItem(1);
			}
			player.inventory[9].SetDefaults(ItemID.SilverCoin);
			player.inventory[9].stack = 50;
			player.inventory[9].UpdateItem(1);
		}
	}
}