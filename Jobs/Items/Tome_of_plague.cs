using ArchaeaMod.Jobs.Buffs;

using ArchaeaMod.Jobs.Projectiles;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    internal class Tome_of_plague : ModItem
	{
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Costs 10 mana"));
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = 1;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.consumable = false;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 3000;
            Item.rare = 3;
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
					if (mouse.Intersects(npcBox) && Main.mouseLeft)
					{
                        for (int i = 0; i < 10; i++)
                        {
                            int d = Dust.NewDust(player.position, player.width, player.height, DustID.AncientLight, ArchaeaNPC.RandAngle() * 4f, ArchaeaNPC.RandAngle() * 4f, 0, default, 2f);
                            Main.dust[d].noGravity = true;
                        }
                        int index = Dust.NewDust(player.position + new Vector2(player.width / 2, player.height - 1), 1, 1, DustID.AncientLight, ArchaeaNPC.RandAngle() * 3f, 0f, 0, default, 1f);
                        Main.dust[index].noGravity = true;
                        Color newColor = default(Color);
						int a = Dust.NewDust(player.position, 20, 20, 61, 0f, 0f, 100, newColor, 2f);
						Main.dust[a].noGravity = true;
						SoundEngine.PlaySound(SoundID.Item20, player.Center);
						int proj = Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<t_effect>(), 0, 0f, Main.myPlayer, 0, nPC.whoAmI);
						Main.projectile[proj].localAI[0] = ModContent.BuffType<Plague>();
                        Main.projectile[proj].localAI[1] = DustID.GreenTorch;
                        player.statMana -= 10;
					}
				}
			}
			return true;
		}
        public override void AddRecipes()
        {	
			CreateRecipe()
				.AddIngredient(ItemID.Book)
				.AddIngredient(ItemID.Deathweed, 20)
				.AddIngredient(ItemID.BlackInk)
				.AddTile(TileID.Bookcases)
				.Register();
        }
    }
}