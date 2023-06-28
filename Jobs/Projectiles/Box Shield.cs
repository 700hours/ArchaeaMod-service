public void AI()
{
/*	Player P = Main.player[Main.myPlayer];
	Vector2 origin = new Vector2(P.position.X+(P.width/2), P.position.Y+(P.height/2));
	int rotation = Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
	projectile.position = new Vector2(rotation
	float r = (float)Math.PI/16;
	projectile.rotation += r;*/
}
public void Kill(){
	for (int i = 0; i < 20; i++) {
		int num54 = Projectile.NewProjectile(this.projectile.position.X, this.projectile.position.Y,Main.rand.Next(10)-5,Main.rand.Next(10)-5,"Ice Diffusion Beam",50,0.1f,this.projectile.owner);
		Main.projectile[num54].timeLeft = 15;
		Main.projectile[num54].tileCollide = false;
	}
	this.projectile.active = false;
}