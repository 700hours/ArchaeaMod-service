using ArchaeaMod.Items;

using ArchaeaMod.NPCs;
using ArchaeaMod.NPCs.Bosses;
using Humanizer;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.On;


using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    internal class Tome_of_summoning : ModItem
	{
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Tome of Summoning"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip0", "Displaces an NPC into your target. Costs 1/3 max mana."));
            tooltips.Add(new TooltipLine(Mod, "Tooltip1", "Requires line of sight with target."));
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
            Item.value = 2000;
            Item.rare = 3;
        }
        public override bool? UseItem(Player player)
        {
			Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X - (player.width/2), Main.mouseY + Main.screenPosition.Y - (player.height/2));
			NPC nPC = default;
            if (player.statMana >= player.statManaMax / 3)
            {
                int tries = 0;
                int count = Main.npc.Count(t => t.active && !t.friendly && !t.boss && !t.townNPC && !t.CountsAsACritter);
                do
                {
					bool any = Main.npc.Any(t => t.active && !t.friendly && !t.boss && !t.townNPC && !t.CountsAsACritter);
					if (!any)
					{
						return false;
					}
                    var n = Main.npc.Where(t => t.active && !t.boss && !t.townNPC && !t.CountsAsACritter).ToArray();
                    int len = n.Length;
                    nPC = n[Main.rand.Next(len)];
					if (nPC.active && !nPC.boss && !nPC.townNPC && !nPC.CountsAsACritter)
					{
						break;
					}
				} while (++tries < count);
                if (tries == count)
                { 
                    return false;
                }
				player.statMana -= player.statManaMax/3;
				player.manaRegenDelay = (int)player.maxRegenDelay;
                nPC.AddBuff(ModContent.BuffType<Buffs.Summoned>(), Buffs.Transmogrify.MaxTime, Main.netMode == 1);
                nPC.Center = mousev;
                if (Main.netMode != 0) nPC.netUpdate = true;
                SoundEngine.PlaySound(SoundID.Item25, mousev);
                //int CreatePlayered = NPC.NewNPC(Item.GetSource_ItemUse(Item), (int)mousev.X, (int)mousev.Y, m);
                #region dust
                Color newColor = default(Color);
				int a = Dust.NewDust(new Vector2(mousev.X - 10f, mousev.Y - 1f), 20, 2, 20, 0f, 0f, 100, newColor, 2.5f);
				int b = Dust.NewDust(new Vector2(mousev.X - 1f, mousev.Y - 10f), 2, 10, 20, 0f, 0f, 100, newColor, 2.5f);
				int c = Dust.NewDust(new Vector2(mousev.X-2, mousev.Y-2), 4, 4, 6, 0f, 0f, 100, newColor, 2f);
				Main.dust[a].noGravity = true;
				Main.dust[b].noGravity = true;
				Main.dust[c].noGravity = false;
                #endregion
                //Main.npc[CreatePlayered].AddBuff(ModContent.BuffType<Buffs.Summoned>(), Buffs.Summoned.MaxTime, Main.netMode == 1);
                //Main.npc[CreatePlayered].netUpdate = true;
                return true;
			}
			return false;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(ItemID.Deathweed, 10)
                .AddIngredient(ItemID.CorruptSeeds, 5)
                .AddIngredient(ItemID.BlackInk)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}