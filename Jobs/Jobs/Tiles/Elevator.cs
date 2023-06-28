private int x, y;
public void Initialize(int i,int j)
{
	x = i; 
	y = j;
	ModPlayer.Elevator = Projectile.NewProjectile((x+1)*16, (y+3)*16, 0, 0, "Elevator", 0, 0, Main.player[Main.myPlayer].whoAmi);
	Main.projectile[ModPlayer.Elevator].aiStyle = 0;
	Main.projectile[ModPlayer.Elevator].timeLeft = 2000;
	Main.projectile[ModPlayer.Elevator].tileCollide = false;
	Main.projectile[ModPlayer.Elevator].penetrate = 1;
}
public static void DestroyTile(int x, int y) 
{
	Main.projectile[ModPlayer.Elevator].active = false;
}