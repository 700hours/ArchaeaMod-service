using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace ArchaeaMod.Items
{
    public class flask_mercury : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flask of Mercury");
            // Tooltip.SetDefault("Adds mercury sickness on melee hit");
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.useTime = 15;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.maxStack = 20;
            Item.value = 800;
            Item.consumable = true;
        }
        public override bool CanUseItem(Player player)
        {
            SoundEngine.PlaySound(SoundID.Pixie, player.Center);
            player.AddBuff(ModContent.BuffType<Buffs.flask_mercury>(), 72000);
            if (Item.stack > 0)
            {
                Item.stack--;
                return true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.Bottles)
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ModContent.ItemType<Merged.Items.Materials.cinnabar_crystal>(), 3)
//            recipe.SetResult(this, 1);
                .Register();
        }
    }
}
