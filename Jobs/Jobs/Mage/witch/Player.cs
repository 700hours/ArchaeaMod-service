using Terraria.ID;
using Terraria;

namespace ArchaeaMod.Jobs.Global
{
    public class witch
	{
        public static void GiveGear(Player player)
        {
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ItemID.WandofSparking);
        }
        public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(ItemID.WandofSparking);
			player.inventory[1].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[2].SetDefaults(ItemID.CopperAxe);
			for(int i = 0; i < 3; i++)
			{
				player.inventory[i].stack = 1;
				player.inventory[i].UpdateItem(1);
			}
		}
	}
}