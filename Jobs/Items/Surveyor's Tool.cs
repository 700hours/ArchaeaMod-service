public void UseItem(Player player, int playerID)
{ 
	Vector2 tilev = new Vector2(player.position.X/16, player.position.Y/16);
	for(int i = 0; i < 100; i++){
		Main.NewText(" ",200,200,200);
		Main.NewText(" ",200,200,200);
		Main.NewText(" ",200,200,200);
		if(Main.tile[(int)tilev.X, (int)tilev.Y+i].type == 7){
			Main.NewText("Copper Ore: True",255,170,120);
		}
		else Main.NewText("Copper Ore: False",255,170,120);
		if(Main.tile[(int)tilev.X, (int)tilev.Y+i].type == 6){
			Main.NewText("Iron Ore: True",200,175,140);
		}
		else Main.NewText("Iron Ore: False",200,175,140);
		if(Main.tile[(int)tilev.X, (int)tilev.Y+i].type == 9){
			Main.NewText("Silver Ore: True",165,165,175);
		}
		else Main.NewText("Silver Ore: False",165,165,175);
		if(Main.tile[(int)tilev.X, (int)tilev.Y+i].type == 8){
			Main.NewText("Gold Ore: True",200,190,140);
		}
		else Main.NewText("Gold Ore: False",200,190,140);
	}
}