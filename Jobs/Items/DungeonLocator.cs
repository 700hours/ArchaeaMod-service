using ArchaeaMod.Items;

using ArchaeaMod.Mode;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Humanizer.In;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    public class DungeonLocator : ModItem
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dungeon Locator");
            /* Tooltip.SetDefault("Use to locate the dungeon\n" +
                "One use"); */
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = 1;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.maxStack = 1;
            Item.consumable = true;
            Item.autoReuse = false;
            Item.useTurn = true;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 0;
        }
        public override bool CanUseItem(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item4, player.Center);
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            { 
                var modPlayer = player.GetModPlayer<ArchaeaPlayer>();
                modPlayer.locatorDirection = Main.dungeonX * 16 < player.position.X ? -1 : 1;
                modPlayer.dungeonLocatorTicks = 1;
            }
            return null;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Compass)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}