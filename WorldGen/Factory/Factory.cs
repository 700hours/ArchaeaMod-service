using System;
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

namespace ArchaeaMod.Factory
{
    public class Factory
    {
        private static int
            Width = 150, 
            Height;
        internal static int
            Top => Main.UnderworldLayer - Height;
        public static ushort
            Air = 1,
            Tile = ArchaeaWorld.factoryBrick,
            Wall = ArchaeaWorld.factoryBrickWallUnsafe,
            Tile2 = ArchaeaWorld.Ash,
            ConveyerL = TileID.ConveyorBeltLeft,
            ConveyerR = TileID.ConveyorBeltRight;
        public static IList<Room> room = new List<Room>();
        public void CastleGen(out ushort[,] tile, out ushort[,] background, int width, int height, int size = 4, int maxNodes = 50, float nodeDistance = 60)
        {
            Width = width;
            Height = height;

            var brush = new ushort[width + size * 2, height + size * 2];
            background = new ushort[width, height];

            Vector2[] nodes = new Vector2[maxNodes];
            int numNodes = 0;

            //  Generating vector nodes
            int randX = 0,
                randY = 0;
            while (numNodes < maxNodes)
            {
                foreach (Vector2 node in nodes)
                {
                    do
                    {
                        randX = Main.rand.Next(size, width);
                        randY = Main.rand.Next(size, height - size * 4);
                        nodes[numNodes] = new Vector2(randX, randY);
                    } while (nodes.All(t => t.Distance(nodes[numNodes]) < nodeDistance));
                    numNodes++;
                }
            }

            //  Carve out rooms
            int W = 0, H = 0;
            int maxSize = 7;
            int border = 8;
            foreach (Vector2 node in nodes)
            {
                Room r = new Room(Main.rand.Next(RoomID.Total));
                int rand = 0;//Main.rand.Next(2);
                switch (rand)
                {
                    case 0:
                        W = Main.rand.Next(4, maxSize) * size;
                        H = Main.rand.Next(4, maxSize) * size;
                        //  Room construction
                        int X1 = (int)node.X - W / 2;
                        int X2 = (int)node.X + W / 2;
                        int Y1 = (int)node.Y - H / 2;
                        int Y2 = (int)node.Y + H / 2;
                        r.bound = new Rectangle(X1 - border, Y1 - border, W + border, H + border);
                        if (room.FirstOrDefault(t => t.bound.Intersects(r.bound)) != default)
                        {
                            continue;
                        }
                        for (int i = X1 - border; i < X2 + border; i++)
                        {
                            for (int j = Y1 - border; j < Y2 + border; j++)
                            {
                                //  If tile in-bounds
                                if (i > 0 && j > 0 && i < width && j < height)
                                {
                                    if (i < brush.GetLength(0) && j < brush.GetLength(1))
                                    {
                                        brush[i, j] = Air;
                                        if (i <= X1 || i >= X2 || j <= Y1 || j >= Y2)
                                        {
                                            if (i > X1 && i < X2 && j >= Y2)
                                            {
                                                //  Floor
                                                brush[i, j] = Tile2;
                                                continue;
                                            }
                                            //  Ceiling and walls
                                            brush[i, j] = Tile;
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                        room.Add(r);
                        break;
                    default:
                        break;
                }
            }

            //  Generating hallways
            nodes = nodes.OrderBy(t => t.Distance(new Vector2(0, height / 2))).ToArray();
            for (int k = 1; k < nodes.Length; k++)
            {
                int X, Y;
                Vector2 start,
                        end;

                //  Normal pass
                start = nodes[k - 1];
                end = nodes[k];

                #region Hallway carving
                if (start.X < end.X && start.Y < end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (Y++ <= (start.Y + end.Y) / 2 + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                }
                else if (start.X > end.X && start.Y < end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (Y++ <= (start.Y + end.Y) / 2 + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                }
                else if (start.X < end.X && start.Y > end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (X++ <= (start.X + end.X) / 2 + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                }
                else if (start.X > end.X && start.Y > end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (X++ <= (start.X + end.X) / 2 + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                }
                #endregion
                #region Reversed pass
                start = nodes[k];
                end = nodes[k - 1];

                if (start.X < end.X && start.Y < end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (Y++ <= (start.Y + end.Y) / 2 + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                    }
                }
                else if (start.X > end.X && start.Y < end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (Y++ <= (start.Y + end.Y) / 2 + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                    }
                }
                else if (start.X < end.X && start.Y > end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (X++ <= (start.X + end.X) / 2 + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                }
                else if (start.X > end.X && start.Y > end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (X++ <= (start.X + end.X) / 2 + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(ref brush, ref background, X, Y, 6);
                        
                    }
                }
                #endregion
            }

            //  Clear center column
            for (int i = 0; i < brush.GetLength(0); i++)
            {
                for (int j = 0; j < brush.GetLength(1); j++)
                {
                    int cx  = brush.GetLength(0) / 2 - 10;
                    int cx2 = brush.GetLength(0) / 2 + 10;
                    if (i >= cx && i <= cx2)
                    {
                        brush[i, j] = Air;
                    }
                }
            }
            
            //  Return value
            tile = brush;
        }
        private void CarveHall(ref ushort[,] tile, ref ushort[,] wall, int x, int y, int size = 10)
        {
            int border = 4;
            for (int i = -border; i < size + border; i++)
            {
                for (int j = -border; j < size + border; j++)
                {
                    int X = Math.Max(0, Math.Min(x + i, Width - 1));
                    int Y = Math.Max(0, Math.Min(y + j, Height - 1));
                    var r = room.FirstOrDefault(t => t.bound.Intersects(new Rectangle(X, Y, size + border, size + border)));
                    if (r != default)
                    {
                        continue;
                    }
                    if (wall[X, Y] != Wall)
                    { 
                        tile[X, Y] = Tile;
                    }
                }
            }
            bool flag = WorldGen.genRand.NextBool(4);
            bool flag2 = WorldGen.genRand.NextBool();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int X = Math.Max(0, Math.Min(x + i, Width - 1));
                    int Y = Math.Max(0, Math.Min(y + j, Height - 1));
                    tile[X, Y] = Air;
                    wall[X, Y] = Wall;
                    if (flag && j == size - 1)
                    {
                        tile[X, Y] = flag2 ? ConveyerL : ConveyerR;
                    }
                }
            }
        }
    }
}
