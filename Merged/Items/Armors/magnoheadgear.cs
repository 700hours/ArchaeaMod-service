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
    public class magnoheadgear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Headgear");
            Tooltip.SetDefault("10% increased arrow damage");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = 100;
            Item.rare = 2;
            Item.defense = 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<magno_bar>(), 10)
                .AddIngredient(ModContent.ItemType<magno_fragment>(), 8)
                .AddTile(TileID.Anvils)
//            recipe.SetResult(this, 1);
                .Register();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == Mod.Find<ModItem>("magnoplate").Type && legs.type == Mod.Find<ModItem>("magnogreaves").Type;
        }
        public override void UpdateEquip(Player player)
        {
            player.arrowDamage /= 0.90f;
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "15% chance for arrows to explode";
        }
    }
}
