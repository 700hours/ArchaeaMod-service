using ArchaeaMod.Items;

using ArchaeaMod.NPCs;
using Humanizer;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Drawing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    internal class Textbook_of_gathering : ModItem
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Textbook of Gathering");
			/* Tooltip.SetDefault("If held and on a grassy area, you will collect acorns.\n" +
				"Also a chance, if on stone, to collect stone."); */
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 60;
            Item.useTime = 1;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 300;
            Item.rare = 2;
        }
        public override bool? UseItem(Player player)
        {
			Vector2 tilev = new Vector2((int)player.position.X/16, (int)(player.position.Y+player.height-2)/16);
			if(Main.tile[(int)tilev.X, (int)tilev.Y+1].TileType == 2)
			{
				if(Main.rand.NextBool(24)) 
				{
					int acorn = Item.NewItem(Item.GetSource_DropAsItem(), (int)player.position.X,(int)player.position.Y,32,32,ItemID.Acorn,1,false);
					if(Main.netMode == 1)
					{
						NetMessage.SendData(21, -1, -1, null, acorn, 0f, 0f, 0f, 0);
					}
				}
				if(Main.rand.NextBool(128))
				{
					WorldGen.KillTile((int)tilev.X, (int)tilev.Y+1, false, false, true);
					if (Main.netMode == 1)
					{ 
						NetMessage.SendTileSquare(player.whoAmI, (int)tilev.X, (int)tilev.Y);
					}
				}
			}
			if(Main.tile[(int)tilev.X, (int)tilev.Y+1].TileType == 1)
			{
				if(Main.rand.NextBool(96))
				{
					int stone = Item.NewItem(Item.GetSource_DropAsItem(), (int)player.position.X,(int)player.position.Y,32,32,ItemID.StoneBlock,1,false);
					if (Main.netMode == 1)
					{ 
						NetMessage.SendData(21, -1, -1, null, stone, 0f, 0f, 0f, 0);
					}
				}
				if(Main.rand.NextBool(512))
				{
					WorldGen.KillTile((int)tilev.X, (int)tilev.Y + 1, false, false, true);
                    if (Main.netMode == 1)
                    {
                        NetMessage.SendTileSquare(player.whoAmI, (int)tilev.X, (int)tilev.Y);
                    }
                }
			}
			return null;
		}
        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemID.Book)
				.AddIngredient(ItemID.Acorn, 50)
				.AddIngredient(ItemID.StoneBlock, 50)
				.AddTile(TileID.Bookcases)
				.Register();
        }
    }
}