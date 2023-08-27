using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Buffs;

using ArchaeaMod.Jobs.Projectiles;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Runtime.Intrinsics.X86;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static System.Formats.Asn1.AsnWriter;

namespace ArchaeaMod.Jobs.Items
{
    internal class Scroll_incognito : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scroll of Incognito");
            Tooltip.SetDefault("Put on a zombie disguise." +
                "Enemies do no damage for 2 minutes.");
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
                for (int i = 0; i < 20; i++)
                {
                    int index = Dust.NewDust(player.Center, 1, 1, DustID.GreenMoss, ArchaeaNPC.RandAngle() * 4f, ArchaeaNPC.RandAngle() * 4f, 0, default, 1.2f);
                    Main.dust[index].noGravity = false;
                    Main.dust[index].noLight = false;
                }
                player.AddBuff(ModContent.BuffType<Buffs.Zombie>(), Buffs.Zombie.MaxTime, Main.netMode == 1);
                SoundEngine.PlaySound(SoundID.ZombieMoan, player.Center);
                return true;
            }
            return false;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(ItemID.Shackle)
                .AddIngredient(ItemID.Deathweed, 2)
                .AddIngredient(ItemID.DemoniteOre)
                .AddTile(TileID.CrystalBall)
                .Register();
        }
    }
}