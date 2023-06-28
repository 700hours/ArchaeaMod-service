using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ArchaeaMod.Items;
using ArchaeaMod.Structure;
using IL.Terraria.GameContent.Bestiary;
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
    public class ambient_rocks_1 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.AnchorValidTiles = new int[] { ArchaeaWorld.magnoStone };
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(210, 110, 110));
            TileID.Sets.DisableSmartCursor[Type] = true;
            MineResist = 1.2f;
            MinPick = 45;
        }
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
        }
    }
}
