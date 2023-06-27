public void UseItem(Player player, int playerID)
{
	Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
	Rectangle mouse = new Rectangle((int)(mousev.X - 16f), (int)(mousev.Y - 16f), 32, 32);
	Player[] plr = Main.player;
	if(player.statMana >= 3){
		for(int m = 0; m < plr.Length-1; m++)
		{
			Player P = plr[m];
			if(!P.active) continue;
			if(P.statLife <= 0) continue;
			Vector2 pv = new Vector2(P.position.X, P.position.Y);
			Rectangle pBox = new Rectangle((int)pv.X, (int)pv.Y, P.width, P.height);
			if(mouse.Intersects(pBox) && Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Main.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
			{
			Color newColor = default(Color);
			int a = Dust.NewDust(new Vector2(pv.X, pv.Y), P.width, P.height, 29, 0f, 0f, 100, newColor, 2f);
			Main.dust[a].noGravity = true;
			//	Main.PlaySound(2,(int)player.position.X,(int)player.position.Y,SoundHandler.soundID["curse"]);
				P.AddBuff("Fortitude",7200,false);
				if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", P.whoAmi, 0f, 0f, 0f, 0);
			}
		}
	}
}