using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Global
{
    public class scientist
	{
        public static void GiveGear(Player player)
        {
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Items.Textbook_of_gathering>());
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ItemID.Acorn, 5);
        }
        public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(ItemID.WoodenBoomerang);
			player.inventory[1].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[2].SetDefaults(ItemID.CopperAxe);
			player.inventory[3].SetDefaults(ModContent.ItemType<Items.Textbook_of_gathering>());
			for(int i = 0; i < 4; i++)
			{
				player.inventory[i].stack = 1;
				player.inventory[i].UpdateItem(1);
			}
			player.inventory[9].SetDefaults(ItemID.Acorn);
			player.inventory[9].stack = 5;
		}
	}
}