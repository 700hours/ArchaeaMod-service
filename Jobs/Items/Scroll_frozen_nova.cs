using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Global;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Runtime.Intrinsics.X86;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static IL.Terraria.WorldBuilding.Searches;
using static System.Formats.Asn1.AsnWriter;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ArchaeaMod.Jobs.Items
{
    internal class Scroll_frozen : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scrollof Frozen Nova");
            Tooltip.SetDefault("Create a blast of frost around you.\n" +
                "One use.");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = 2;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.damage = 40;
            Item.maxStack = 10;
            Item.consumable = true;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 0;
            Item.rare = 2;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            { 
                for (int i = 0; i < 10; i++)
                {
                    int index = Dust.NewDust(player.position, player.width, player.height, DustID.AncientLight, ArchaeaNPC.RandAngle() * 4f, ArchaeaNPC.RandAngle() * 4f, 0, default, 2f);
                    Main.dust[index].noGravity = true;
                }
                ArchaeaItem.ProjectileCircle(Main.MouseWorld, Item.damage, ModContent.ProjectileType<Projectiles.Frost>(), 20);
				return true;
            }
            return false;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(ItemID.Waterleaf, 4)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}