using ArchaeaMod.Effects;
using ArchaeaMod.Mode;
using ArchaeaMod.Progression;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{ 
	public class AnotherChance : ModItem
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Another Chance");
            /* Tooltip.SetDefault(
                "Increases extra life" +
                "\ncount by 1 (max 3)," +
                "\nand max life by 80."); */
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.useStyle = 4;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.maxStack = 10;
            Item.consumable = true;
            Item.autoReuse = false;
            Item.scale = 1;
            Item.UseSound = SoundID.Item4;
            Item.rare = 3;
            Item.value = 20000;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                var modPlayer = player.GetModPlayer<ArchaeaPlayer>();
                if (modPlayer.extraLife >= 3)
                {
                    return false;
                }
            }
            return false;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                var modPlayer = player.GetModPlayer<ArchaeaPlayer>();
                if (modPlayer.extraLife < 3) modPlayer.extraLife++;
                if (modPlayer.extraLife == 3)
                {
                    return false;
                }
            }
            return null;
        }
        public bool FakeUseLifeCrystal(Player Player)
        {
            if (ModContent.GetInstance<ModeToggle>().archaeaMode)
            {
                if (Player.statLifeMax < 9999)
                {
                    Player.statLifeMax += ArchaeaMode.LifeCrystal();
                    Player.statLifeMax = Math.Min(Player.statLifeMax, 9999);
                }
                else return false;
            }
            else
            {
                if (Player.statLifeMax < 400)
                {
                    Player.statLifeMax += 80;
                    Player.statLifeMax = Math.Min(Player.statLifeMax, 400);
                }
                else return false;
            }
            Player.ApplyItemAnimation(Item);
            SoundEngine.PlaySound(SoundID.Item4, Player.Center);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.PlayerLifeMana);
            return true;
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