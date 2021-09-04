using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class RustbanePlate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rustbane Chestplate");
            Tooltip.SetDefault("20% increased throwing damage");
        }
        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 20;
            item.rare = 3;
            item.defense = 18;
            item.value = 4000;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return
            head == ModContent.GetInstance<RustbaneHead>().item &&
            body == item &&
            legs == ModContent.GetInstance<RustbaneLegs>().item;
        }
        public override void UpdateEquip(Player player)
        {
            player.thrownDamage *= 1.20f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.r_plate>(), 15);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
