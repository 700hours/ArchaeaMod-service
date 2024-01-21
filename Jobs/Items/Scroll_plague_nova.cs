using ArchaeaMod.Items;
using ArchaeaMod.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Items
{ 
    internal class Scroll_plague_nova : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.damage = 60;
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
                for (int i = 0; i < 10; i++)
                {
                    int index = Dust.NewDust(player.position, player.width, player.height, DustID.AncientLight, ArchaeaNPC.RandAngle() * 4f, ArchaeaNPC.RandAngle() * 4f, 0, default, 2f);
                    Main.dust[index].noGravity = true;
                }
                ArchaeaItem.ProjectileCircle(Main.MouseWorld, Item.damage, ModContent.ProjectileType<Projectiles.diffusion>(), 20, 61, ModContent.BuffType<Buffs.Plague>(), 300, 0);
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(ItemID.Deathweed, 4)
                .AddIngredient(ItemID.Meteorite)
                .AddTile(TileID.CrystalBall)
                .Register();
        }
    }
}