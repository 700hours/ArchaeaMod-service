using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Items
{
    internal class IronWorkbench : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iron Workbench");
            Tooltip.SetDefault("Used to craft items from metal bars");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 14;
            Item.useStyle = 1;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.scale = 1;
            Item.value = 600;
            //  Create elevator tile
            //Item.createTile = -1
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 5)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.WorkBench)
                .AddIngredient(ItemID.IronAnvil)
                .AddTile(TileID.DemonAltar)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.WorkBench)
                .AddIngredient(ItemID.LeadAnvil)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
