public void UseItem(Player player, int playerID)
{ 
	if(Main.dungeonX*16 < player.position.X) Main.NewText("Dungeon left",160,180,200);
	else Main.NewText("Dungeon right",160,180,200);
}