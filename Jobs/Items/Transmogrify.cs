
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.On;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    internal class Transmogrify : ModItem
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Transmogrify");
			// Tooltip.SetDefault("Cost 1/3 max mana");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = 1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 0;
            Item.rare = 2;
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
				if(mouse.Intersects(npcBox) && !nPC.boss && !nPC.townNPC && player.statMana >= player.statManaMax/3 && Main.mouseLeft)
				{
                    do
                    {
                        var n = npc.Where(t => t.active && !t.boss && !t.townNPC).ToArray();
                        int len = npc.Count(t => t.active && !t.boss && !t.townNPC);
                        nPC.Transform(n[Main.rand.Next(len)].type);
                    } while (nPC.boss);
					player.statMana -= player.statManaMax/3;
					player.manaRegenDelay = (int)player.maxRegenDelay;
					if(Main.netMode == 1)
                    {
                        nPC.netUpdate = true;
                    }
                    return true;
				}
			}
			return false;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(ItemID.Deathweed, 15)
                .AddIngredient(ItemID.CorruptSeeds, 2)
                .AddIngredient(ItemID.BlackInk)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}