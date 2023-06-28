using Terraria.ID;
using Terraria;

namespace ArchaeaMod.Jobs.Global
{
    public class corperate_usurper
	{ 
		public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(0, false);
			player.inventory[1].SetDefaults(0, false);
			player.inventory[2].SetDefaults(0, false);
			player.inventory[3].SetDefaults(0, false);
	
			player.inventory[5].SetDefaults(ItemID.Wood);
			player.inventory[5].stack = 60;
			player.inventory[6].SetDefaults(ItemID.CopperBar);
			player.inventory[6].stack = 50;
			player.inventory[7].SetDefaults(ItemID.IronBar);
			player.inventory[7].stack = 30;
			player.inventory[8].SetDefaults(ItemID.SilverBar);
			player.inventory[8].stack = 15;
			player.inventory[9].SetDefaults(ItemID.GoldBar);
			player.inventory[9].stack = 12;
		}
	}
}