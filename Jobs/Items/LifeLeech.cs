using ArchaeaMod.Items;

using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    internal class LifeLeech : ModItem
	{
        public override void SetStaticDefaults()
        {
		//	Blessed Ankh casts Life Leech
            DisplayName.SetDefault("Life Leech");
            Tooltip.SetDefault("Absorb life from your enemies\n" +
				"1 mana per 1 life");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = 1;
            Item.useAnimation = 20;
            Item.useTime = 3;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 0;
            Item.rare = 2;
        }
        public override bool? UseItem(Player player)
        {
			if (player.statMana <= 0)
			{
				return false;
			}
            if (player.statLife > player.statLifeMax)
            {
                player.statLife = player.statLifeMax;
                return false;
            }
            Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y );
			Rectangle mouse = new Rectangle((int)(mousev.X - 16f), (int)(mousev.Y - 16f), 32, 32);
			NPC[] npc = Main.npc;
			for(int m = 0; m < npc.Length-1; m++)
			{
				NPC nPC = npc[m];
				Vector2 npcv = new Vector2(nPC.position.X, nPC.position.Y);
				Rectangle npcBox = new Rectangle((int)npcv.X, (int)npcv.Y, nPC.width, nPC.height);
				if(Collision.CanHitLine(nPC.Center, nPC.width, nPC.height, player.Center, player.width, player.height) && mouse.Intersects(npcBox) && !nPC.boss && player.statMana > 0 && Main.mouseLeft)
				{
					if(player.statLife != player.statLifeMax)
					{
						player.statLife++;
						nPC.life--;
						if(nPC.life <= 5)
						{
							nPC.StrikeNPC(6, 0f, player.direction, true, false, Main.netMode == 1);
						}
						player.statMana--;
						player.manaRegenDelay = (int)player.maxRegenDelay;
						Color newColor = default(Color);
						int a = Dust.NewDust(new Vector2(nPC.position.X, nPC.position.Y), nPC.width, nPC.height, 5, 0f, 0f, 100, newColor, 1f);
						Main.dust[a].noGravity = false;			   
						Vector2 speed = ArchaeaNPC.AngleToSpeed(nPC.AngleTo(player.Center), 8f);
						int b = Dust.NewDust(nPC.Center, 1, 1, 5, speed.X, speed.Y, 0, default, 3f);
						Main.dust[b].noGravity = true;
						SoundEngine.PlaySound(SoundID.Item39, player.Center);
                        nPC.netUpdate = true;
                        if (Main.netMode == 1) 
						{
                            NetMessage.SendData(16, player.whoAmI);
							//NetMessage.SendData(23, -1, -1, null, nPC.whoAmI);
						}
						int index = Dust.NewDust(player.position + new Vector2(player.width / 2, player.height - 1), 1, 1, DustID.AncientLight, ArchaeaNPC.RandAngle() * 3f, 0f, 0, default, 1f);
                        Main.dust[index].noGravity = true;
						return true;
					}
				}
			}
			return null;
		}
        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemID.Book)
				.AddIngredient(ItemID.Deathweed, 25)
				.AddIngredient(ItemID.RottenChunk)
				.AddTile(ItemID.CrystalBall)
				.Register();
            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(ItemID.Deathweed, 25)
                .AddIngredient(ItemID.Vertebrae)
                .AddTile(ItemID.CrystalBall)
                .Register();
        }
    }
}