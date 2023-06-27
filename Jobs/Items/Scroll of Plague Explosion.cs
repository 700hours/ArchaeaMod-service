public void UseItem(Player player, int playerID)
{
	Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
	Rectangle mouse = new Rectangle((int)(mousev.X - 16f), (int)(mousev.Y - 16f), 32, 32);
	NPC[] npc = Main.npc;
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
			nPC.life = 1;
			for (int i = 0; i < 20; i++) {
				int num54 = Projectile.NewProjectile(npcv.X+(nPC.width/2), npcv.Y+(nPC.height/2), Main.rand.Next(10)-5, Main.rand.Next(10)-5, "Poison Diffusion", 5, 0f, player.whoAmi);
				Main.projectile[num54].timeLeft = 90;
				Main.projectile[num54].tileCollide = false;
			}
			if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", nPC.whoAmI, 0f, 0f, 0f, 0);
		}
	}
}