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
            Item.width = 34;
            Item.height = 20;
            Item.rare = 3;
            Item.defense = 18;
            Item.value = 4000;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return
            head == ModContent.GetInstance<RustbaneHead>().Item &&
            body == Item &&
            legs == ModContent.GetInstance<RustbaneLegs>().Item;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Throwing) *= 1.20f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.MythrilAnvil)
                .AddIngredient(ModContent.ItemType<Items.Materials.r_plate>(), 15)
//            recipe.SetResult(this, 1);
                .Register();
        }
    }
}
