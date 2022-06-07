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
    [AutoloadEquip(EquipType.Head)]
    public class RustbaneHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rustbane Visor");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false;
            // drawHair = false;
            // drawAltHair = false;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.rare = 3;
            Item.defense = 12;
            Item.value = 2500;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.MythrilAnvil)
                .AddIngredient(ModContent.ItemType<Items.Materials.r_plate>(), 10)
                .Register();
        }
    }
}
