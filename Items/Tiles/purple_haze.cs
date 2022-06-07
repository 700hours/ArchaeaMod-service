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
            Item.width = 48;
            Item.height = 48;
            Item.rare = 2;
            Item.value = 10000;
            Item.consumable = false;
        }
    }
}