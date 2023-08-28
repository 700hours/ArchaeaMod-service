using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Merged.Items.Materials;
namespace ArchaeaMod.Merged.Items
{
    public class cinnabar_pickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cinnabar Pickaxe");
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.scale = 1f;
            Item.useTime = 16;
            Item.useAnimation = 22;
            Item.useStyle = 1;
            //  item % power
            Item.pick = 95;
            Item.crit = 5;
            Item.damage = 13;
            Item.knockBack = 7f;
            Item.value = 1500;
            Item.rare = 2;
            //  custom sound?
            //  item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/*");
            //  or vanilla sound?
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.DamageType = DamageClass.Melee;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<magno_bar>(), 8)
                .AddIngredient(ModContent.ItemType<cinnabar_crystal>(), 6)
                .AddTile(TileID.Anvils)
//            recipe.SetResult(this, 1);
                .Register();
        }
    }
}
