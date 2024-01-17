using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Global
{
    public class surveyor
    {
        public static void GiveGear(Player player)
        {
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Jobs.Items.Prospecting_tool>());
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Jobs.Items.DungeonLocator>());
        }
        public static void CreatePlayer(Player player)
        {
            player.inventory[0].SetDefaults(ItemID.CopperShortsword);
            player.inventory[1].SetDefaults(ItemID.CopperPickaxe);
            player.inventory[2].SetDefaults(ItemID.CopperAxe);
            //player.inventory[3].SetDefaults("Surveyor's Tool");
            //player.inventory[4].SetDefaults("Dungeon Locator");
            for (int i = 0; i < 5; i++)
            {
                player.inventory[i].stack = 1;
                player.inventory[i].UpdateItem(1);
            }
        }
    }
}