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
    public class ArchaeaTiles : GlobalTile
    {
        public static Tile GetSafeTile(float x, float y)
        {
            int i = (int)x / 16;
            int j = (int)y / 16;
            if (i <= 0) i = 50;
            if (i >= Main.maxTilesX) i = Main.maxTilesX - 50;
            if (j <= 0) i = 50;
            if (j >= Main.maxTilesY) i = Main.maxTilesY - 50;
            return Main.tile[i, j];
        }
        public static Tile GetSafeTile(Vector2 position)
        {
            int i = (int)position.X / 16;
            int j = (int)position.Y / 16;
            if (i <= 0) i = 50;
            if (i >= Main.maxTilesX) i = Main.maxTilesX - 50;
            if (j <= 0) i = 50;
            if (j >= Main.maxTilesY) i = Main.maxTilesY - 50;
            return Main.tile[i, j];
        }
        public static void GetCeilingTile(Vector2 position, out Tile ceiling, out Tile underneath)
        {
            int i = (int)position.X / 16;
            int j = (int)position.Y / 16;
            if (i <= 0) i = 50;
            if (i >= Main.maxTilesX) i = Main.maxTilesX - 50;
            if (j <= 0) i = 50;
            if (j >= Main.maxTilesY) i = Main.maxTilesY - 50;
            ceiling = Main.tile[i, j];
            underneath = Main.tile[i, j + 1];
        }
        public override bool CanExplode(int i, int j, int type)
        {
            for (int k = -1; k <= 1; k++)
                for (int l = -1; l <= 1; l++)
                {
                    Tile tile = Main.tile[i + k, j + l];
                    if (tile.TileType == ArchaeaWorld.crystal ||
                        tile.TileType == ArchaeaWorld.crystal2x1 ||
                        tile.TileType == ArchaeaWorld.crystal2x2)
                        return ModContent.GetInstance<ArchaeaWorld>().downedMagno;
                }
            return base.CanExplode(i, j, type);
        }
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            for (int k = -1; k <= 1; k++)
                for (int l = -1; l <= 1; l++)
                {
                    Tile tile = Main.tile[i + k, j + l];
                    if (tile.TileType == ArchaeaWorld.crystal ||
                        tile.TileType == ArchaeaWorld.crystal2x1 ||
                        tile.TileType == ArchaeaWorld.crystal2x2)
                        return ModContent.GetInstance<ArchaeaWorld>().downedMagno;
                }
            return base.CanKillTile(i, j, type, ref blockDamaged);
        }
        private int counter;
        public override void RandomUpdate(int i, int j, int type)
        {
            if (!ArchaeaWorld.Inbounds(i, j))
                return;
            if (Main.rand.NextFloat() > 0.95f && type == ArchaeaWorld.magnoStone)
            {
                int count = 0;
                ushort[] types = new ushort[]
                {
                    ArchaeaWorld.crystal,
                    ArchaeaWorld.crystal2x1,
                    ArchaeaWorld.crystal2x2,
                    ArchaeaWorld.crystalLarge,
                    ArchaeaWorld.magnoPlantsSmall,
                    ArchaeaWorld.magnoPlantsLarge
                };
                for (int k = i - 8; k < i + 8; k++)
                for (int l = j - 8; l < j + 8; l++)
                {
                    foreach (ushort t in types)
                    {
                        if (Main.tile[k, l].TileType == t)
                        {
                            count++;
                        }
                    }
                }
                if (count == 0)
                {
                    Tile top = Main.tile[i, j + 1];
                    Tile right = Main.tile[i + 1, j];
                    Tile bottom = Main.tile[i, j - 1];
                    Tile left = Main.tile[i - 1, j];
                    if (type == ArchaeaWorld.magnoStone)
                    {
                        if (!top.HasTile)
                            WorldGen.PlaceTile(i, j - 1, (int)types[0], true, false, -1, 3);
                        else if (!right.HasTile)
                            WorldGen.PlaceTile(i, j - 1, (int)types[0], true, false, -1, 1);
                        else if (!bottom.HasTile)
                        {
                            if (Main.rand.NextBool())
                                WorldGen.PlaceTile(i, j - 1, Main.rand.Next(new int[] { (int)types[0], (int)types[1], (int)types[2] }), true, false, -1, 0);
                            else if (Main.hardMode)
                                WorldGen.PlaceTile(i, j - 1, types[3], true, false);
                        }
                        else if (!left.HasTile)
                            WorldGen.PlaceTile(i, j - 1, (int)types[0], true, false, -1, 2);
                    }
                }
                if (count < 3 && type == ArchaeaWorld.Ash)
                {
                    if (!Main.tile[i, j - 1].HasTile && !Main.tile[i, j - 2].HasTile)
                    {
                        if (Main.rand.NextBool())
                            WorldGen.PlaceTile(i, j - 1, types[4], true, false, -1, WorldGen.genRand.Next(4));
                        else WorldGen.PlaceTile(i, j - 1, types[5], true, false, -1, WorldGen.genRand.Next(3));
                    }
                }
            }
        }
    }
}
