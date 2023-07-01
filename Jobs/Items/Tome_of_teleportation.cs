using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Global;
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
    internal class Tome_of_teleportation : ModItem
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
                Vector2 tilev = new Vector2((Main.mouseX + Main.screenPosition.X - player.width) / 16, (Main.mouseY + Main.screenPosition.Y - player.height) / 16);
                Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X - (player.width / 2), Main.mouseY + Main.screenPosition.Y - (player.height / 2));
                if (!Collision.CanHitLine(mousev, 1, 1, player.Center, player.width, player.height))
                {
                    return false;
                }
				Color newColor = default(Color);
				int a = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 20, 0f, 0f, 100, newColor, 2.5f);
				Main.dust[a].noGravity = true;
                player.statMana -= player.statManaMax2 / 3;
				player.manaRegenDelay = (int)player.maxRegenDelay;
                return true;
			}
			return false;
		}
	}
}