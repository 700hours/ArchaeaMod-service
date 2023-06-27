public void UseItem(Player player, int playerID)
{ 
	int m = Main.rand.Next(Main.npc.Length);
	if(!Main.npc[m].boss) 
	{
		int CreatePlayered = NPC.NewNPC((int)(Main.mouseX + Main.screenPosition.X), (int)(Main.mouseY + Main.screenPosition.Y), m);	
	}
}