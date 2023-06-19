using ArchaeaMod.Progression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArchaeaMod.Gen.Sector;
using tUserInterface.Extension;

using Microsoft.Xna.Framework;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

using ArchaeaMod;
using ArchaeaMod.NPCs;

namespace ArchaeaMod.Gen
{
    internal class Factory
    {
        public static ushort Air = 0;
        public static ushort Tile = ArchaeaWorld.factoryBrick;
        public static ushort Wall = ArchaeaWorld.factoryBrickWallUnsafe;
        public void CastleGen(out ushort[,] tile, out ushort[,] background, int width, int height, int size = 16, int maxNodes = 5, float range = 300f, float nodeDistance = 800f)
        {
            //  Constructing values
            width += width % size;
            height += height % size;
            var brush = new ushort[width, height];
            background = new ushort[width, height];

            Vector2[] nodes = new Vector2[maxNodes];
            int numNodes = 0;

            //  Filling entire space with brushes
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    brush[i, j] = ArchaeaWorld.factoryBrick;
                    if (i > 0 && i < width - 1 && j > 0 && j < height - 1)
                    {
                        brush[i, j] = Tile;
                        background[i, j] = Wall;
                    }
                }
            }

            //  Generating vector nodes
            int randX = 0,
                randY = 0;
            while (numNodes < maxNodes)
            {
                foreach (Vector2 node in nodes)
                {
                    do
                    {
                        randX = Main.rand.Next(size, width - size);
                        randY = Main.rand.Next(size, height - size);
                        nodes[numNodes] = new Vector2(randX, randY);
                    } while (nodes.All(t => Vector2.Distance(t, nodes[numNodes]) < nodeDistance));
                    numNodes++;
                }
            }

            //  Carve out rooms
            int W = 0, H = 0;
            int maxSize = 7 * size;
            int index = 0;
            foreach (Vector2 node in nodes)
            {
                Room room = new Room((short)Main.rand.Next(RoomType.MAX));
                int rand = 0;//Main.rand.Next(2);
                switch (rand)
                {
                    case 0:
                        W = Main.rand.Next(4, maxSize) * size;
                        H = Main.rand.Next(4, maxSize) * size;
                        //  Room construction
                        room.region = new ushort[W / size, H / size];
                        room.bounds = new Rectangle((int)node.X - W / 2, (int)node.Y - H / 2, W, H);
                        for (int i = (int)node.X - W / 2; i < node.X + W / 2; i++)
                        {
                            for (int j = (int)node.Y - H / 2; j < node.Y + H / 2; j++)
                            {
                                if (i > 0 && j > 0 && i < width && j < height)
                                {
                                    if (i / size < brush.GetLength(0) && j / size < brush.GetLength(1))
                                    {
                                        brush[i / size, j / size] = Air;
                                        //room.region[i / size, j / size] = Air;
                                    }
                                }
                            }
                        }
                        Room.room.Add(room);
                        break;
                    case 1:
                        //  Room construction
                        /*
                        room.region = new short[room0.GetLength(0), room0.GetLength(1)];
                        room.bounds = new Rectangle((int)node.X, (int)node.Y, room0.GetLength(0) * size, room0.GetLength(1) * size);
                        for (int i = 0; i < room0.GetLength(0); i++)
                        {
                            for (int j = 0; j < room0.GetLength(1); j++)
                            {
                                W = i * size + (int)node.X;
                                H = j * size + (int)node.Y;
                                W += W % size;
                                H += H % size;
                                if (W > 0 && H > 0 && W < width && H < height)
                                {
                                    if (W / size < brush.GetLength(0) && H / size < brush.GetLength(1))
                                    {
                                        Tile tile;
                                        (tile = brush[W / size, H / size]) = Air;
                                        //room.region[W / size, H / size] = tile;
                                    }
                                }
                            }
                        }
                        room.InitRoom();
                        room.Add(index++, room);*/
                        break;
                    default:
                        break;
                }
            }

            //  Generating hallways
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
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                }
                else if (start.X > end.X && start.Y < end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (Y++ <= (start.Y + end.Y) / 2 + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                }
                else if (start.X < end.X && start.Y > end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (X++ <= (start.X + end.X) / 2 + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                }
                else if (start.X > end.X && start.Y > end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (X++ <= (start.X + end.X) / 2 + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                }

                //  Reversed pass
                start = nodes[k];
                end = nodes[k - 1];

                if (start.X < end.X && start.Y < end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (Y++ <= (start.Y + end.Y) / 2 + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                }
                else if (start.X > end.X && start.Y < end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (Y++ <= (start.Y + end.Y) / 2 + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                }
                else if (start.X < end.X && start.Y > end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (X++ <= (start.X + end.X) / 2 + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                }
                else if (start.X > end.X && start.Y > end.Y)
                {
                    X = (int)start.X + (int)start.X % size;
                    Y = (int)start.Y + (int)start.Y % size;

                    while (X++ <= (start.X + end.X) / 2 + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (Y++ <= end.Y + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                    while (X++ <= end.X + size)
                    {
                        CarveHall(brush, X, Y);
                        //brush[Math.Min(X / size, width / size - 1), Math.Min(Y / size, height / size - 1)] = Air;
                    }
                }
            }
                #endregion

                //  Placing doors
                /*
                bool flag = false;
                bool randFlagX = Main.rand.NextBool();
                bool randFlagY = Main.rand.NextBool();
                for (int i = randFlagX ? -20 : 20; i < (randFlagX ? 20 : -20); i += randFlagX ? 1 : -1)
                {
                    for (int j = randFlagY ? -20 : 20; j < (randFlagY ? 20 : -20); j += randFlagY ? 1 : -1)
                    {
                        X = (int)(start.X + (int)start.X % size + i * size) / size;
                        Y = (int)(start.Y + (int)start.Y % size + j * size) / size;
                        //  Bottom
                        if (!Tile.GetSafely(X, Y, width, height, brush).Active && !Tile.GetSafely(X + 1, Y - 1, width, height, brush).Active && !Tile.GetSafely(X - 1, Y - 1, width, height, brush).Active && Tile.GetSafely(X - 1, Y, width, height, brush).Active && Tile.GetSafely(X + 1, Y, width, height, brush).Active && !Tile.GetSafely(X, Y + 1, width, height, brush).Active && !Tile.GetSafely(X, Y - 1, width, height, brush).Active)
                        {
                            Door.CreateDoor(false, Tile.GetSafely(X, Y, width, height, brush), Direction.Bottom);
                            flag = true;
                            break;
                        }
                        //  Top
                        if (!Tile.GetSafely(X, Y, width, height, brush).Active && !Tile.GetSafely(X + 1, Y + 1, width, height, brush).Active && !Tile.GetSafely(X - 1, Y + 1, width, height, brush).Active && Tile.GetSafely(X - 1, Y, width, height, brush).Active && !Tile.GetSafely(X + 1, Y, width, height, brush).Active && !Tile.GetSafely(X, Y + 1, width, height, brush).Active && !Tile.GetSafely(X, Y - 1, width, height, brush).Active)
                        {
                            Door.CreateDoor(false, Tile.GetSafely(X, Y, width, height, brush), Direction.Top);
                            flag = true;
                            break;
                        }
                        //  Right 
                        if (!Tile.GetSafely(X, Y, width, height, brush).Active && !Tile.GetSafely(X + 1, Y + 1, width, height, brush).Active && !Tile.GetSafely(X + 1, Y - 1, width, height, brush).Active && Tile.GetSafely(X, Y + 1, width, height, brush).Active && Tile.GetSafely(X, Y - 1, width, height, brush).Active && !Tile.GetSafely(X - 1, Y, width, height, brush).Active && !Tile.GetSafely(X + 1, Y, width, height, brush).Active)
                        {
                            Door.CreateDoor(false, Tile.GetSafely(X, Y, width, height, brush), Direction.Right);
                            flag = true;
                            break;
                        }
                        //  Left
                        if (!Tile.GetSafely(X, Y, width, height, brush).Active && !Tile.GetSafely(X - 1, Y + 1, width, height, brush).Active && !Tile.GetSafely(X - 1, Y - 1, width, height, brush).Active && Tile.GetSafely(X, Y + 1, width, height, brush).Active && Tile.GetSafely(X, Y - 1, width, height, brush).Active && !Tile.GetSafely(X - 1, Y, width, height, brush).Active && !Tile.GetSafely(X + 1, Y, width, height, brush).Active)
                        {
                            Door.CreateDoor(false, Tile.GetSafely(X, Y, width, height, brush), Direction.Left);
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                        break;
                }
            }*/

            //  Map boundaries
            int m = 0, n = 0;
            while (true)
            {
                for (int i = 0; i < width; i += size)
                {
                    if (i / size < brush.GetLength(0) && m / size < brush.GetLength(1))
                    {
                        brush[i / size, m / size] = Tile;
                        //brush[i / size, m / size].width += 1;
                        //brush[i / size, m / size].height += 1;
                    }
                }
                if (m < height - size)
                {
                    m = height - size;
                    continue;
                }
                break;
            }
            while (true)
            {
                for (int j = 0; j < height; j += size)
                {
                    if (n / size < brush.GetLength(0) && j / size < brush.GetLength(1))
                        brush[n / size, j / size] = Tile;
                }
                if (n < width - size)
                {
                    n = width - size;
                    continue;
                }
                break;
            }

            tile = brush;
            return;
            #region TODO placing world objects
            int num = 0;
            int numTraps = 0;
            int numDown = 0, numUp = 0;
            int numChests = 0;
            int numItems = 0;
            int numNPCs = 0;
            int numTorches = 0;
            const float mult = 1.5f;
            /*
            //SquareBrush.InitializeArray(brush.Length);
            while (numDown == 0 || numUp == 0 || numTorches < maxTorches || numItems < maxItems)
            {
                foreach (var b in brush)
                {
                    //  Adding background objects
                    if (!b.Active) new Background(b.X / size, b.Y / size, size);

                    //  Adding tile objects
                    for (int k = 0; k < nodes.Length; k++)
                    {
                        if (!b.Active && Main.Distance(nodes[k], b.Center) < range * mult)
                        {
                            Vector2 randv2 = Vector2.Zero;
                            do
                            {
                                randX = Main.rand.Next(size, width - size);
                                randY = Main.rand.Next(size, height - size);
                                randv2 = new Vector2(randX, randY);
                            } while (brush[randX / size, randY / size].Active);
                            int rand = Main.rand.Next(8);
                            randv2.X -= randv2.X % size;
                            randv2.Y -= randv2.Y % size;
                            switch (rand)
                            {
                                case TileID.Empty:
                                    break;
                                case TileID.Item:
                                    if (numItems++ < 12)
                                    {
                                        Item.NewItem(randv2.X, randv2.Y, 32, 32, (short)(Main.rand.Next(9) + 1));
                                    }
                                    break;
                                case TileID.Torch:
                                    //  Unoptimized: causes large slowdown
                                    if (numTorches++ < maxTorches)
                                    {
                                        int offsetX = Main.rand.Next(Tile.Size);
                                        int offsetY = Main.rand.Next(Tile.Size);
                                        index = Lamp.NewLamp(new Vector2(randv2.X + offsetX, randv2.Y + offsetY), 200f, Lamp.TorchLight, b, true);
                                        Main.lamp[index].active = true;
                                        Main.lamp[index].owner = 255;
                                        //b.lamp = Main.lamp[index];
                                    }
                                    break;
                                case TileID.Monster:
                                    if (numNPCs++ < 12)
                                        Npc.NewNPC((int)randv2.X + randv2.X % Tile.Size + Tile.Size / 10, (int)randv2.Y + randv2.Y % Tile.Size + Tile.Size / 10, NpcType.Kobold);
                                    break;
                                case TileID.Trap:
                                    if (numTraps++ < 10)
                                        Trap.NewTrap((int)randv2.X, (int)randv2.Y, size, size, (short)(Main.rand.Next(TrapID.Sets.Total - 1) + 1));
                                    break;
                                case TileID.Chest:
                                    if (numChests++ < 3)
                                        Stash.NewStash((int)(randv2.X + randv2.X % Tile.Size + Tile.Size / 10), (int)(randv2.Y + randv2.Y % Tile.Size + Tile.Size / 10), 0, Item.FillStash((int)randv2.X, (int)randv2.Y, Main.rand.Next(3, 12)));
                                    break;
                                case TileID.StairsDown:
                                    if (numDown < 1)
                                    {
                                        //  Place down stairs
                                        Vector2 vector2 = randv2;
                                        var up = Main.staircase.Where(t => t != null && t.direction == StaircaseDirection.LeadingUp);
                                        if (up.Count() > 0)
                                        {
                                            var stair = up.First();
                                            if (Helper.Distance(stair.Center, vector2) > range)
                                            {
                                                Staircase.NewStaircase((int)vector2.X, (int)vector2.Y, stair, StaircaseDirection.LeadingDown);
                                                numDown++;
                                            }
                                        }
                                        else
                                        {
                                            Staircase.NewStaircase((int)vector2.X, (int)vector2.Y, StaircaseDirection.LeadingDown);
                                            numDown++;
                                        }
                                    }
                                    break;
                                case TileID.StairsUp:
                                    if (numUp < 1)
                                    {
                                        //  Place up stairs
                                        Vector2 vector2 = randv2;
                                        var down = Main.staircase.Where(t => t != null && t.direction == StaircaseDirection.LeadingDown);
                                        if (down.Count() > 0)
                                        {
                                            var stair = down.First();
                                            if (Helper.Distance(stair.Center, vector2) > range)
                                            {
                                                Staircase.NewStaircase((int)vector2.X, (int)vector2.Y, stair, StaircaseDirection.LeadingUp);
                                                numUp++;
                                            }
                                        }
                                        else
                                        {
                                            Staircase.NewStaircase((int)vector2.X, (int)vector2.Y, StaircaseDirection.LeadingUp);
                                            numUp++;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion
            //  Pre lighting random rooms
            for (int k = 0; k < Main.room.Count; k++)
            {
                Room room = Main.room[k];
                if (Main.rand.NextFloat() > 0.33f)
                    continue;
                for (int i = room.bounds.Left; i < room.bounds.Right + Tile.Size; i += Tile.Size)
                {
                    for (int j = room.bounds.Top; j < room.bounds.Bottom + Tile.Size; j += Tile.Size)
                    {
                        Background bg = Background.GetSafely(i / Tile.Size, j / Tile.Size);
                        if (bg == null || !bg.active)
                            continue;
                        bg.lit = true;
                    }
                }
            }
            */
            #endregion
        }
        private void CarveHall(ushort[,] tile, int x, int y, int size = 16)
        {
            for (int i = 17; i < size; i++)
            {
                for (int j = 17; j < size; j++)
                {
                    tile[x + i - 16, y + j -16] = Air;
                }
            }
        }
    }
}