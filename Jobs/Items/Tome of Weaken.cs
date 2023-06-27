public void UseItem(Player player, int playerID)
{
	Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
	Rectangle mouse = new Rectangle((int)(mousev.X - 16f), (int)(mousev.Y - 16f), 32, 32);
	NPC[] npc = Main.npc;
	if(player.statMana >= 7){
		for(int m = 0; m < npc.Length-1; m++)
		{
			NPC nPC = npc[m];
			if(!nPC.active) continue;
			if(nPC.life <= 0) continue;
			if(nPC.friendly) continue;
			if(nPC.dontTakeDamage) continue;
			if(nPC.boss) continue;
			Vector2 npcv = new Vector2(nPC.position.X, nPC.position.Y);
			Rectangle npcBox = new Rectangle((int)npcv.X, (int)npcv.Y, nPC.width, nPC.height);
			if(mouse.Intersects(npcBox) && Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Main.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
			{
				player.statMana -= 5;
				player.manaRegenDelay = (int)player.maxRegenDelay;
			Color newColor = default(Color);
			int a = Dust.NewDust(new Vector2(mousev.X - 10f, mousev.Y - 10f), 20, 20, 19, 0f, 0f, 100, newColor, 2f);
			Main.dust[a].noGravity = true;
				Main.PlaySound(2,(int)player.position.X,(int)player.position.Y,SoundHandler.soundID["curse"]);
				nPC.AddBuff("Weaken",600,false);
				if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", nPC.whoAmI, 0f, 0f, 0f, 0);
			}
		}
	}
}