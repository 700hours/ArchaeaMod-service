public void UseStyle(Player player)
{
	for(int i = (int)(player.position.X)/16; i < (int)(player.position.X+player.width)/16; i++)
	for(int j = (int)player.position.Y/16; j < (int)(player.position.Y+player.height+16f)/16; j++)
	if(Main.tile[i, j].active && Main.tileSolid[Main.tile[i, j].type] && (Main.tile[i, j].type == 0 || Main.tile[i, j].type == 1 || Main.tile[i, j].type == 2 || Main.tile[i, j].type == 3 || Main.tile[i, j].type == 5 || Main.tile[i, j].type == 7 || Main.tile[i, j].type == 8 || Main.tile[i, j].type == 9 || Main.tile[i, j].type == 19 ||Main.tile[i, j].type == 23 || Main.tile[i, j].type == 30 || Main.tile[i, j].type == 32 || Main.tile[i, j].type == 40 || Main.tile[i, j].type == 45 || Main.tile[i, j].type == 46 || Main.tile[i, j].type == 47 || Main.tile[i, j].type == 53 || Main.tile[i, j].type == 54 || Main.tile[i, j].type == 57 || Main.tile[i, j].type == 59 || Main.tile[i, j].type == 60 || Main.tile[i, j].type == 69 || Main.tile[i, j].type == 70 || Main.tile[i, j].type == 72 || Main.tile[i, j].type == 109 || Main.tile[i, j].type == 112 || Main.tile[i, j].type == 116 || Main.tile[i, j].type == 120 || Main.tile[i, j].type == 123 || Main.tile[i, j].type == 124 || Main.tile[i, j].type == 130 || Main.tile[i, j].type == 131 || Main.tile[i, j].type == 145 || Main.tile[i, j].type == 146 || Main.tile[i, j].type == 147 || Main.tile[i, j].type == 148))
	{
		WorldGen.KillTile(i, j, false, false, false);
		if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(10, -1, -1, "", 0, 0f, 0f, 0f, 0);
	}
	if (player.runSoundDelay <= 0)
	{
		Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 22);
		player.runSoundDelay = 30;
	}
	if (Main.rand.Next(6) == 0)
	{
		int num123 = Dust.NewDust(player.position + player.velocity * (float)Main.rand.Next(6, 10) * 0.1f, player.width, player.height, 31, 0f, 0f, 80, default(Color), 1.4f);
		Dust expr_5B99_cp_0 = Main.dust[num123];
		expr_5B99_cp_0.position.X = expr_5B99_cp_0.position.X - 4f;
		Main.dust[num123].noGravity = true;
		Main.dust[num123].velocity *= 0.2f;
		Main.dust[num123].velocity.Y = (float)(-(float)Main.rand.Next(7, 13)) * 0.15f;
	}
}