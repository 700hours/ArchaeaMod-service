public static void SetBonus(Player P)
{
	Rectangle pBox = new Rectangle((int)P.position.X-8, (int)P.position.Y-8, P.width+8, P.height+8);
	foreach(NPC N in Main.npc){
		if(!N.active) continue;
		if(N.life <= 0) continue;
		if(N.friendly) continue;
		if(N.dontTakeDamage) continue;
		if(N.boss) continue;
		Rectangle nBox = new Rectangle((int)N.position.X, (int)N.position.Y, N.width, N.height);
		if(pBox.Intersects(nBox)){
			N.StrikeNPC(10, (float)Math.Round((double)P.velocity.X, 1), P.direction, false, false, 2f);
		}
	}
}