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
			if(player.statLife != player.statLifeMax)
			{
				player.statLife++;
				nPC.life--;
				if(nPC.life <= 5){
					nPC.StrikeNPC(6, 0f, player.direction, true, false, 2f);
				}
				player.statMana--;
				player.manaRegenDelay = (int)player.maxRegenDelay;
			Color newColor = default(Color);
			int a = Dust.NewDust(new Vector2(nPC.position.X, nPC.position.Y), nPC.width, nPC.height, 5, 0f, 0f, 100, newColor, 1f);
			Main.dust[a].noGravity = false;
				Main.PlaySound(2,(int)mousev.X,(int)mousev.Y,SoundHandler.soundID["leech"]);
				if(Main.dedServ || Main.netMode != 0) 
				{
					NetMessage.SendData(16, -1, -1, "", player.whoAmi, 0f, 0f, 0f, 0);
					NetMessage.SendData(23, -1, -1, "", nPC.whoAmI, 0f, 0f, 0f, 0);
				}
			}
		}
	}
	if(player.statLife > player.statLifeMax) player.statLife = player.statLifeMax;
}