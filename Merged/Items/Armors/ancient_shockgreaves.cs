using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class ancient_shockgreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Shock Greaves");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 100;
            Item.rare = 3;
            Item.defense = 3;
        }
    }
}
