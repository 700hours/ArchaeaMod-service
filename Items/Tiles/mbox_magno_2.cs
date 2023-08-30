using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items.Tiles
{
    public class mbox_magno_2 : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Magnoliac Music Box Alt"));
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 3500;
            Item.rare = 3;
            Item.useStyle = 1;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ArchaeaMod.Tiles.music_boxes_alt>();
            Item.placeStyle = 1;
        }
    }
}
