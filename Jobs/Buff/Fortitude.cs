public void Effects(Player player,int buffIndex,int buffType,int buffTime) 
{
	player.statDefense += 20;
	if(Main.rand.Next(10) == 0){
		Color newColor = default(Color);
		int a = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 29, 0f, 0f, 100, newColor, 2f);
		Main.dust[a].noGravity = true;
	}
	if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(13, -1, -1, "", player.whoAmi, 0f, 0f, 0f, 0);
}