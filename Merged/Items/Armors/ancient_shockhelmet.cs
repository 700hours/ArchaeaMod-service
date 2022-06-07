using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class ancient_shockhelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Shock Mask");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
            // drawHair = true;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = 100;
            Item.rare = 3;
            Item.defense = 5;
        }
    }
}
