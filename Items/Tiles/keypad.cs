using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items.Tiles
{
    internal class keypad : ModItem
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.Timer5Second}";
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //tooltips.Add(new TooltipLine(Mod, "ItemName", "Magnoliac Biome"));
            //tooltips.Add(new TooltipLine(Mod, "Tooltip0", "R.A."));
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 30;
            Item.useAnimation = 35;
            Item.useStyle = 1;
            Item.value = 500;
            Item.maxStack = 99;
            Item.rare = 2;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ArchaeaMod.Structure.Keypad>();
        }
    }
}
