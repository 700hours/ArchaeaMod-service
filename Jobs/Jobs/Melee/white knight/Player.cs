using Terraria.ID;
using Terraria;

namespace ArchaeaMod.Jobs.Global
{
    public class white_knight
	{
		public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(ItemID.SilverBroadsword);
			player.inventory[1].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[2].SetDefaults(ItemID.CopperAxe);
			//player.inventory[3].SetDefaults("Convert");
			for(int i = 0; i < 4; i++)
			{
				player.inventory[i].stack = 1;
				player.inventory[i].UpdateItem(1);
			}
			player.inventory[9].SetDefaults(ItemID.ManaCrystal);
			player.inventory[9].stack = 1;
			player.inventory[9].UpdateItem(1);
		}
	}
}