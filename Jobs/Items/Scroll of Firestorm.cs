public void UseItem(Player player, int playerID)
{
	ModPlayer.fireStorm = true;
	Main.PlaySound(2,(int)player.position.X,(int)player.position.Y,SoundHandler.soundID["conjure"]);
}