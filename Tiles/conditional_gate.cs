using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ID.ContentSamples.CreativeHelper;
using Terraria.ModLoader;

using Terraria;
using Terraria.ID;

namespace ArchaeaMod.Tiles
{
    internal class conditional_gate : ModTile
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            AddMapEntry(Color.LightSlateGray);
            // soundStyle = 0;
            HitSound = SoundID.Tink;
            MineResist = 1.5f;
            MinPick = 100;
        }
    }
}
