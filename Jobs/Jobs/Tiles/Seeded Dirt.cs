private int x, y, time = 0, shouldbe = 0, shouldbe2 = 0, randomizer;
public void Initialize(int i,int j)
{
	x = i; 
	y = j;
}
public void Update()
{
	time++;
	if(Main.tile[x, y].frameNumber != shouldbe)
    {
		Main.tile[x, y].frameX = (short)(18*shouldbe);
		Main.tile[x, y].frameNumber = (byte)shouldbe;
		if(Main.netMode == 2) NetMessage.SendTileSquare(-1, x, y, 1);
    }
	if(Main.tile[x-1, y].liquid > 20 || Main.tile[x+1, y].liquid > 20){
		shouldbe = 2;
	}
	else shouldbe = 1;
	RandomPlant();
}
public void RandomPlant()
{
	for(int i = 0; i < 10; i++)
	if(time%4800 != 0 && Main.tile[x, y-i].active){
		return;
	}
	else
	{
		if(Main.tile[x-1, y].liquid > 20 || Main.tile[x+1, y].liquid > 20){
			WorldGen.PlaceTile(x, y-1, 84, true, false);
		}
		if(Main.tile[x, y-1].type == 84){
			Main.tile[x, y-1].frameX = (short)(18*Main.rand.Next(1, 6));
			Main.tile[x, y-1].frameNumber = (byte)Main.rand.Next(1, 6);
			if(Main.netMode == 2) NetMessage.SendTileSquare(-1, x, y-1, 1);
		}
	}
}