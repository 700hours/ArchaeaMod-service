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
    public class m_biomepainting : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Magnoliac Biome"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip0", "R.A."));
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 32;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.value = 5000;
            Item.maxStack = 99;
            Item.rare = 3;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ArchaeaMod.Tiles.paintings>();
        }
    }
}
