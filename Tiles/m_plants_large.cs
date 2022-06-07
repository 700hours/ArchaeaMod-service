using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArchaeaMod.Tiles
{
    public class m_plants_large : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.AnchorValidTiles = new int[] { ArchaeaWorld.Ash };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(210, 110, 210));
            TileID.Sets.DisableSmartCursor[Type] = true;
            MineResist = 1.2f;
            MinPick = 45;
           // soundStyle = 0;
            HitSound = SoundID.Grass;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            type = ModContent.DustType<Merged.Dusts.magno_dust>();
            return base.CreateDust(i, j, ref type);
        }
    }
}
