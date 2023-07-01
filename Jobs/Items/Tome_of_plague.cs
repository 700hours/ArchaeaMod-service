using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Items
{
    internal class Tome_of_plague : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Plague");
			Tooltip.SetDefault("Plagues an enemy.");
        }
        public override bool? UseItem(Player player)
        {
            Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
			Rectangle mouse = new Rectangle((int)(mousev.X - 16f), (int)(mousev.Y - 16f), 32, 32);
			NPC[] npc = Main.npc;
			if (player.statMana >= 10)
			{
				for (int m = 0; m < npc.Length-1; m++)
				{
					NPC nPC = npc[m];
					if(!nPC.active) continue;
					if(nPC.life <= 0) continue;
					if(nPC.friendly) continue;
					if(nPC.dontTakeDamage) continue;
					Vector2 npcv = new Vector2(nPC.position.X, nPC.position.Y);
					Rectangle npcBox = new Rectangle((int)npcv.X, (int)npcv.Y, nPC.width, nPC.height);
					if(mouse.Intersects(npcBox) && Main.mouseLeft)
					{
						player.statMana -= 3;
						player.manaRegenDelay = (int)player.maxRegenDelay;
						Color newColor = default(Color);
						int a = Dust.NewDust(new Vector2(mousev.X - 10f, mousev.Y - 10f), 20, 20, 61, 0f, 0f, 100, newColor, 2f);
						Main.dust[a].noGravity = true;
						SoundEngine.PlaySound(SoundID.Item20, player.Center);
						nPC.AddBuff(20, 600, false);
						if(Main.netMode == 1)
						{ 
							nPC.netUpdate = true;
						}
						return true;
					}
				}
				return false;
			}
			return false;
		}
	}
}