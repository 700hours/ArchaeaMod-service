using ArchaeaMod.Jobs.Global;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Drawing;
using System.Runtime.Intrinsics.X86;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static System.Formats.Asn1.AsnWriter;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ArchaeaMod.Jobs.Items
{
    internal class Scroll_fortitude : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scroll of Fortitude");
			Tooltip.SetDefault("Increases a player's defense by 20.\n" +
				"Strengthens melee damage by 3x.");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.maxStack = 10;
            Item.consumable = true;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 0;
            Item.rare = 2;
        }
        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer)
			{ 
				Vector2 mousev = Main.MouseWorld;
				Player[] plr = Main.player;
                for (int m = 0; m < plr.Length-1; m++)
				{
					Player P = plr[m];
					if (!P.active) continue;
					if (P.statLife <= 0) continue;
					if (P.HasBuff(ModContent.BuffType<Buffs.Fortitude>())) continue;
					if (!P.Hitbox.Contains(mousev.ToPoint())) continue;
					Vector2 pv = new Vector2(P.position.X, P.position.Y);
					if (Main.mouseLeft)
					{
						for (int i = 0; i < 10; i++)
						{
							int index = Dust.NewDust(player.position, player.width, player.height, DustID.AncientLight, ArchaeaNPC.RandAngle() * 4f, ArchaeaNPC.RandAngle() * 4f, 0, default, 2f);
							Main.dust[index].noGravity = true;
						}
                        for (int i = 0; i < 10; i++)
                        {
                            int index = Dust.NewDust(pv, P.width, P.height, DustID.Blood, 0f, 0f, 0, default, 2f);
                            Main.dust[index].noGravity = true;
                        }
                        P.AddBuff(ModContent.BuffType<Buffs.Fortitude>(), Buffs.Fortitude.MaxTime, Main.netMode == 1);
						SoundEngine.PlaySound(SoundID.Item8, player.Center);
						return true;
					}
				}
                NPC[] npc = Main.npc;
                for (int m = 0; m < npc.Length - 1; m++)
                {
                    NPC nPC = npc[m];
                    if (!nPC.active) continue;
                    if (nPC.life <= 0) continue;
                    if (nPC.HasBuff(ModContent.BuffType<Buffs.Fortitude>())) continue;
                    if (!nPC.Hitbox.Contains(mousev.ToPoint())) continue;
                    if (!nPC.townNPC) continue;
                    Vector2 pv = new Vector2(nPC.position.X, nPC.position.Y);
                    if (Main.mouseLeft)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            int index = Dust.NewDust(player.position, player.width, player.height, DustID.AncientLight, ArchaeaNPC.RandAngle() * 4f, ArchaeaNPC.RandAngle() * 4f, 0, default, 2f);
                            Main.dust[index].noGravity = true;
                        }
                        for (int i = 0; i < 10; i++)
                        {
                            int index = Dust.NewDust(pv, nPC.width, nPC.height, DustID.Blood, 0f, 0f, 0, default, 2f);
                            Main.dust[index].noGravity = true;
                        }
                        nPC.AddBuff(ModContent.BuffType<Buffs.Fortitude>(), Buffs.Fortitude.MaxTime, Main.netMode == 1);
                        SoundEngine.PlaySound(SoundID.Item8, player.Center);
                        return true;
                    }
                }
			}
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(ItemID.IronskinPotion)
                .AddIngredient(ItemID.IronOre)
                .AddTile(TileID.CrystalBall)
                .Register();
        }
    }
}