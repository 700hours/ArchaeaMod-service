using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items.Materials
{
    public class magno_core : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scorched Core");
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 32;
            Item.scale = 1f;
            Item.maxStack = 999;
            Item.value = 3500;
            Item.rare = 3;
        }
    }
}
