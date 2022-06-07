using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace ArchaeaMod.Items.Tiles
{
    public class m_door : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnoliac door");
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 34;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.value = 0;
            Item.rare = 1;
            Item.maxStack = 99;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ArchaeaMod.Tiles.m_doorclosed>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Merged.Items.Tiles.magno_brick>(), 6)
                .AddIngredient(ModContent.ItemType<Merged.Items.Materials.magno_bar>(), 2)
                .AddTile(TileID.Anvils)
//            recipe.SetResult(Item.type);
                .Register();
        }
    }
}
