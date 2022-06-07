using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class magno_mask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Mask");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = 1000;
            Item.rare = 2;
            Item.defense = 0;
            Item.vanity = true;
        }
    }
}
