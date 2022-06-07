using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Merged.Items.Materials;
namespace ArchaeaMod.Merged.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class cinnabarplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cinnabar Breastplate");
            Tooltip.SetDefault("10% increased damage");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = 100;
            Item.rare = 3;
            Item.defense = 7;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<magno_bar>(), 15)
                .AddIngredient(ModContent.ItemType<cinnabar_crystal>(), 16)
                .AddTile(TileID.Anvils)
//            recipe.SetResult(this, 1);
                .Register();
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) /= 0.93f;
            player.GetDamage(DamageClass.Ranged) /= 0.93f;
            player.GetDamage(DamageClass.Magic) /= 0.93f;
            player.GetDamage(DamageClass.Throwing) /= 0.93f;
            player.GetDamage(DamageClass.Summon) /= 0.93f;
        }
    }
}
