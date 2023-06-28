int OldNPCdef = 0;
public void NPCEffectsStart(NPC N,int buffIndex,int buffType,int buffTime)
{
	buffTime = 600;
	buffType = -1;
	OldNPCdef = N.defense;
	if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", N.whoAmI, 0f, 0f, 0f, 0);
}
public void NPCEffects(NPC N,int buffIndex,int buffType,int buffTime)
{
	N.defense /= 2;
	Color color = new Color(0, 255, 200, 220);
	N.color = color;
	if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", N.whoAmI, 0f, 0f, 0f, 0);
}
public void NPCEffectsEnd(NPC N,int buffIndex,int buffType,int buffTime)
{
	N.color = default(Color);
	N.defense = OldNPCdef;
	if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", N.whoAmI, 0f, 0f, 0f, 0);
}