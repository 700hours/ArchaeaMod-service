private static int ticks = 0;
public void AI()
{
	float r = (float)Math.PI/16;
	projectile.rotation += r;
	Projectile P = this.projectile;
	for(int i = 1; i < 2; i++){
		Color color = new Color();
		int dust = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 59, 0, 0, 100, color, 2.0f);
		Main.dust[dust].noGravity = true;
	}
	ticks++;
	if(ticks%3 == 0 && (P.velocity.X != 0 || P.velocity.Y != 0)){
		int num55 = Projectile.NewProjectile(P.position.X+(P.width/2), P.position.Y+(P.height/2), 0, 0,"OneBox Shadow", 0, 0, P.owner);
		Main.projectile[num55].aiStyle = 0;
		Main.projectile[num55].timeLeft = 10;
		Main.projectile[num55].tileCollide = false;
		Main.projectile[num55].rotation = P.rotation;
	}
}
public void Kill(){
	for (int i = 0; i < 20; i++) {
		int num54 = Projectile.NewProjectile(this.projectile.position.X, this.projectile.position.Y, Main.rand.Next(10)-5, Main.rand.Next(10)-5,"Box Diffusion", 0, 1f, this.projectile.owner);
		Main.projectile[num54].timeLeft = 15;
		Main.projectile[num54].tileCollide = false;
	}
	this.projectile.active = false;
}