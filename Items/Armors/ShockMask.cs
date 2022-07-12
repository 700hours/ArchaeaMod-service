using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace ArchaeaMod.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class ShockMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shock Mask");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
            // drawHair = true;
            // drawAltHair = false;
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 12;
            Item.defense = 7;
            Item.value = 5000;
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.Autohammer)
                .AddIngredient(ModContent.ItemType<Items.Materials.r_plate>(), 10)
                .Register();
        }
    }
}
