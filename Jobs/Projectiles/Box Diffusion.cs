private const float gravity = 0.0612f;
public void AI()
{
	projectile.AI(true);
	projectile.velocity.Y -= gravity;
	Color color = new Color();
	int dust = Dust.NewDust(new Vector2((float) projectile.position.X, (float) projectile.position.Y), projectile.width, projectile.height, 59, 0, 0, 100, color, 3.0f);
	Main.dust[dust].noGravity = true;
}