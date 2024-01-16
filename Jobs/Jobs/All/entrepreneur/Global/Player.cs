using Terraria.ID;
using Terraria;

namespace ArchaeaMod.Jobs.Global
{
    public class entrepreneur
	{
		public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(ItemID.CopperShortsword);
			player.inventory[1].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[2].SetDefaults(ItemID.CopperAxe);
			player.inventory[9].SetDefaults(ItemID.LifeCrystal);
			for(int i = 0; i < 3; i++)
			{
				player.inventory[i].stack = 1;
				player.inventory[i].UpdateItem(1);
			}
		}
	}
}