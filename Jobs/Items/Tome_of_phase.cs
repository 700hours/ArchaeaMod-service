using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Buffs;

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
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    internal class Tome_of_phase : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Phase");
            Tooltip.SetDefault("Reappear in another location.\n" +
                "Costs 1/3 total mana.");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = 1;
            Item.useAnimation = 30;
            Item.useTime = 180;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 1;
            Item.rare = 4;
        }
        public override bool? UseItem(Player player)
        {
            Item.mana = player.statManaMax2 / 3;
            if (player.statMana >= player.statManaMax2 / 3)
			{
                player.AddBuff(ModContent.BuffType<Phase>(), Phase.MaxTime, Main.netMode == 1);
                player.statMana -= player.statManaMax2 / 3;
				player.manaRegenDelay = (int)player.maxRegenDelay;
                return true;
			}
			return false;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(ItemID.Blinkroot, 20)
                .AddIngredient(ItemID.HolyWater, 10)
                .AddIngredient(ItemID.BlackInk, 1)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}