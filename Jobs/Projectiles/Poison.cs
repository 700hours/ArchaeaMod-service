public void AI()
{
	projectile.AI(true);
	Color color = new Color();
	int dust = Dust.NewDust(new Vector2((float) projectile.position.X, (float) projectile.position.Y), projectile.width, projectile.height, 61, 0, 0, 100, color, 3.0f);
	Main.dust[dust].noGravity = true;
	foreach(NPC N in Main.npc)
    {
		if(!N.active) continue;
		if(N.life <= 0) continue;
		if(N.friendly) continue;
		if(N.dontTakeDamage) continue;
		if(N.boss) continue;
		Rectangle MB = new Rectangle((int)projectile.position.X+(int)projectile.velocity.X,(int)projectile.position.Y+(int)projectile.velocity.Y,projectile.width,projectile.height);
		Rectangle NB = new Rectangle((int)N.position.X,(int)N.position.Y,N.width,N.height);
		if(MB.Intersects(NB))
		{
			N.AddBuff(20,600,false);
			N.StrikeNPC(18, 0f, 0, false, false, 2f);
		}
	}
}