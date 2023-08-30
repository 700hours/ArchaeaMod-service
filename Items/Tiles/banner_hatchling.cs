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
    public class banner_hatchling : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Hatchling banner"));
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.value = 1000;
            Item.maxStack = 99;
            Item.rare = 2;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ArchaeaMod.Tiles.banners>();
        }
    }
}
