using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Items
{
    internal class Transmogrify : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Transmogrify");
			Tooltip.SetDefault("");
        }
        public override bool? UseItem(Player player)
        {
            Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y );
			Rectangle mouse = new Rectangle((int)(mousev.X - 16f), (int)(mousev.Y - 16f), 32, 32);
			NPC[] npc = Main.npc;
			for(int m = 0; m < npc.Length-1; m++)
			{
				NPC nPC = npc[m];
				Vector2 npcv = new Vector2(nPC.position.X, nPC.position.Y);
				Rectangle npcBox = new Rectangle((int)npcv.X, (int)npcv.Y, nPC.width, nPC.height);
				if(mouse.Intersects(npcBox) && !nPC.boss && player.statMana >= player.statManaMax/3 &&Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
				{
					nPC.Transform(m);
					player.statMana -= player.statManaMax/3;
					player.manaRegenDelay = (int)player.maxRegenDelay;
					if(Main.dedServ || Main.netMode != 0) NetMessage.SendData(23, -1, -1, "", nPC.whoAmI, 0f, 0f, 0f, 0);
				}
			}
			return false;
		}
	}
}