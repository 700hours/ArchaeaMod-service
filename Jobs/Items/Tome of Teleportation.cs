public void UseItem(Player player, int playerID)
{
	Vector2 tilev = new Vector2((Main.mouseX + Main.screenPosition.X - (player.width/2))/16, (Main.mouseY + Main.screenPosition.Y - (player.height/2))/16);
	Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X - (player.width/2), Main.mouseY + Main.screenPosition.Y - (player.height/2));
	if(player.statMana >= 10){
		if(!Main.tile[(int)tilev.X, (int)tilev.Y].active || !Main.tileSolid[Main.tile[(int)tilev.X, (int)tilev.Y].type]) {
			Main.PlaySound(2,(int)player.position.X,(int)player.position.Y,SoundHandler.soundID["teleport"]);
		Color newColor = default(Color);
		int a = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 20, 0f, 0f, 100, newColor, 2.5f);
		Main.dust[a].noGravity = true;
			player.position = mousev;
		int b = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 20, 0f, 0f, 100, newColor, 2.5f);
		Main.dust[b].noGravity = true;
			player.statMana -= 10;
			player.manaRegenDelay = (int)player.maxRegenDelay;
		}
	}
}