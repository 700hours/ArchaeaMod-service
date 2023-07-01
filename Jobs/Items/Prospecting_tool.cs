using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Global;
using ArchaeaMod.Mode;
using ArchaeaMod.NPCs;
using Humanizer;
using Microsoft.Xna.Framework;
using System.Runtime.Intrinsics.X86;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{ 
	internal class Prospecting_tool : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prospecting Tool");
            Tooltip.SetDefault("Use to locate ore within 100 tiles below you");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 1000;
            Item.rare = 2;
        }
        public override bool? UseItem(Player player)
        {
            Vector2 tilev = new Vector2(player.position.X/16, player.position.Y/16);
			for (int i = 0; i < 100; i++)
			{
				switch (Main.tile[(int)tilev.X, (int)tilev.Y + i].TileType)
				{ 
					case TileID.Platinum:
                        ModeUI.NewText($"{SetText(TileID.Platinum)} Ore: Yes", SetColor(200, 190, 140)); // Platinum
                        return true;
					case 8:
						ModeUI.NewText($"{SetText(8)} Ore: Yes", SetColor(200, 190, 140));			 // gold
						return true;
					case TileID.Tungsten:
                        ModeUI.NewText($"{SetText(TileID.Tungsten)} Ore: Yes", SetColor(200, 190, 140)); // Tungsten
                        return true;
					case 9:
						ModeUI.NewText($"{SetText(9)} Ore: Yes", SetColor(165, 165, 175));			 // silver
						return true;
                    case TileID.Lead:
                        ModeUI.NewText($"{SetText(TileID.Lead)} Ore: Yes", SetColor(200, 190, 140)); // Lead
                        return true;
                    case 6:
						ModeUI.NewText($"{SetText(6)} Ore: Yes", SetColor(200, 175, 140));		     // iron
						return true;
                    case TileID.Tin:
                        ModeUI.NewText($"{SetText(TileID.Tin)} Ore: Yes", SetColor(200, 190, 140));  // Tin
                        return true;
                    case 7:
						ModeUI.NewText($"{SetText(7)} Ore: Yes", SetColor(255, 170, 120));		     // copper
						return true;
					default:
                        ModeUI.NewText($"{SetText(default)} Ore", SetColor(255, 255, 255));			 // none
                        return true;
				}
			}
			return false;
		}
        Color SetColor(byte r, byte g, byte b)
        {
            return Color.FromNonPremultiplied(r, g, b, 0);
        }
        string SetText(int type)
        {
            switch (type)
            {
                case 7:
                    return "Copper";
                case 6:
                    return "Iron";
                case 9:
                    return "Silver";
                case 8:
                    return "Gold";
                case TileID.Tin:
                    return "Tin";
                case TileID.Lead:
                    return "Lead";
                case TileID.Tungsten:
                    return "Tungsten";
                case TileID.Platinum:
                    return "Platinum";
                default:
                    return "No";
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GPS)
                .AddIngredient(ItemID.Compass)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}