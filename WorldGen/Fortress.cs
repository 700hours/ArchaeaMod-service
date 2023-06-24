﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

using ArchaeaMod.GenLegacy;
using ArchaeaMod.Items;
using ArchaeaMod.Items.Alternate;
using ArchaeaMod.Merged;
using ArchaeaMod.Merged.Items;
using ArchaeaMod.Merged.Tiles;
using ArchaeaMod.Merged.Walls;

namespace ArchaeaMod
{
    public class Structures
    {
        internal bool[] direction;
        public static int index;
        private int count;
        private const int max = 3;
        public int[][,] house;
        public int[][,] rooms;
        public int[][,] fort;
        public int[,] island;
        public int[,] tower;
        public ushort tileID;
        public ushort wallID;
        private ushort[] decoration = new ushort[] { TileID.Statues, TileID.Pots };
        private ushort[] furniture = new ushort[] { TileID.Tables, TileID.Chairs, TileID.Pianos, TileID.GrandfatherClocks, TileID.Dressers };
        private ushort[] useful = new ushort[] { TileID.Loom, TileID.SharpeningStation, TileID.Anvils, TileID.CookingPots };
        private Vector2 origin;
        private List<Vector2> path;
        public const int YCoord = 60;
        public const ushort
            TILE_None = 0,
            TILE_Chain = 1,
            TILE_Brick = 2,
            TILE_WoodBeams = 3,
            TILE_IronFence = 4,
            TILE_Extra = 5,
            ENTR_Brick = 1,
            ENTR_Chain = 2,
            ENTR_Wood = 3,
            ENTR_Wood1 = 6,
            ENTR_Wood3 = 7,
            ENTR_Wood7 = 8,
            ENTR_Wood9 = 9;
        public static ushort[,] cage = new ushort[,]
        {
            { 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 2, 2, 2, 0, 0 },
            { 0, 2, 2, 2, 2, 2, 0 },
            { 0, 3, 4, 5, 4, 3, 0 },
            { 0, 3, 4, 4, 4, 3, 0 },
            { 0, 3, 4, 4, 4, 3, 0 },
            { 0, 2, 2, 2, 2, 2, 0 },
            { 0, 0, 2, 2, 2, 0, 0 }
        };
        private ushort[,] pulley = new ushort[,]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0 },
            { 1, 1, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0 },
            { 1, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0 },
            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 2, 8, 3, 9, 0 },
            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 2, 3, 0, 3, 0 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 6, 3, 7, 0 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0 },
            { 1, 1, 1, 1, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3 },
        };
        class ID
        {
            public const int
                Dirt = -2,
                Grass = -1,
                Empty = 0,
                Tile = 1,
                Wall = 2,
                Platform = 3,
                Stairs = 4,
                Floor = 5,
                Door = 6,
                Decoration = 7,
                Furniture = 8,
                Useful = 9,
                Lamp = 10,
                Chest = 11,
                Cloud = 12,
                Trap = 13,
                Danger = 14,
                Wire = 15,
                Window = 16,
                Light = 17,
                Dark = 18,
                WallHanging = 19,
                Portal = 20,
                Haze = 21;
        }
        class RoomID
        {
            public const int
                Empty = 0,
                FilledIn = 1,
                Safe = 2,
                Danger = 3,
                Chest = 4,
                Platform = 5,
                Start = 6,
                End = 7,
                Lighted = 8,
                Decorated = 9,
                Haze = 10;
        }
        class FortID
        {
            public const int
                Light = 0,
                Dark = 1;
        }
        public Structures(Vector2 origin = default(Vector2), ushort tileID = TileID.StoneSlab, ushort wallID = WallID.StoneSlab)
        {
            this.tileID = tileID;
            this.wallID = wallID;
            this.origin = origin;
        }
        public void InitializeFort()
        {
            int radius = 105;
            int lX = radius - 45;
            int lY = radius - 6;
            int cloud = radius + 75;
            int center = cloud / 2;
            int roomX = 15;
            int roomY = 9;
            index = -1;
            for (int i = 600; i < 600 + cloud; i++)
                for (int j = 50; j < 50 + lY + cloud / 8; j++)
                    WorldGen.KillTile(i, j, false, false, true);

            //island = new int[cloud, cloud / 2];
            //InitIsland();
            FortPathing(radius, roomX, roomY, lY);

            int lengthX = fort[0].GetLength(0);
            int lengthY = fort[0].GetLength(1);
            int[] roomTypes = new int[] { RoomID.Chest, RoomID.Danger, RoomID.Decorated, RoomID.Lighted, RoomID.Haze };

            int x1, x2, x3;
            int y1 = YCoord, y2 = y1, y3;
            int[,] cRoom;
            int cRoomWidth = 100;


            CloudGenerate((int)origin.X, (int)origin.Y + lengthY - 2, lengthX * 2 + cRoomWidth + 30, 45, TileID.RainCloud, true);
            CloudGenerate((int)origin.X + 8, (int)origin.Y + lengthY - 2, lengthX * 2 + cRoomWidth - 8 + 40, 20, TileID.Dirt);
            PlaceEntrance((int)origin.X, (int)origin.Y, 35, lengthY);
            //GenerateStructure(600, 50 + fort[0].GetLength(1), island);
            GenerateStructure(x1 = (int)origin.X + center - lengthX / 2, y1, fort[0], tileID, wallID);
            GenerateStructure(x2 = (int)origin.X + center + lengthX / 2, y1, fort[1], tileID, wallID);
            GenerateStructure(x3 = (int)origin.X + center + lengthX / 2 + lengthX, y3 = (int)(y1 + lengthY * 0.33f), cRoom = Chamber(cRoomWidth, (int)(lengthY * 0.67f)), tileID, wallID);
            ChamberRoof(x3, y1, cRoom.GetLength(0), (int)(lengthY * 0.33f));

            PlaceInteriorRooms(new Vector2((int)origin.X + center - lengthX / 2, y1), roomX, roomY, fort[0], roomTypes);
            PlaceInteriorRooms(new Vector2((int)origin.X + center + lengthX / 2, y1), roomX, roomY, fort[1], roomTypes);
            house = new int[max][,];
            rooms = new int[max - 1][,];

            house[0] = fort[0];
            SkyRoom();

            GenerateWalls(x1, y1, x3, fort[1].GetLength(1));

            GenConnect(x1, y1 + lengthY - 10, 20, 8);
            GenConnect(x1 + lengthX - 15, y1 + 5, 20, 8);
            GenConnect(x2, y1 + 5, 10, 8);
            GenConnect(x2 + lengthX - 50, y1 + lengthY - 11, 40 + cRoom.GetLength(0) / 2, 6);
            GenConnect(x3 + cRoom.GetLength(0) / 2 - 10, y1 + lengthY - 20, 8, 14);
        }
       
        internal void PlaceEntrance(int i, int j, int width, int height)
        {
            int num = 0;
            bool flag = false, flag2 = false;
            for (int m = i; m < i + width; m++)
                for (int n = j; n < j + height; n++)
                {
                    if (!flag && WorldGen.genRand.NextBool())
                        flag = true;
                    if (n < j + height - 10 || n > j + height - 2)
                    {
                        Main.tile[m, n].TileType = ArchaeaWorld.skyBrick;
                        Tile tile = Main.tile[m, n];
                        tile.HasTile = false;
                    }
                    else if (m == j + 3)
                        WorldGen.PlaceTile(m, n, TileID.TallGateClosed, true, false);
                    if (!flag2 && m == i && n == j + height / 2)
                    {

                        flag2 = true;
                    }
                    if (m == i && flag)
                    {
                        if (num++ < 5)
                        {
                            Main.tile[m, n].TileType = 0;
                            Tile tile = Main.tile[m, n];
                            tile.HasTile = false;
                        }
                        else flag = false;
                    }
                }
        }
        internal void CloudGenerate(int i, int j, int width, int height, ushort tileType, bool danger = false)
        {
            bool flag = false, flag2 = false, flag3 = false, flag4 = false, flag5 = false;
            int num = 2;
            Tile t;
            for (int m = i; m < i + width; m++)
            {
                if (m < i + 30)
                    num += WorldGen.genRand.Next(3);
                if (m > i + width - 30)
                {
                    if (!flag3)
                        flag3 = true;
                    num -= WorldGen.genRand.Next(1, 3);
                }
                if (m % 10 == 0 && WorldGen.genRand.NextBool())
                    flag = !flag;
                if (m % 15 == 0)
                {
                    flag4 = !flag4;
                    if (WorldGen.genRand.NextBool())
                        flag5 = !flag5;
                }
                float sine = (float)Math.Sin(((m - i) / width) * 360f);
                if (!flag3)
                {
                    if (WorldGen.genRand.NextFloat() >= 0.33f)
                        num -= (int)(WorldGen.genRand.Next(flag ? 0 : -2, flag ? 2 : 0) * sine);
                    else num -= (flag ? -1 : 1);
                }
                if (num > height)
                {
                    num -= 2;
                    flag = !flag;
                }
                if (num < height / 3)
                    flag = !flag;
                for (int n = j; n < j + num; n++)
                {
                    if (n < j + height / 3)
                        n += (int)(WorldGen.genRand.Next(0, 2) * sine);
                    Main.tile[m, n].TileType = tileType;
                    t = Main.tile[m, n];
                    t.HasTile = true;
                    if (danger && n == j + num - 1)
                    {
                        if (flag4)
                        {
                            Main.tile[m, n].TileType = flag5 ? TileID.Spikes : TileID.Chain;
                            t = Main.tile[m, n];
                            t.HasTile = true;
                        }
                        if (m % 80 == 0)
                        {
                            for (int k = 0; k < cage.GetLength(1); k++)
                                for (int l = 0; l < cage.GetLength(0); l++)
                                {
                                    Tile tile = Main.tile[m + k, n + l];
                                    switch (cage[l, k])
                                    {
                                        case 10:
                                            tile.HasTile = true;
                                            break;
                                        case TILE_None:
                                            tile.TileType = 0;
                                            tile.HasTile = false;
                                            break;
                                        case TILE_Chain:
                                            for (int x = 0; x < 6; x++)
                                            {
                                                if (!Main.tile[m + k, n + l - x].HasTile)
                                                {
                                                    Main.tile[m + k, n + l - x].TileType = TileID.Chain;
                                                    t = Main.tile[m + k, n + l - x];
                                                    t.HasTile = true;
                                                }
                                            }
                                            tile.TileType = TileID.Chain;
                                            goto case 10;
                                        case TILE_Brick:
                                            tile.TileType = ArchaeaWorld.skyBrick;
                                            goto case 10;
                                        case TILE_WoodBeams:
                                            tile.TileType = TileID.WoodenBeam;
                                            goto case 10;
                                        case TILE_IronFence:
                                            //  TODO iron fence
                                            tile.TileType = TileID.Chain;
                                            break;
                                        case TILE_Extra:
                                            //  TODO 3x3 dungeon tiles
                                            byte rand = (byte)WorldGen.genRand.Next(2);
                                            if (rand == 0) WorldGen.PlaceTile(m + k, n + l, TileID.HangingLanterns);
                                            else WorldGen.Place3x3Wall(m + k, n + l, TileID.Painting3X3, WorldGen.genRand.Next(16, 17));
                                            break;
                                    }
                                }
                        }
                    }
                }
            }
        }
        internal void ChamberRoof(int x, int y, int width, int height)
        {
            float n = y;
            for (int i = x; i < x + width; i++)
            {
                n += (float)height / width;
                for (int j = (int)n; j < y + height; j++)
                {
                    Tile tile = Main.tile[i, j];
                    tile.HasTile = true;
                    Main.tile[i, j].TileType = tileID;
                }
            }
        }
        internal void GenConnect(int x, int y, int width, int height)
        {
            for (int i = x; i < x + width; i++)
                for (int j = y; j < y + height; j++)
                {
                    Tile tile = Main.tile[i, j];
                    tile.HasTile = true;
                    Main.tile[i, j].WallType = wallID;
                }
        }
        internal int[,] Chamber(int w, int height)
        {
            int width = w;
            int[,] room = new int[width, height];
            for (int i = 0; i < room.GetLength(0); i++)
                for (int j = 0; j < height; j++)
                {
                    room[i, j] = ID.Tile;
                }
            for (int i = 0; i < width / 4; i++)
                for (int j = 0; j < height / 2 - 5; j++)
                {
                    for (float r = 0f; r < 360f; r++)
                    {
                        int x = (int)(width / 2 + i * Math.Cos(r));
                        int y = (int)((height / 2 - 5) + j * Math.Sin(r));
                        room[x, y] = ID.Empty;
                    }
                }
            room[width / 2 - 1, height / 2 - 1] = ID.Portal;
            return room;
        }
        internal void InitIsland()
        {
            int lengthX = island.GetLength(0);
            int lengthY = island.GetLength(1);
            int margin = 10;
            for (int k = 0; k < lengthX; k++)
            {
                int j = (int)Math.Min(lengthY - 1, Math.Abs(lengthY * Math.Sin(k * Draw.radian)));
                for (int l = 0; l < j; l++)
                    island[k, l] = ID.Cloud;
                for (int m = 0; m < j / 2; m++)
                    island[k, m] = ID.Dirt;
            }
            for (int k = margin; k < lengthX - margin; k++)
                island[k, 0] = ID.Grass;
        }
        internal void InitTower()
        {
            int lengthX = tower.GetLength(0) - 1;
            int lengthY = tower.GetLength(1) - 1;
            int margin = 5;
            int width = lengthX - margin * 2;
            int top = 15;
            for (int m = margin; m < width; m++)
                for (int n = lengthY - 1; n > top; n--)
                    tower[m, n] = ID.Tile;
            List<Vector2> move = new List<Vector2>();
            for (int j = lengthY; j > top; j--)
                move.Add(new Vector2(margin + (float)(width - width * Math.Abs(Math.Cos(j * Draw.radian))), j));
            foreach (Vector2 v in move)
            {
                for (int k = 0; k < 4; k++)
                    for (int l = 0; l < 6; l++)
                    {
                        int i = (int)v.X + k;
                        int j = (int)v.Y + l;
                        if (i > margin && i < width && l < lengthY && l > top)
                            tower[i, j] = ID.Empty;
                    }
            }
            for (int m = 0; m <= lengthX; m++)
                for (int n = 0; n <= top; n++)
                {
                    if (m == 0 || m == lengthX - 1 || n == 0 || n == top)
                        tower[m, n] = ID.Tile;
                    else tower[m, n] = ID.Wall;
                }
        }
        public void GenerateStructure(int x, int y, int[,] structure, ushort tileType = 0, ushort wallType = 0)
        {
            int lengthX = structure.GetLength(0);
            int lengthY = structure.GetLength(1);
            for (int i = 0; i < lengthX; i++)
                for (int j = 0; j < lengthY; j++)
                {
                    int m = i + x;
                    int n = j + y;
                    Tile tile = Main.tile[m, n];
                    switch (structure[i, j])
                    {
                        case ID.Empty:
                            WorldGen.KillTile(m, n, false, false, true);
                            goto case ID.Wall;
                        case ID.Tile:
                            WorldGen.PlaceTile(m, n, tileType, true, true);
                            break;
                        case ID.Wall:
                            WorldGen.PlaceWall(m, n, wallType, true);
                            break;
                        case ID.Dirt:
                            tile.HasTile = true;
                            tile.TileType = TileID.Dirt;
                            break;
                        case ID.Grass:
                            tile.HasTile = true;
                            tile.TileType = TileID.Grass;
                            break;
                        case ID.Cloud:
                            tile.HasTile = true;
                            tile.TileType = TileID.Cloud;
                            break;
                        case ID.Portal:
                            WorldGen.Place3x3Wall(m, n, (ushort)ModContent.TileType<ArchaeaMod.Tiles.sky_portal>(), 0);
                            goto case ID.Wall;
                    }
                }
        }
        internal void FortPathing(int radius, int roomX, int roomY, int lY)
        {
            fort = new int[2][,];
            int Width = (radius - 45) * 2;
            for (int k = 0; k < 2; k++)
            {
                fort[k] = new int[Width, lY];
                for (int i = 0; i < Width; i++)
                    for (int j = 0; j < lY; j++)
                        fort[k][i, j] = ID.Tile;
                bool direction = k == 0;
                bool genRoom = false;
                int m = 0;
                int n = 0;
                int width = Width / 5 - 3;
                int margin = roomY * 2;
                int space = roomX / 3;
                int moved = 0;
                int start;
                int room = 0;
                int roomHeight = 4;
                Vector2 move = new Vector2(k == 0 ? 5 : Width - 30, lY - roomY);
                for (int total = 0; total < lY / margin; total++)
                {
                    int spacing = WorldGen.genRand.Next(8, 12);
                    for (moved = 0; moved < Width - 20; moved += space)
                    {
                        if ((total == 0 && moved > (Width / 1.5f)) || (total == 1 && move.X < 10 && !direction))
                            break;
                        move.X += direction ? space : space * -1;
                        space = roomX / (WorldGen.genRand.NextBool() ? 3 : 5);
                        int height = WorldGen.genRand.Next(5, roomY - 1);
                        start = WorldGen.genRand.Next(-3, 1);
                        for (int j = genRoom ? start - roomHeight : start; j < height; j++)
                            for (int i = 0; i < 5; i++)
                            {
                                m = (int)move.X + i;
                                n = (int)move.Y + j;
                                ArchaeaWorld.Clamp(ref m, 3, Width - 3, out m);
                                ArchaeaWorld.Clamp(ref n, 3, lY - 3, out n);
                                fort[k][m, n] = ID.Empty;
                            }
                        room++;
                        if (room > (genRoom ? spacing + 4 : spacing))
                        {
                            genRoom = !genRoom;
                            room = 0;
                        }
                    }
                    int destination = (int)move.Y - margin;
                    int oldY = (int)move.Y;
                    for (move.Y = oldY; move.Y > destination; move.Y--)
                    {
                        start = WorldGen.genRand.Next(-2, 2);
                        for (int i = start; i < 5 + start; i++)
                            for (int j = 0; j < 5; j++)
                            {
                                m = (int)move.X + i;
                                n = (int)move.Y + j;
                                ArchaeaWorld.Clamp(ref m, 3, Width - 3, out m);
                                ArchaeaWorld.Clamp(ref n, 3, lY - 3, out n);
                                fort[k][m, n] = ID.Empty;
                            }
                    }
                    direction = !direction;
                }
            }
        }
        internal void PlaceInteriorRooms(Vector2 origin, int roomX, int roomY, int[,] structure, int[] types)
        {
            int lengthX = structure.GetLength(0);
            int lengthY = structure.GetLength(1);
            int[,] rooms = new int[lengthX / roomX, lengthY / roomY];
            bool[,] placed = new bool[lengthX / roomX, lengthY / roomY];
            for (int i = 0; i < lengthX / roomX; i++)
                for (int j = 0; j < lengthY / roomY; j++)
                    rooms[i, j] = WorldGen.genRand.Next(types);
            for (int j = 0; j < lengthY; j++)
                for (int i = 0; i < lengthX; i++)
                {
                    int x = (int)origin.X + i;
                    int y = (int)origin.Y + j;
                    int m = i / roomX;
                    int n = j / roomY;
                    switch (rooms[m, n])
                    {
                        case RoomID.Chest:
                            if (!placed[m, n])
                                WorldGen.PlaceChest(x, y);
                            if (IsPlaced(x, y, TileID.Containers))
                                placed[m, n] = true;
                            break;
                        case RoomID.Danger:
                            if (structure[i, Math.Max(j - 1, 0)] == ID.Empty && structure[i, j] == ID.Tile)
                                WorldGen.PlaceTile(x, y, TileID.Spikes, true, true);
                            break;
                        case RoomID.Decorated:
                            if (WorldGen.genRand.NextBool())
                                WorldGen.PlaceTile(x, y, WorldGen.genRand.Next(decoration));
                            break;
                        case RoomID.Lighted:
                            if (!Treasures.Vicinity(new Vector2(x, y), 15, TileID.HangingLanterns))
                                WorldGen.PlaceTile(x, y, TileID.HangingLanterns);
                            break;
                        case RoomID.Haze:
                            if (WorldGen.genRand.Next(5) == 0)
                                WorldGen.PlaceTile(x, y, ModContent.TileType<Tiles.purple_haze>(), true, true);
                            break;
                    }
                    WorldGen.PlaceWall(i, j, wallID);
                }
        }
        internal bool IsPlaced(int i, int j, ushort tile)
        {
            return Main.tile[i, j].TileType == tile && Main.tile[i, j].HasTile;
        }
        public void DesignateRoom(int i, int j, int roomX, int roomY, int index)
        {
            index = 0;
            bool lamp = false,
                useful = false,
                placed = false;
            int q = i;
            int r = j;
            int m = 0;
            int n = 0;
            int width = q + roomX;
            int height = r + roomY;
            for (int y = r; y < r + roomY; y++)
            {
                if (y < roomY || y >= fort.GetLength(1) - roomY)
                    continue;
                for (int x = q; x < q + roomX; x++)
                {
                    if (x < roomX || x >= fort.GetLength(0) - roomX)
                        continue;
                    int[] choice = new int[] { RoomID.Safe, RoomID.Decorated, RoomID.Lighted, RoomID.Chest, RoomID.Danger };
                    int rand = WorldGen.genRand.Next(choice);
                    m = x;
                    n = y;
                    switch (rand)
                    {
                        case -2:
                            break;
                        case -1:
                            break;
                        case RoomID.Safe:
                            for (int t = 0; t < 5; t++)
                            {
                                int ground = q + WorldGen.genRand.Next(1, 11);
                                if (ground % 2 == 0)
                                {
                                    while (fort[index][ground, n] != ID.Wall)
                                        n++;
                                    n--;
                                    if (!useful)
                                    {
                                        fort[index][ground, n] = ID.Useful;
                                        useful = true;
                                    }
                                    fort[index][ground, n] = WorldGen.genRand.Next(new int[] { ID.Decoration, ID.Furniture });
                                }
                            }
                            goto case RoomID.Lighted;
                        case RoomID.Decorated:
                            if (WorldGen.genRand.NextFloat() > 0.67f)
                                fort[index][m, n] = ID.WallHanging;
                            for (int t = 0; t < 4; t++)
                            {
                                int ground = q + WorldGen.genRand.Next(1, 11);
                                while (fort[index][ground, n] != ID.Wall)
                                    n++;
                                n--;
                                if (ground % 2 == 0)
                                    fort[index][ground, n] = ID.Decoration;
                            }
                            goto case -1;
                        case RoomID.Lighted:
                            if (!lamp)
                            {
                                int roof = q + WorldGen.genRand.Next(2, 10);
                                fort[index][roof, y] = ID.Lamp;
                                lamp = true;
                            }
                            goto case -1;
                        case RoomID.Chest:
                            for (int t = 0; t < 4; t++)
                            {
                                int ground = i + WorldGen.genRand.Next(1, 11);
                                while (fort[index][ground, n] != ID.Wall)
                                    n++;
                                n--;
                                if (ground % 2 == 1)
                                {
                                    if (fort[index][ground, n] == ID.Empty)
                                        fort[index][ground, n] = ID.Decoration;
                                    if (!placed)
                                    {
                                        fort[index][ground, n] = ID.Chest;
                                        placed = true;
                                    }
                                }
                            }
                            goto case -1;
                        case RoomID.Danger:
                            goto case -1;
                    }
                }
            }
        }
        public Vector2 PlaceRoom(ref int i, ref int j, int index)
        {
            int width = WorldGen.genRand.Next(10, 22);
            int height = WorldGen.genRand.Next(10, 12);
            Vector2 origin = Vector2.Zero;
            for (int k = i; k < i + width; k++)
            {
                if (origin != Vector2.Zero)
                    break;
                for (int l = j; l < j + height; l++)
                    if (k < fort.GetLength(0) && l < fort.GetLength(1))
                        if (fort[index][k, l] == ID.Empty && origin == Vector2.Zero)
                        {
                            origin = new Vector2(k, l);
                            i = k;
                            j = l;
                            break;
                        }
            }
            for (int k = i; k < i + width; k++)
                for (int l = j; l < j + height; l++)
                    if (k < fort.GetLength(0) && l < fort.GetLength(1))
                        fort[index][k, l] = ID.Empty;
            for (float radius = 0; radius < height; radius++)
                for (float r = -(float)Math.PI; r <= 0; r += Draw.radians(radius))
                {
                    Vector2 c = NPCs.ArchaeaNPC.AngleBased(new Vector2(i + width / 2, j + height / 2), r, radius);
                    if (c.X < fort.GetLength(0) && c.Y < fort.GetLength(1))
                        fort[index][(int)c.X, (int)c.Y] = ID.Empty;
                }
            return origin;
        }
        public void CloudForm(int i, int j)
        {
            int width = 90;
            for (int m = 0; m < width; m++)
                for (int n = 0; n < width / 2; n++)
                {
                    if (WorldGen.genRand.Next(25) == 0)
                        for (float k = 0; k < 12; k += 12 * Draw.radian)
                            for (float r = 0f; r < Math.PI * 2f; r += Draw.radians(k))
                            {
                                int cos = (int)(i + m + k * Math.Cos(r));
                                int sine = (int)(j + n + k / 2f * Math.Sin(r));
                                Tile tile = Main.tile[cos, sine];
                                tile.HasTile = true;
                                Main.tile[cos, sine].TileType = TileID.Cloud;
                            }
                }
        }
        public void RoomItems(int k, int index = 0, int roomX = 15, int roomY = 9, bool genWalls = false)
        {
            for (int i = 0; i < rooms[k].GetLength(0); i++)
                for (int j = 0; j < rooms[k].GetLength(1); j++)
                {
                    int m = i * roomX;
                    int n = j * roomY;
                    int width = m + roomX;
                    int height = n + roomY;
                    int added = 0;
                    int floor;
                    bool placed;
                    for (int q = m; q < width; q++)
                        for (int r = n; r < height; r++)
                            switch (rooms[k][i, j])
                            {
                                case -2:
                                    if (genWalls)
                                    {
                                        int door = 0;
                                        if (rooms[k][Math.Max(i - 1, 0), j] != RoomID.FilledIn)
                                            if (i != 0 && q == m && r < height)
                                            {
                                                if (j != rooms[k].GetLength(1) - 1 && r >= height - 3)
                                                    house[index][q, r] = ID.Empty;
                                                else if (j == rooms[k].GetLength(1) - 1)
                                                {
                                                    door = 1;
                                                    if (r >= height - 4 && r < height - 1)
                                                        house[index][q, r] = ID.Empty;
                                                }
                                                else door = 0;
                                                if (r == height - 1 - door)
                                                    house[index][q, r] = ID.Door;
                                            }
                                    }
                                    if (rooms[k][i, j] == RoomID.Start)
                                    {
                                        if (q == (k == FortID.Light ? m : width - 1) && r >= height - 4 && r < height - 1)
                                        {
                                            house[index][q, r] = ID.Empty;
                                            if (r == height - 3)
                                                house[index][q, r] = ID.Door;
                                        }
                                        if (q > m + 5 && q < m + 10 && r == n)
                                            house[index][q, r] = ID.Platform;
                                    }
                                    if (rooms[k][i, j] == RoomID.End)
                                        if (q == (k == FortID.Light ? width - 1 : m) && r >= height - 3 && r < height)
                                        {
                                            if (r == height - 2)
                                                house[index][q, r] = ID.Door;
                                            else house[index][q, r] = ID.Empty;
                                        }
                                    break;
                                case -1:
                                    if (genWalls)
                                    {
                                        if ((i == 0 && q == m) || (i == rooms[k].GetLength(0) - 1 && q == width - 1))
                                            house[index][q, r] = ID.Wall;
                                        if (rooms[k][Math.Max(i - 1, 0), j] != RoomID.FilledIn)
                                            if (i != 0 && q == m)
                                            {
                                                house[index][q, r] = ID.Wall;
                                                goto case -2;
                                            }
                                    }
                                    break;
                                case RoomID.Empty:
                                    goto case RoomID.Decorated;
                                case RoomID.Start:
                                    goto case -2;
                                case RoomID.End:
                                    goto case -2;
                                case RoomID.FilledIn:
                                    house[index][q, r] = ID.Wall;
                                    break;
                                case RoomID.Platform:
                                    if (q > m + 5 && q < m + 10 && r == n)
                                        house[index][q, r] = ID.Platform;
                                    goto case -1;
                                case RoomID.Safe:
                                    added = 0;
                                    bool useful = false;
                                    if (j == rooms[k].GetLength(1) - 1)
                                        floor = 2;
                                    else floor = 1;
                                    if (q == m + 1 && r == height - floor)
                                    {
                                        while (added < 5)
                                        {
                                            int ground = q + WorldGen.genRand.Next(1, 11);
                                            if (ground % 2 == 0)
                                            {
                                                if (!useful)
                                                {
                                                    house[index][ground, r] = ID.Useful;
                                                    useful = true;
                                                }
                                                house[index][ground, r] = WorldGen.genRand.Next(new int[] { ID.Decoration, ID.Furniture });
                                                added++;
                                            }
                                        }
                                    }
                                    goto case RoomID.Lighted;
                                case RoomID.Decorated:
                                    added = 0;
                                    if (j == rooms[k].GetLength(1) - 1)
                                        floor = 2;
                                    else floor = 1;
                                    if (q == m + 1 && r == height - floor)
                                    {
                                        while (added < 4)
                                        {
                                            int ground = q + WorldGen.genRand.Next(1, 11);
                                            if (ground % 2 == 0)
                                            {
                                                house[index][ground, r] = ID.Decoration;
                                                added++;
                                            }
                                        }
                                    }
                                    goto case -1;
                                case RoomID.Lighted:
                                    bool lamp = false;
                                    if (q == m && r == n + 1)
                                    {
                                        if (!lamp)
                                        {
                                            int roof = q + WorldGen.genRand.Next(2, 10);
                                            house[index][roof, r] = ID.Lamp;
                                            lamp = true;
                                        }
                                    }
                                    goto case -1;
                                case RoomID.Chest:
                                    added = 0;
                                    placed = false;
                                    if (j == rooms[k].GetLength(1) - 1)
                                        floor = 2;
                                    else floor = 1;
                                    if (q == m + 3 && q < width - 3 && r == height - floor)
                                    {
                                        while (added < 4)
                                        {
                                            int ground = q + WorldGen.genRand.Next(1, 11);
                                            if (ground % 2 == 1)
                                            {
                                                if (house[index][ground, r] == ID.Empty)
                                                {
                                                    house[index][ground, r] = ID.Decoration;
                                                    added++;
                                                }
                                                if (!placed)
                                                {
                                                    house[index][ground, r] = ID.Chest;
                                                    placed = true;
                                                }
                                            }
                                        }
                                    }
                                    if (q > m + 3 && q < width - 2 && r >= 3 && r <= 3)
                                        house[index][q, r] = ID.Window;
                                    goto case -1;
                                case RoomID.Danger:
                                    goto case -1;
                            }
                }
        }
        public void SkyRoom(bool killRegion = false)
        {
            int m;
            int n;
            int w = 0;
            int k = 0;
            int width = house[0].GetLength(0);
            int lengthX = house[k].GetLength(0);
            int lengthY = house[k].GetLength(1);
            ushort type;
            for (int i = 0; i < lengthX; i++)
                for (int j = 0; j < lengthY; j++)
                {
                    m = (int)origin.X + i;
                    n = (int)origin.Y + j;
                    if (killRegion)
                        WorldGen.KillTile(m, n, false, false, true);
                    Tile tile = Main.tile[m, n];
                    if (house[k][i, j] == ID.Wall)
                    {
                        tile.HasTile = true;
                        tile.TileType = tileID;
                    }
                    if (house[k][i, j] != ID.Wall && house[k][i, j] != ID.Door && house[k][i, j] != ID.Window)
                        WorldGen.PlaceWall(m, n, wallID);
                }
            for (int i = 0; i < lengthX; i++)
                for (int j = 0; j < lengthY; j++)
                {
                    m = (int)origin.X + i;
                    n = (int)origin.Y + j;
                    Tile tile = Main.tile[m, n];
                    switch (house[k][i, j])
                    {
                        case -2:
                            tile.HasTile = false;
                            break;
                        case -1:
                            tile.HasTile = true;
                            break;
                        case ID.Cloud:
                            tile.TileType = TileID.Cloud;
                            goto case -1;
                        case ID.Floor:
                            tile.TileType = tileID;
                            goto case -1;
                        case ID.Platform:
                            tile.TileType = TileID.Platforms;
                            goto case -1;
                        case ID.Chest:
                            WorldGen.PlaceChest(m, n);
                            break;
                        case ID.Furniture:
                            type = furniture[WorldGen.genRand.Next(furniture.Length)];
                            WorldGen.PlaceTile(m, n, type);
                            break;
                        case ID.Useful:
                            type = useful[WorldGen.genRand.Next(useful.Length)];
                            if (!Treasures.Vicinity(origin, 20, type))
                                WorldGen.PlaceTile(m, n, type);
                            break;
                        case ID.Decoration:
                            type = decoration[WorldGen.genRand.Next(decoration.Length)];
                            WorldGen.PlaceTile(m, n, type);
                            break;
                        case ID.Lamp:
                            if (Main.rand.NextBool(4))
                            { 
                                WorldGen.PlaceWall(m, n, wallID);
                                WorldGen.PlaceTile(m, n, TileID.HangingLanterns);
                            }
                            break;
                        case ID.Door:
                            tile.HasTile = false;
                            tile.TileType = 0;
                            WorldGen.PlaceWall(m, n, wallID);
                            WorldGen.PlaceDoor(m, n, TileID.ClosedDoor);
                            break;
                        case ID.Window:
                            WorldGen.PlaceWall(m, n, (ushort)WorldGen.genRand.Next(88, 93));
                            break;
                        case ID.Light:
                            tile.TileType = ArchaeaWorld.skyBrick;
                            goto case -1;
                        case ID.Dark:
                            tile.TileType = ArchaeaWorld.skyBrick;
                            goto case -1;
                        case ID.WallHanging:
                            WorldGen.Place3x2Wall(m, n, TileID.WeaponsRack, 0);
                            break;
                        case ID.Haze:
                            if (WorldGen.genRand.Next(4) == 0)
                                WorldGen.PlaceTile(m, n, ModContent.TileType<Tiles.purple_haze>(), true);
                            break;
                    }
                }
        }
        public class Magno : Structures
        {
            public void Initialize()
            {
                house = new int[max][,];
                bool rand = WorldGen.genRand.Next(2) == 0;
                direction = new bool[] { !rand, rand, !rand };
                int randFloor = WorldGen.genRand.Next(max);
                bool craft = false;
                bool chest = false;
                for (int k = 0; k < max; k++)
                {
                    int randX = k != 1 ? WorldGen.genRand.Next(15, 28) : WorldGen.genRand.Next(15, 20);
                    int randY = WorldGen.genRand.Next(7, 9);
                    int numLights = 0;
                    int numObjects = 0;
                    bool furniture = false;
                    bool stairs = false;
                    house[k] = new int[randX, randY];
                    for (int i = 0; i < randX; i++)
                        for (int j = 0; j < randY; j++)
                        {
                            if (i == 0 || i == randX - 1 || (k != 1 && (j == 0 || j == randY - 1)))
                                house[k][i, j] = tileID;
                            if (k != 1 && (i == 0 || i == randX - 1) && j == randY - 3)
                                house[k][i, j] = ID.Door;
                            if (i > 0 && i < randX - 1)
                            {
                                int x;
                                int y;
                                int count = 0;
                                int top = direction[k] ? 8 : 12;
                                if (i == top && j == 1)
                                {
                                    while (!stairs)
                                    {
                                        if (count == randY - 1)
                                            stairs = true;
                                        if (direction[k] && i + count < randX && j + count < randY)
                                            house[k][i + count, j + count] = ID.Stairs;
                                        if (!direction[k] && i - count >= 0 && j + count < randY)
                                            house[k][i - count, j + count] = ID.Stairs;
                                        count++;
                                    }
                                }
                                while (i == 1 && j == 1 && numLights < 2)
                                {
                                    x = i + WorldGen.genRand.Next(randX - 2);
                                    house[k][x, j] = ID.Lamp;
                                    numLights++;
                                }
                                if (i == 1 && j == randY - 2)
                                {
                                    while (numObjects < 3)
                                    {
                                        x = i + WorldGen.genRand.Next(randX - 3);
                                        if (x % 2 == 1)
                                            if (house[k][x, j] == 0)
                                            {
                                                house[k][x, j] = ID.Decoration;
                                                house[k][x + 1, j] = ID.Empty;
                                                numObjects++;
                                            }
                                    }
                                    while (!craft && k == randFloor)
                                    {
                                        x = i + WorldGen.genRand.Next(randX - 3);
                                        if (house[k][x, j] == 0)
                                        {
                                            house[k][x, j] = ID.Useful;
                                            house[k][x + 1, j] = ID.Empty;
                                            craft = true;
                                        }
                                    }
                                    while (!furniture && k != 1)
                                    {
                                        x = i + WorldGen.genRand.Next(randX - 3);
                                        if (x % 2 == 0)
                                            if (house[k][x, j] == 0)
                                            {
                                                house[k][x, j] = ID.Furniture;
                                                house[k][x + 1, j] = ID.Empty;
                                                furniture = true;
                                            }
                                    }
                                    while (!chest && k == randFloor)
                                    {
                                        x = i + WorldGen.genRand.Next(randX - 3);
                                        if (house[k][x, j] == 0)
                                        {
                                            house[k][x, j] = ID.Chest;
                                            house[k][x + 1, j] = ID.Empty;
                                            chest = true;
                                        }
                                    }
                                }
                            }
                        }
                }
            }
            public bool MagnoHouse(Vector2 origin, bool fail = false)
            {
                if (fail || origin == Vector2.Zero)
                    return false;
                bool success = false;
                int x = (int)origin.X;
                int y = (int)origin.Y;
                if (!ArchaeaWorld.Inbounds(x, y))
                    return false;
                int m = 0;
                int n = 0;
                int height = 0;
                int randFloor = WorldGen.genRand.Next(max);
                for (int k = 0; k < max; k++)
                {
                    int lengthX = house[k].GetLength(0);
                    int lengthY = house[k].GetLength(1);
                    for (int i = 0; i < lengthX; i++)
                        for (int j = 0; j < lengthY; j++)
                        {
                            m = i + x;
                            n = j + y + height;
                            Tile tile = Main.tile[m, n];
                            if (tile.WallType == wallID || tile.TileType == tileID)
                                return false;
                            if (i >= 0 && i < lengthX && j >= 0 && j < lengthY)
                                if (WorldGen.genRand.NextFloat() < 0.50f)
                                {
                                    tile.WallType = wallID;
                                    WorldGen.PlaceWall(m, n, wallID, true);
                                }
                            if (house[k][i, j] == tileID)
                            {
                                tile.HasTile = true;
                                tile.TileType = tileID;
                            }
                            else tile.HasTile = false;
                            if (tile.TileType == tileID && tile.HasTile)
                                success = true;
                            if (i > 6 && i < 14)
                            {
                                if (k == 0 && (j == 0 || j == lengthY - 1))
                                {
                                    tile.TileType = TileID.Platforms;
                                    WorldGen.SquareTileFrame(m, n, true);
                                }
                                if (k == max - 1 && j == 0)
                                {
                                    tile.TileType = TileID.Platforms;
                                    WorldGen.SquareTileFrame(m, n, true);
                                }
                                if (direction[k] && i == 7)
                                    tile.Slope = (SlopeType)1;
                                else if (!direction[k] && i == 13)
                                    tile.Slope = (SlopeType)2;
                            }
                        }
                    for (int i = 0; i < lengthX; i++)
                        for (int j = 0; j < lengthY; j++)
                        {
                            m = i + x;
                            n = j + y + height;
                            ushort type;
                            switch (house[k][i, j])
                            {
                                case ID.Door:
                                    if (!Treasures.ActiveAndSolid(m - 1, n) && !Treasures.ActiveAndSolid(m + 1, n))
                                        WorldGen.PlaceDoor(m, n, TileID.ClosedDoor);
                                    else
                                    {
                                        for (int t = -1; t <= 1; t++)
                                        {
                                            Tile tile = Main.tile[m, n + t];
                                            tile.HasTile = true;
                                            Main.tile[m, n + t].TileType = tileID;
                                        }
                                    }
                                    break;
                                case ID.Stairs:
                                    WorldGen.KillTile(m, n, false, false, true);
                                    WorldGen.PlaceTile(m, n, TileID.Platforms);
                                    Tile t0 = Main.tile[m, n];
                                    t0.Slope = (SlopeType)(direction[k] ? 1 : 2);
                                    WorldGen.SquareTileFrame(m, n, true);
                                    break;
                                case ID.Lamp:
                                    if (Main.rand.NextBool(4))
                                    { 
                                        WorldGen.PlaceTile(m, n, TileID.HangingLanterns);
                                    }
                                    break;
                                case ID.Chest:
                                    WorldGen.PlaceChest(m, n, ArchaeaWorld.magnoChest);
                                    break;
                                case ID.Furniture:
                                    type = furniture[WorldGen.genRand.Next(furniture.Length)];
                                    if (!Treasures.Vicinity(origin, 50, type))
                                        WorldGen.PlaceTile(m, n, type);
                                    break;
                                case ID.Useful:
                                    type = useful[WorldGen.genRand.Next(useful.Length)];
                                    if (!Treasures.Vicinity(origin, 50, type))
                                        WorldGen.PlaceTile(m, n, type);
                                    break;
                                case ID.Decoration:
                                    type = decoration[WorldGen.genRand.Next(decoration.Length)];
                                    if (Treasures.ProximityCount(origin, 50, type) < 5)
                                        WorldGen.PlaceTile(m, n, type);
                                    break;
                                default:
                                    break;
                            }
                        }
                    height += lengthY;
                }
                index++;
                return success;
            }
        }
        internal void PlaceChest(int i, int j, int width, ushort groundID)
        {
            i -= width;
            j -= 1;
            bool chest = false;
            int count = 0;
            int total = 100;
            while (!chest)
            {
                int m = i + WorldGen.genRand.Next(width - 1);
                Tile floor = Main.tile[m, j];
                Tile ground = Main.tile[m, j + 1];
                if (!floor.HasTile && ground.TileType == groundID)
                {
                    WorldGen.PlaceChest(m, j);
                    if (floor.TileType == ArchaeaWorld.magnoChest)
                        chest = true;
                }
                if (count < total)
                    count++;
                else break;
            }
        }
        internal void Decorate(int i, int j, int radius, ushort tileType)
        {
            var t = new Treasures();
            Vector2 v2 = new Vector2(i, j);
            var floor = Treasures.GetFloor(v2, radius, radius, false, new ushort[] { tileType, TileID.Platforms });
            var ceiling = Treasures.GetCeiling(v2, radius, false, tileType);
            ushort[] decoration = new ushort[] { TileID.Statues, TileID.Pots };
            ushort[] furniture = new ushort[] { TileID.Tables, TileID.Chairs, TileID.Pianos, TileID.GrandfatherClocks, TileID.Dressers };
            ushort[] useful = new ushort[] { TileID.Loom, TileID.SharpeningStation, TileID.Anvils, TileID.CookingPots };
            int length = 0;
            foreach (ushort tile in furniture)
                t.PlaceTile(floor, 2, 30, tile, true, false, false, 0, true, 40);
            length = WorldGen.genRand.Next(useful.Length);
            t.PlaceTile(floor, 1, 30, useful[length], true, false, false, 0, true, 40);
            foreach (ushort tile in decoration)
                t.PlaceTile(floor, 4, 20, tile, true);
            t.PlaceTile(ceiling, 3, 1, TileID.HangingLanterns, true, true);
            t = null;
        }
        private void GenerateWalls(int x, int y, int width, int height)
        {
            for (int i = x; i < width; i++)
            {
                for (int j = y + 2; j < height; j++)
                {
                    WorldGen.PlaceWall(i, j, wallID, true);
                }
            }
        }
        public void Reset()
        {
            index = 0;
        }
    }
}