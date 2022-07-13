using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

using ArchaeaMod.NPCs.Bosses;

namespace ArchaeaMod.Items.Tiles
{
    public class purple_haze : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purple Haze");
            Tooltip.SetDefault("Fever dreams coalesced into a tangible essence");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.rare = 2;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.value = 2500;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ArchaeaMod.Tiles.purple_haze>();
        }
    }
}