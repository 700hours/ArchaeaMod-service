public static Rectangle prjB, plrB;
public static bool switched = false, connected;
public static int switchTimer = 0;
public void AI()
{
	if(switched) switchTimer--;
	if(switchTimer <= 0) 
	{
		switchTimer = 30;
		switched = false;
	}
	if(projectile.timeLeft < 100) projectile.timeLeft = 2000;
	Player player = Main.player[Main.myPlayer];
	prjB = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
	plrB = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
	if(plrB.Intersects(prjB) && Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.X) < 0 && !connected && !switched)
	{
		connected = true;
		switched = true;
	}
	if(plrB.Intersects(prjB) && Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.X) < 0 && connected && !switched)
	{
		connected = false;
		switched = true;
	}
	if(connected)
	{
		player.position = new Vector2(projectile.position.X + 6f, projectile.position.Y - 16f);
		player.fallStart = (int)player.position.Y;
		if(player.controlDown) projectile.velocity.Y += 0.2f;
		if(player.controlUp) projectile.velocity.Y -= 0.2f;
		if(!player.controlDown && projectile.velocity.Y < 0) projectile.velocity.Y -= 0.5f;
		if(!player.controlUp && projectile.velocity.Y > 0) projectile.velocity.Y += 0.5f;
		int n = Config.tileDefs.ID["Elevator"];
		for(int i = (int)projectile.position.X/16; i < (int)(projectile.position.X+projectile.width)/16; i++)
		for(int j = (int)(projectile.position.Y+projectile.height)/16; j < (int)(projectile.position.Y+projectile.height+8f)/16; j++)
		{
			if(Main.tile[i, j].active && Main.tile[i, j].type == n && !player.controlDown)
			{
				projectile.velocity *= 0f;
			}
			if(Main.tile[i, j].active && Main.tileSolid[Main.tile[i, j].type] && Main.tile[i, j].type != 19 && !player.controlUp)
			{
				projectile.velocity *= 0f;
			}
		}
	}
	if(!connected) projectile.velocity.Y = 0;
	if(projectile.velocity.Y > 5f) projectile.velocity.Y = 5f;
	if(projectile.velocity.Y < -5f) projectile.velocity.Y = -5f;
}