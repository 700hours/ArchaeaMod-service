int OldNPCdmg = 0;
public void NPCEffectsStart(NPC N,int buffIndex,int buffType,int buffTime)
{
	buffTime = 600;
	buffType = -1;
	OldNPCdmg = N.damage;
	if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", N.whoAmI, 0f, 0f, 0f, 0);
}
public void NPCEffects(NPC N,int buffIndex,int buffType,int buffTime)
{
	N.damage /= 2;
	Color color = new Color(0, 230, 200, 190);
	N.color = color;
	if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", N.whoAmI, 0f, 0f, 0f, 0);
}
public void NPCEffectsEnd(NPC N,int buffIndex,int buffType,int buffTime)
{
	N.color = default(Color);
	N.damage = OldNPCdmg;
	if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", N.whoAmI, 0f, 0f, 0f, 0);
}