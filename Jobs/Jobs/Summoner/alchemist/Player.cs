using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Global
{
    public class alchemist
	{
        public static void GiveGear(Player player)
        {
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Jobs.Items.Scroll_plague_nova>(), 3);
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Jobs.Items.Scroll_plague_explosion>(), 3);
        }
        public static void CreatePlayer(Player player)
		{
			player.inventory[0].SetDefaults(ItemID.CopperPickaxe);
			player.inventory[1].SetDefaults(ItemID.CopperAxe);
			//player.inventory[2].SetDefaults("Tome of Summoning");
			//player.inventory[3].SetDefaults("Tome of Plague");
			//player.inventory[4].SetDefaults("Life Leech");
			//player.inventory[5].SetDefaults("Transmogrify");
			//for(int i = 0; i < 5; i++)
			//{
			//	  player.inventory[i].stack = 1;
			//	  player.inventory[i].UpdateItem(1);
			//}
			//player.inventory[7].SetDefaults("Scroll of Plague Nova");
			//player.inventory[7].stack = 3;
			//player.inventory[7].UpdateItem(1);
			//player.inventory[8].SetDefaults("Scroll of Plague Explosion");
			//player.inventory[8].stack = 3;
			//player.inventory[8].UpdateItem(1);
			//player.inventory[9].SetDefaults("Mana Crystal");
			//player.inventory[9].stack = 1;
			//player.inventory[9].UpdateItem(1);
		}
	}
}