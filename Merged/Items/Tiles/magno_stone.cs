using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items.Tiles
{
    public class magno_stone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Stone");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.scale = 1f;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.value = 0;
            Item.rare = 1;
            Item.maxStack = 999;
            Item.createTile = Mod.Find<ModTile>("m_stone").Type;
        }
    }
}
