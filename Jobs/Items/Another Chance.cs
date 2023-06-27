using Terraria;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Items
{ 
	public class AnotherChance : ModItem
	{
        public override bool? UseItem(Player player)
        {
            UseItem(player, player.whoAmI);
        }
        public void UseItem(Player player, int playerID)
		{
			if(ModPlayer.extraLife < 3) ModPlayer.extraLife++;
			if(ModPlayer.extraLife == 3) item.stack++;
			player.statLifeMax += 20;
			if(Main.dedServ || Main.netMode != 0) 
			{
				NetMessage.SendData(16, -1, -1, "", playerID, 0f, 0f, 0f, 0);
				NetMessage.SendModData(ModWorld.modId,ModPlayer.extraLife,-1,-1,player.whoAmi);
			}
		}
	}
}