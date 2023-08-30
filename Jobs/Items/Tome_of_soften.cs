using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Items
{
    internal class Tome_of_soften : ModItem
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tome of Soften");
			/* Tooltip.SetDefault("Weakens an enemy's defenses, including bosses.\n" +
				"Costs mana directionally proportional to NPC size."); */
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 2000;
            Item.rare = 3;
        }
        int ticks = 0;
        public override bool? UseItem(Player player)
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
                Vector2 npcv = new Vector2(nPC.position.X, nPC.position.Y);
				Rectangle npcBox = new Rectangle((int)npcv.X, (int)npcv.Y, nPC.width, nPC.height);
				if(mouse.Intersects(npcBox) && player.statMana >= nPC.width/4 && Main.mouseLeft)
				{
                    int cost = nPC.width / 4;
                    if (cost > player.statManaMax2)
                    {
                        cost = player.statManaMax2;
                    }
                    if (player.statMana < cost)
                    {
                        return false;
                    }
					player.statMana -= cost;
					player.manaRegenDelay = (int)player.maxRegenDelay;
					Color newColor = default(Color);
                    if (ticks++ % 5 == 0)
                    {
					    int a = Dust.NewDust(new Vector2(mousev.X - 10f, mousev.Y - 10f), nPC.width, nPC.height, 19, 0f, 0f, 100, newColor, 2f);
					    Main.dust[a].noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.Item8, mousev);
                    ArchaeaNPC.AddBuffNetNPC(nPC, ModContent.BuffType<Buffs.Soft>(), Buffs.Soft.MaxTime);
					return true;
				}
			}
			return false;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(ItemID.Daybloom, 10)
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.BlackInk, 1)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}