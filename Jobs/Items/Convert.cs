using ArchaeaMod.Jobs.Global;
using Humanizer;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Runtime.Intrinsics.X86;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{ 
	public class Convert : ModItem
	{
        //  The power to turn any NPC into a follower
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Convert");
			Tooltip.SetDefault("Use to convert friends or foes");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = 1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.noMelee = true;
			Item.mana = 35;
            Item.scale = 1;
            Item.value = 3000;
            Item.rare = 2;
        }
        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer)
			{ 
				Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y );
				Rectangle mouse = new Rectangle((int)(mousev.X - 16f), (int)(mousev.Y - 16f), 32, 32);
				NPC[] npc = Main.npc;
				for(int m = 0; m < npc.Length-1; m++)
				{
					NPC nPC = npc[m];
					Vector2 npcv = new Vector2(nPC.position.X, nPC.position.Y);
					Rectangle npcBox = new Rectangle((int)npcv.X, (int)npcv.Y, nPC.width, nPC.height);
					if(Collision.CanHitLine(nPC.Center, nPC.width, nPC.height, player.Center, player.width, player.height) && mouse.Intersects(npcBox) && !nPC.boss && player.statMana >= 35 && Main.mouseLeft)
					{
						nPC.friendly = !nPC.friendly;
						Color newColor = default(Color);
						int a = Dust.NewDust(new Vector2(mousev.X - 10f, mousev.Y - 10f), 20, 20, 20, 0f, 0f, 100, newColor, 2f);
						Main.dust[a].noGravity = true;
						if (Main.netMode == 1) 
						{
							nPC.netUpdate = true;
						}
                        break;
					}
				}
			}
			return null;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.CrystalBall)
                .AddIngredient(ItemID.Book, 1)
                .AddIngredient(ItemID.Sunflower, 10)
                .AddIngredient(ItemID.DemoniteOre)
                .Register();
        }
    }
}