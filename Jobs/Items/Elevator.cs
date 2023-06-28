using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Global;
using ArchaeaMod.Mode;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Humanizer.In;
using static Humanizer.On;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    internal class Elevator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Elevator");
            Tooltip.SetDefault("Platform to stand on and grant vertical control");
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
            Item.useTurn = false;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 0;
            //  Create elevator tile
            //Item.createTile = -1
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 5)
                .AddIngredient(ItemID.Wood, 20)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
