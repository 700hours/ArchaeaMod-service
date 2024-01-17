using Terraria.ID;
using Terraria;

namespace ArchaeaMod.Jobs.Global
{
    public class smith
	{
        public static void GiveGear(Player player)
        {
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ItemID.CopperHammer);
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ItemID.Furnace);
        }
        public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(ItemID.CopperBroadsword);
			player.inventory[1].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[2].SetDefaults(ItemID.CopperAxe);
			player.inventory[3].SetDefaults(ItemID.CopperHammer);
			player.inventory[4].SetDefaults(ItemID.Furnace);
			//player.inventory[5].SetDefaults("Iron Work Bench");
			for(int i = 0; i < 6; i++)
			{
				player.inventory[i].stack = 1;
				player.inventory[i].UpdateItem(1);
			}
		}
	}
}