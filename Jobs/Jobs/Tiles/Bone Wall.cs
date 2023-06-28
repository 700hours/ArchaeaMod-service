public bool CheckPlaceTile(int x, int y) { return true;	}
private int x, y;
private int life = 480;
public void Initialize(int i,int j)
{
	x = i; 
	y = j;
}
public void Update()
{
	life--;
	if(life <= 0){
		Color color = new Color();
		int dust = Dust.NewDust(new Vector2((float)x*16f, (float)y*16f), 16, 16, 32, 0, 0, 100, color, 1.5f);
		Main.dust[dust].noGravity = false;
		Main.tile[x, y].active = false;
		Main.tile[x, y].type = 0;
	}
}