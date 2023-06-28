using ArchaeaMod.Effects;
using ArchaeaMod.Jobs.Global;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{ 
	public class AnotherChance : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Another Chance");
            Tooltip.SetDefault(
                "Increases extra life" +
                "\ncount by 1 (max 3)," +
                "\nand max life by 20.");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.useStyle = 4;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.maxStack = 10;
            Item.consumable = true;
            Item.scale = 1;
            Item.UseSound = SoundID.Item4;
            Item.rare = 3;
            Item.value = 20000;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                var modPlayer = player.GetModPlayer<ArchaeaPlayer>();
                modPlayer.FakeUseLifeCrystal(Item);
                if (modPlayer.extraLife < 3) modPlayer.extraLife++;
                if (modPlayer.extraLife == 3)
                {
                    return false;
                }
            }
            return null;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ItemID.LifeCrystal, 4)
                .Register();
            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ItemID.HealingPotion, 40)
                .AddIngredient(ItemID.DemoniteBar, 10)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}