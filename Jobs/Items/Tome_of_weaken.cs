using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Buffs;
using ArchaeaMod.Jobs.Global;
using ArchaeaMod.Jobs.Projectiles;
using ArchaeaMod.NPCs;
using ArchaeaMod.NPCs.Bosses;
using Humanizer;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Diagnostics;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.On;
using static IL.Terraria.ID.ArmorIDs;
using static On.Terraria.ID.ArmorIDs;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    internal class Tome_of_weaken : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Weaken");
			Tooltip.SetDefault("Weakens an enemy.");
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
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 1000;
            Item.rare = 3;
        }
        public override bool? UseItem(Player player)
        {
            Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
			Rectangle mouse = new Rectangle((int)(mousev.X - 16f), (int)(mousev.Y - 16f), 32, 32);
			NPC[] npc = Main.npc;
			if(player.statMana >= 7)
			{
				for(int m = 0; m < npc.Length-1; m++)
				{
					NPC nPC = npc[m];
					if(!nPC.active) continue;
					if(nPC.life <= 0) continue;
					if(nPC.friendly) continue;
					if(nPC.dontTakeDamage) continue;
					if(nPC.boss) continue;
					Vector2 npcv = new Vector2(nPC.position.X, nPC.position.Y);
					Rectangle npcBox = new Rectangle((int)npcv.X, (int)npcv.Y, nPC.width, nPC.height);
					if (mouse.Intersects(npcBox) && Main.mouseLeft)
					{
						player.statMana -= 5;
						player.manaRegenDelay = (int)player.maxRegenDelay;
						Color newColor = default(Color);
						int a = Dust.NewDust(new Vector2(mousev.X - 10f, mousev.Y - 10f), 20, 20, 19, 0f, 0f, 100, newColor, 2f);
						Main.dust[a].noGravity = true;
                        SoundEngine.PlaySound(SoundID.Item20, player.Center);
                        int proj = Projectile.NewProjectile(Item.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<t_effect>(), 0, 0f, Main.myPlayer, 0, nPC.whoAmI);
                        Main.projectile[proj].localAI[0] = ModContent.BuffType<Weaken>();
                        Main.projectile[proj].localAI[1] = DustID.PinkTorch;
                        return true;
					}
				}
			}
			return false;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(ItemID.Daybloom, 5)
                .AddIngredient(ItemID.Deathweed, 5)
                .AddIngredient(ItemID.BlackInk)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}