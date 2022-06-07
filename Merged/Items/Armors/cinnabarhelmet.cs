using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Merged.Items.Materials;
namespace ArchaeaMod.Merged.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class cinnabarhelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cinnabar Helmet");
            Tooltip.SetDefault("Increases rate of" 
                +   "\nhealing");
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
                .AddIngredient(ModContent.ItemType<magno_bar>(), 10)
                .AddIngredient(ModContent.ItemType<cinnabar_crystal>(), 8)
                .AddTile(TileID.Anvils)
//            recipe.SetResult(this, 1);
                .Register();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<cinnabarplate>() && legs.type == ModContent.ItemType<cinnabargreaves>();
        }
        public override void UpdateEquip(Player player)
        {
            player.lifeRegen += 2;
            player.lifeRegenTime++;
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "20% increased"
                +   "\nmelee speed";
            player.GetAttackSpeed(DamageClass.Melee) /= 0.80f;
        }
    }
}
