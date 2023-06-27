using Terraria.ID;
using Terraria;

namespace ArchaeaMod.Jobs.Global
{
    public class wizard
	{ 
		public static void CreatePlayer(Player player)
		{
			//player.inventory[0].SetDefaults("Box Staff");
			player.inventory[1].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[2].SetDefaults(ItemID.CopperAxe);
			//player.inventory[3].SetDefaults("Tome of Teleportation");
			//player.inventory[4].SetDefaults("Manipulate");
			for(int i = 0; i < 5; i++)
			{
				player.inventory[i].stack = 1;
				player.inventory[i].UpdateItem(1);
			}
			//player.inventory[7].SetDefaults("Scroll of Frozen Nova");
			//player.inventory[7].stack = 3;
			//player.inventory[7].UpdateItem(1);
			//player.inventory[8].SetDefaults("Scroll of Incognito");
			//player.inventory[8].stack = 3;
			//player.inventory[8].UpdateItem(1);
			player.inventory[9].SetDefaults(ItemID.ManaCrystal);
			player.inventory[9].stack = 1;
			player.inventory[9].UpdateItem(1);
		}
	}
}