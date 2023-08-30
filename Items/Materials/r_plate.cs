using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items.Materials
{
    public class r_plate : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Rusty Plate"));
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = 1500;
        }
    }
}
