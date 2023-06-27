using Terraria.ID;
using Terraria;

namespace ArchaeaMod.Jobs.Global
{
    public class outlaw
	{ 
		public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(ItemID.FlintlockPistol);
			player.inventory[1].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[2].SetDefaults(ItemID.CopperAxe);
			for(int i = 0; i < 3; i++)
			{
				player.inventory[i].stack = 1;
				player.inventory[i].UpdateItem(1);
			}
			player.inventory[45].SetDefaults(ItemID.MusketBall);
			player.inventory[45].stack = 100;
			player.inventory[45].UpdateItem(1);
		}
	}
}