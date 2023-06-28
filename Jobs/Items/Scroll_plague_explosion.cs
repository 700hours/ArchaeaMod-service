using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Buffs;
using ArchaeaMod.Jobs.Global;
using ArchaeaMod.Jobs.Projectiles;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Runtime.Intrinsics.X86;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    internal class Scroll_plague_explosion : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scroll of Plague Explosion");
        }
        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer)
			{ 
				Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
				Rectangle mouse = new Rectangle((int)(mousev.X - 16f), (int)(mousev.Y - 16f), 32, 32);
				NPC[] npc = Main.npc;
				for(int m = 0; m < npc.Length; m++)
				{
					NPC nPC = npc[m];
					if(!nPC.active) continue;
					if(nPC.life <= 0) continue;
					if(nPC.friendly) continue;
					if(nPC.dontTakeDamage) continue;
					if(nPC.boss) continue;
					if(!Collision.CanHitLine(nPC.Center, nPC.width, nPC.height, player.Center, player.width, player.height)) continue;
					Vector2 npcv = new Vector2(nPC.position.X, nPC.position.Y);
					Rectangle npcBox = new Rectangle((int)npcv.X, (int)npcv.Y, nPC.width, nPC.height);
					if (mouse.Intersects(npcBox) && Main.mouseLeft)
					{
						nPC.StrikeNPC(Item.damage, 0f, 0, fromNet: Main.netMode == 1);
						nPC.AddBuff(ModContent.BuffType<Plague>(), 300, Main.netMode == 0);
						for (int i = 0; i < 10; i++)
						{
							int index = Dust.NewDust(player.position, player.width, player.height, DustID.AncientLight, ArchaeaNPC.RandAngle() * 4f, ArchaeaNPC.RandAngle() * 4f, 0, default, 2f);
							Main.dust[index].noGravity = true;
						}
						for (int i = 0; i < 20; i++) 
						{
							int num54 = Projectile.NewProjectile(Projectile.GetSource_None(), npcv.X+(nPC.width/2), npcv.Y+(nPC.height/2), Main.rand.Next(10)-5, Main.rand.Next(10)-5, ModContent.ProjectileType<Projectiles.poison_diffusion>(), 5, 0, player.whoAmI);
							Main.projectile[num54].timeLeft = 90;
							Main.projectile[num54].tileCollide = false;
							Main.projectile[num54].ignoreWater = false;
						}
						if (Main.netMode == 1) 
						{
							nPC.netUpdate = true;
						}
						return true;
					}
				}
			}
			return false;
		}
	}
}