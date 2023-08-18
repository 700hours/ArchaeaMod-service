using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Buffs;

using ArchaeaMod.Jobs.Projectiles;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using tUserInterface.ModUI;
using static Humanizer.On;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    internal class Scroll_plague_explosion : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Plague Explosion");
			Tooltip.SetDefault("Strikes an enemy with poison.\n" +
				"One use.");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 30;
            Item.useTime = 30;
			Item.mana = 
			Item.damage = 45;
            Item.maxStack = 10;
			Item.consumable = true;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 5000;
            Item.rare = 3;
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
							int num54 = Projectile.NewProjectile(Projectile.GetSource_None(), npcv.X+(nPC.width/2), npcv.Y+(nPC.height/2), Main.rand.Next(10)-5, Main.rand.Next(10)-5, ModContent.ProjectileType<Projectiles.diffusion>(), 5, 0, player.whoAmI, 61, ModContent.BuffType<Plague>());
							Main.projectile[num54].timeLeft = 90;
							Main.projectile[num54].tileCollide = false;
							Main.projectile[num54].ignoreWater = false;
							Main.projectile[num54].localAI[0] = 300;
							Main.projectile[num54].localAI[1] = 1;
                        }
						if (Main.netMode == 1) 
						{
						 	nPC.netUpdate = true;
						}
						SoundEngine.PlaySound(SoundID.Item8, mousev);
						return true;
					}
				}
			}
			return false;
		}
        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemID.SpellTome)
				.AddIngredient(ModContent.ItemType<Scroll_plague_explosion>(), 3)
				.AddIngredient(ItemID.Fireblossom, 5)
				.AddTile(TileID.CrystalBall)
				.Register();
        }
    }
}