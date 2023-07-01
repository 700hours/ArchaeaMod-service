using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Global;
using ArchaeaMod.NPCs;
using Humanizer;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    internal class Seeded_dirt : ModItem
    {
        public override string Texture => "ArchaeaMod/Gores/Null";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soil Patch");
            Tooltip.SetDefault("Grows a random plant");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.useStyle = 1;
            Item.useAnimation = 20;
            Item.useTime = 15;
            Item.maxStack = 999;
            Item.createTile = -1;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.scale = 1;
            Item.value = 1000;
            Item.placeStyle = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ClayBlock)
                .AddIngredient(ItemID.DirtBlock)
                .AddIngredient(ItemID.MudBlock)
                .AddIngredient(ItemID.JungleGrassSeeds)
                .AddIngredient(ItemID.CorruptSeeds)
                .AddIngredient(ItemID.GrassSeeds)
                .AddCondition(Recipe.Condition.NearWater)
                .Register();
        }
    }
}
