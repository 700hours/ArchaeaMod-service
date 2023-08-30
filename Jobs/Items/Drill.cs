using ArchaeaMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Items
{
    public class Drill : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Drill");
            // Tooltip.SetDefault("Only digs vertically and breaks two tiles at once");
        }
        public override void SetDefaults()
        {
			Item.width = 30;
			Item.height = 40;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 25;
			Item.useTime = 7;
			Item.maxStack = 1;
			Item.pick = 180;
			Item.damage = 20;
			Item.scale = 1;
			Item.rare = 4;
			Item.autoReuse = true;
			Item.noUseGraphic = false;
			Item.noMelee = true;
			Item.value = 108000;
        }
		int ticks = 0;
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer)
			{
				for (int i = (int)(player.position.X)/16; i < (int)(player.position.X+player.width+8f)/16; i++)
				for (int j = (int)player.position.Y/16; j < (int)(player.position.Y+player.height+18f)/16; j++)
				if (Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType] && (Main.tile[i, j].TileType == 0 || Main.tile[i, j].TileType == 1 || Main.tile[i, j].TileType == 2 || Main.tile[i, j].TileType == 3 || Main.tile[i, j].TileType == 5 || Main.tile[i, j].TileType == 7 || Main.tile[i, j].TileType == 8 || Main.tile[i, j].TileType == 9 || Main.tile[i, j].TileType == 19 ||Main.tile[i, j].TileType == 23 || Main.tile[i, j].TileType == 30 || Main.tile[i, j].TileType == 32 || Main.tile[i, j].TileType == 40 || Main.tile[i, j].TileType == 45 || Main.tile[i, j].TileType == 46 || Main.tile[i, j].TileType == 47 || Main.tile[i, j].TileType == 53 || Main.tile[i, j].TileType == 54 || Main.tile[i, j].TileType == 57 || Main.tile[i, j].TileType == 59 || Main.tile[i, j].TileType == 60 || Main.tile[i, j].TileType == 69 || Main.tile[i, j].TileType == 70 || Main.tile[i, j].TileType == 72 || Main.tile[i, j].TileType == 109 || Main.tile[i, j].TileType == 112 || Main.tile[i, j].TileType == 116 || Main.tile[i, j].TileType == 120 || Main.tile[i, j].TileType == 123 || Main.tile[i, j].TileType == 124 || Main.tile[i, j].TileType == 130 || Main.tile[i, j].TileType == 131 || Main.tile[i, j].TileType == 145 || Main.tile[i, j].TileType == 146 || Main.tile[i, j].TileType == 147 || Main.tile[i, j].TileType == 148))
				{
					if (ticks++ % 14 == 0)
					{ 
						WorldGen.KillTile(i, j);
						if (Main.netMode == 1)
						{
                            NetMessage.SendTileSquare(player.whoAmI, i, j);
						}
					}
				}
				if (player.runSoundDelay <= 0)
				{
					SoundEngine.PlaySound(SoundID.Item22, player.Center);
					player.runSoundDelay = 30;
				}
				if (Main.rand.NextBool(6))
				{
					int num123 = Dust.NewDust(player.position + player.velocity * (float)Main.rand.Next(6, 10) * 0.1f, player.width, player.height, 31, 0f, 0f, 80, default(Color), 1.4f);
					Dust expr_5B99_cp_0 = Main.dust[num123];
					expr_5B99_cp_0.position.X = expr_5B99_cp_0.position.X - 4f;
					Main.dust[num123].noGravity = true;
					Main.dust[num123].velocity *= 0.2f;
					Main.dust[num123].velocity.Y = (float)(-(float)Main.rand.Next(7, 13)) * 0.15f;
				}
			}
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AdamantiteBar, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.MythrilBar, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.CobaltBar, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 24)
                .AddIngredient(ItemID.IronBar, 6)
                .AddTile(TileID.DemonAltar)
				.AddCondition(new Condition(LocalizedText.Empty, delegate() { return Main.hardMode; }))
                .Register();
        }
    }
}