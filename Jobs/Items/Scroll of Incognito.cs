public void UseItem(Player player, int PID)
{
	player.AddBuff("Zombie", 900, false);
	Main.PlaySound(2,(int)player.position.X,(int)player.position.Y,SoundHandler.soundID["ts"]);
}