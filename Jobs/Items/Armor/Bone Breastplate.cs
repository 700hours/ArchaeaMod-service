public static void SetBonus(Player player)
{
	Vector2 mousev = new Vector2((Main.mouseX + Main.screenPosition.X)/16, (Main.mouseY + Main.screenPosition.Y)/16);
	if(Main.mouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && player.statMana >= 5){
		if(!Main.tile[(int)mousev.X, (int)mousev.Y].active || !Main.tileSolid[Main.tile[(int)mousev.X, (int)mousev.Y].type]) {
			WorldGen.PlaceTile((int)mousev.X, (int)mousev.Y, Config.tileDefs.ID["Bone Wall"], false, false, player.whoAmi, 0);
			player.statMana -= 5;
		}
	}
}