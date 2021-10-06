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
    public class m_plants_small : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileNoFail[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.CoordinateHeights = new [] { 16, 18 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorValidTiles = new int[] { ArchaeaWorld.Ash };
            TileObjectData.newTile.AnchorAlternateTiles = new int[] { TileID.ClayPot, TileID.PlanterBox };
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(210, 110, 180));
            disableSmartCursor = true;
            mineResist = 1.2f;
            minPick = 45;
            soundStyle = 0;
            soundType = SoundID.Grass;
        }
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
        }
        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i ,j);
            if (tile.frameX < 18 * 3 && WorldGen.genRand.NextBool() && ModContent.GetInstance<ArchaeaWorld>().downedMagno)
            {
                tile.frameX += 18;
            }
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            type = ModContent.DustType<Merged.Dusts.magno_dust>();
            return base.CreateDust(i, j, ref type);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new Vector2(i, j).ToWorldCoordinates(), ModContent.ItemType<Items.Materials.magno_plant>());
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].frameX != 18 * 3 || Main.tile[i, j].liquid > 0 || !ModContent.GetInstance<ArchaeaWorld>().downedMagno)
                return;
            r = 224f / 255f / 5f;
            g = 135f / 255f / 5f;
            b = 19f / 255f / 5f;
        }
    }
}
