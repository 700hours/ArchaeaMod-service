public void UseItem(Player player, int playerID)
{ 
	Vector2 tilev = new Vector2((int)player.position.X/16, (int)(player.position.Y+player.height-2)/16);
	if(Main.tile[(int)tilev.X, (int)tilev.Y+1].type == 2)
	{
		if(Main.rand.Next(24) == 0) 
		{
			int acorn = Item.NewItem((int)player.position.X,(int)player.position.Y,32,32,(int)Config.itemDefs.byName["Acorn"].type,1,false);
			if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(21, -1, -1, "", acorn, 0f, 0f, 0f, 0);
		}
		if(Main.rand.Next(128) == 0)
		{
			WorldGen.KillTile((int)tilev.X, (int)tilev.Y+1, false, false, true);
			if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(10, -1, -1, "", 0, 0f, 0f, 0f, 0);
		}
	}
	if(Main.tile[(int)tilev.X, (int)tilev.Y+1].type == 1)
	{
		if(Main.rand.Next(96) == 10)
		{
			int stone = Item.NewItem((int)player.position.X,(int)player.position.Y,32,32,(int)Config.itemDefs.byName["Stone Block"].type,1,false);
			if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(21, -1, -1, "", stone, 0f, 0f, 0f, 0);
		}
		if(Main.rand.Next(512) == 0)
		{
			WorldGen.KillTile((int)tilev.X, (int)tilev.Y+1, false, false, true);
			if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(10, -1, -1, "", 0, 0f, 0f, 0f, 0);
		}
	}
}