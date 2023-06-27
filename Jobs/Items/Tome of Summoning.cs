public void UseItem(Player player, int playerID)
{
	Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X - (player.width/2), Main.mouseY + Main.screenPosition.Y - (player.height/2));
	int m = Main.rand.Next(Main.npc.Length);
	if(!Main.npc[m].boss && player.statMana > player.statManaMax/3) 
	{
		player.statMana -= player.statManaMax/3;
		player.manaRegenDelay = (int)player.maxRegenDelay;
		int CreatePlayered = NPC.NewNPC((int)mousev.X, (int)mousev.Y, m);
		#region dust
		Color newColor = default(Color);
		int a = Dust.NewDust(new Vector2(mousev.X - 10f, mousev.Y - 1f), 20, 2, 20, 0f, 0f, 100, newColor, 2.5f);
		int b = Dust.NewDust(new Vector2(mousev.X - 1f, mousev.Y - 10f), 2, 10, 20, 0f, 0f, 100, newColor, 2.5f);
		int c = Dust.NewDust(new Vector2(mousev.X-2, mousev.Y-2), 4, 4, 6, 0f, 0f, 100, newColor, 2f);
		Main.dust[a].noGravity = true;
		Main.dust[b].noGravity = true;
		Main.dust[c].noGravity = false;
		#endregion
		Main.PlaySound(2,(int)mousev.X,(int)mousev.Y,SoundHandler.soundID["summon"]);
		if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", Main.npc[CreatePlayered].whoAmI, 0f, 0f, 0f, 0);
	}
}