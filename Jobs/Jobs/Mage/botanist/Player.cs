using Terraria.ID;
using Terraria;

namespace ArchaeaMod.Jobs.Global
{ 
	public class botanist
	{ 
		public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(ItemID.WoodenBoomerang);
			player.inventory[1].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[2].SetDefaults(ItemID.CopperAxe);
			//player.inventory[3].SetDefaults("Textbook of Gathering");
			for(int i = 0; i < 3; i++)
			{
				player.inventory[i].stack = 1;
				player.inventory[i].UpdateItem(1);
			}
			player.inventory[9].SetDefaults(ItemID.Acorn);
			player.inventory[9].stack = 5;
		}
	}
}