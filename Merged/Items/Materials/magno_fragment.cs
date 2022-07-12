using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items.Materials
{
    public class magno_fragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Fragment");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 30;
            Item.scale = 0.85f;
            Item.value = 2500;
            Item.maxStack = 999;
            Item.rare = 1;
        }
    }
}
