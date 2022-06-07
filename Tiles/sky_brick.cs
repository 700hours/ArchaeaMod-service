using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Tiles
{
    public class sky_brick : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            ItemDrop = Mod.Find<ModItem>("sky_brick").Type;
            AddMapEntry(Color.LightSlateGray);
           // soundStyle = 0;
            HitSound = SoundID.Tink;
        }
        public override void WalkDust(ref int DustType, ref bool makeDust, ref Color color)
        {
            DustType = ModContent.DustType<Merged.Dusts.c_silver_dust>();
            makeDust = true;
            color = Color.LightGray;
        }
    }
}
