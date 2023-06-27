public void UseItem(Player player, int playerID)
{ 
	Vector2 tilev = new Vector2((int)player.position.X/16, (int)player.position.Y/16);
	for(int i = (int)tilev.X+3; i < (int)tilev.X+13; i++)
	if(!Main.tile[i, (int)tilev.Y].active && Main.tileSolid[Main.tile[i, (int)tilev.Y+1].type])
	{
		//fill in gap
		WorldGen.PlaceTile((int)tilev.X+3, (int)tilev.Y-1, 30, false, true);
		WorldGen.PlaceTile((int)tilev.X+13, (int)tilev.Y-1, 30, false, true);
		//steps
		WorldGen.PlaceTile((int)tilev.X+12, (int)tilev.Y-1, 30, false, true);
		WorldGen.PlaceTile((int)tilev.X+12, (int)tilev.Y-2, 30, false, true);
		WorldGen.PlaceTile((int)tilev.X+12, (int)tilev.Y-1, 30, false, true);
		WorldGen.PlaceTile((int)tilev.X+12, (int)tilev.Y-2, 30, false, true);
		//frame
		for(int j = (int)tilev.X+3; j < (int)tilev.X+13; j++)
		{
			WorldGen.PlaceTile(j, (int)tilev.Y+1, 30, false, true);
			WorldGen.PlaceTile(j, (int)tilev.Y-6, 30, false, true);
			WorldGen.PlaceTile(j, (int)tilev.Y-12, 30, false, true);
			if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(10, -1, -1, "", 0, 0f, 0f, 0f, 0);
		}
	}
}