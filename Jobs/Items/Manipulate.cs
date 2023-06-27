public void UseItem(Player player, int playerID)
{ 
	Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y );
	Rectangle mouse = new Rectangle((int)(mousev.X - 16f), (int)(mousev.Y - 16f), 32, 32);
	NPC[] npc = Main.npc;
	for(int m = 0; m < npc.Length-1; m++)
	{
		NPC nPC = npc[m];
		Vector2 npcv = new Vector2(nPC.position.X, nPC.position.Y);
		Rectangle npcBox = new Rectangle((int)npcv.X, (int)npcv.Y, nPC.width, nPC.height);
		if(mouse.Intersects(npcBox) && !nPC.boss && player.statMana > 0 && Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
		{
			nPC.position = new Vector2(mousev.X - (float)nPC.width/2, mousev.Y - (float)nPC.height/2);
			player.statMana--;
			player.manaRegenDelay = (int)player.maxRegenDelay;
			if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", nPC.whoAmI, 0f, 0f, 0f, 0);
		}
	}
}