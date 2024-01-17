using Terraria.ID;
using Terraria;

namespace ArchaeaMod.Jobs.Global
{
    public class merchant
	{
        public static void GiveGear(Player player)
        {
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ItemID.WoodenTable);
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ItemID.WoodenChair);
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ItemID.Chest);
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ItemID.Candle);
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ItemID.SilverCoin, 10);
        }
        public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(ItemID.CopperShortsword);
			player.inventory[1].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[2].SetDefaults(ItemID.CopperAxe);
			player.inventory[3].SetDefaults(ItemID.WoodenTable);
			player.inventory[4].SetDefaults(ItemID.WoodenChair);
			player.inventory[5].SetDefaults(ItemID.Chest);
			for(int i = 0; i < 5; i++)
			{
				player.inventory[i].stack = 1;
				player.inventory[i].UpdateItem(1);
			}
			player.inventory[8].SetDefaults(ItemID.Candle);
			player.inventory[8].stack = 2;
			player.inventory[8].UpdateItem(1);
			player.inventory[9].SetDefaults(ItemID.SilverCoin);
			player.inventory[9].stack = 10;
			player.inventory[9].UpdateItem(1);
		}
	}
}