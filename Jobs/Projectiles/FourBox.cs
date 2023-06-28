private static int ticks = 0;
public void AI()
{
	Projectile P = this.projectile;
	Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y );
	Rectangle mouseBox = new Rectangle((int)(mousev.X - 16f), (int)(mousev.Y - 16f), 32, 32);
	Rectangle projBox = new Rectangle((int)P.position.X, (int)P.position.Y, P.width, P.height);
	if(projBox.Intersects(mouseBox) && Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed){
		P.position = new Vector2(mousev.X - (float)P.width/2, mousev.Y - (float)P.height/2);
	}
	for(int i = 1; i < 2; i++){
		Color color = new Color();
		int dust = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 59, 0, 0, 100, color, 2.0f);
		Main.dust[dust].noGravity = true;
	}
	float r = (float)Math.PI/16; 
	projectile.rotation += r;
	ticks++;
	if(ticks%3 == 0 && (P.velocity.X != 0 || P.velocity.Y != 0)){
		int num55 = Projectile.NewProjectile(P.position.X+(P.width/2), P.position.Y+(P.height/2), 0, 0,"FourBox Shadow", 0, 0, P.owner);
		Main.projectile[num55].aiStyle = 0;
		Main.projectile[num55].timeLeft = 10;
		Main.projectile[num55].tileCollide = false;
		Main.projectile[num55].rotation = P.rotation;
	}
}
public void Kill(){
	Projectile P = this.projectile;
		int num54 = Projectile.NewProjectile(P.position.X+(P.width/2), P.position.Y+(P.height/2), 6f, 6f, "OneBox", 10, 1f, P.owner);
	int num55 = Projectile.NewProjectile(P.position.X+(P.width/2), P.position.Y+(P.height/2), -6f, 6f, "OneBox", 10, 1f, P.owner);
		int num56 = Projectile.NewProjectile(P.position.X+(P.width/2), P.position.Y+(P.height/2), 6f, -6f, "OneBox", 10, 1f, P.owner);
	int num57 = Projectile.NewProjectile(P.position.X+(P.width/2), P.position.Y+(P.height/2), -6f, -6f, "OneBox", 10, 1f, P.owner);
	P.active = false;
}