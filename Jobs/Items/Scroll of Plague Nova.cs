public void UseItem(Player P, int PID)
{
	for(int i = 1; i < 36; i++){
		int num54 = Projectile.NewProjectile(P.position.X+(P.width/2), P.position.Y+(P.height/2), Main.rand.Next(14)-7, Main.rand.Next(14)-7, "Poison", 10, 0f, P.whoAmi);
	}
}