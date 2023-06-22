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
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            Main.tileMerge[Type][TileID.WoodBlock] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[Type][TileID.Sand] = true;
            Main.tileMerge[Type][TileID.Grass] = true;
            Main.tileMerge[TileID.WoodBlock][Type] = true;
            Main.tileMerge[TileID.Stone][Type] = true;
            Main.tileMerge[TileID.Sand][Type] = true;
            Main.tileMerge[TileID.Grass][Type] = true;
            ItemDrop = ModContent.ItemType<Items.Tiles.sky_brick>();
            AddMapEntry(Color.LightSlateGray);
           // soundStyle = 0;
            HitSound = SoundID.Tink;
        }
        public override bool HasWalkDust()
        {
            return true;
        }
        public override void WalkDust(ref int DustType, ref bool makeDust, ref Color color)
        {
            DustType = ModContent.DustType<Merged.Dusts.c_silver_dust>();
            makeDust = true;
            color = Color.LightGray;
        }
    }
}
