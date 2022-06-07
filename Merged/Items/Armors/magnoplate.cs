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
    public class magnoplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Breastplate");
            Tooltip.SetDefault("7% increased damage");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = 100;
            Item.rare = 2;
            Item.defense = 7;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<magno_bar>(), 20)
                .AddIngredient(ModContent.ItemType<magno_fragment>(), 16)
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
