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
using Terraria.Audio;
using Terraria.DataStructures;
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
using Humanizer;
using ArchaeaMod.NPCs.Bosses;
using System.Runtime.CompilerServices;

namespace ArchaeaMod.Factory
{
    public class Room
    {
        public Room(int type)
        {
            this.type = type;
        }
        public int type;
        public bool isHeated;
        int Top => Factory.Top + bound.Y;
        int Right => bound.X + bound.Width;
        int Bottom => Top + bound.Height;
        int Left => bound.X;
        Rectangle hitbox => new Rectangle(Left, Top, bound.Width, bound.Height);
        
        public Rectangle bound;
        public void Build()
        {
            bool flag = Terraria.WorldGen.genRand.NextBool();
            int offX = 0;
            bool placed = false;
            int X1 = Left;
            int X2 = Right;
            int Y1 = Top;
            int Y2 = Bottom;
            for (int i = X1; i < X2; i++)
            {
                for (int j = Y1; j < Y2; j++)
                {
                    switch (type)
                    {
                        case RoomID.Empty:
                            goto case RoomID.Challenge;
                        case RoomID.Simple:
                            if (IsBottom(j) && i % 2 == 0)
                            {
                                Terraria.WorldGen.PlaceTile(i, j, TileID.Spikes);
                            }
                            break;
                        case RoomID.Trapped:
                            if (IsBottom(j))
                            {
                                if (!placed && i % 4 == 0)
                                {
                                    Terraria.WorldGen.placeTrap(i, j);
                                    placed = true;
                                }
                                if (Terraria.WorldGen.genRand.NextBool(4))
                                {
                                    Terraria.WorldGen.placeTrap(i, j);
                                }
                            }
                            break;
                        case RoomID.Challenge:
                            if (!placed && IsTop(j) && IsCenter(i))
                            {
                                for (int m = 0; m < Structures.cage.GetLength(0); m++)
                                {
                                    for (int n = 0; n < Structures.cage.GetLength(1); n++)
                                    {
                                        int x = i + m;
                                        int y = j + n;
                                        switch (Structures.cage[m, n])
                                        { 
                                            case Structures.TILE_Chain:
                                                for (int l = 0; l < 6; l++)
                                                {
                                                    Terraria.WorldGen.PlaceTile(x, y - l, TileID.Chain);
                                                }
                                                break;
                                            case Structures.TILE_Brick:
                                                Terraria.WorldGen.PlaceTile(x, y, ArchaeaWorld.factoryBrick);
                                                break;
                                        }
                                    }
                                }
                                placed = true;
                            }
                            if (IsBottom(j) && i % 2 == 0)
                            {
                                Terraria.WorldGen.PlaceTile(i, j, TileID.Spikes);
                            }
                            break;
                        case RoomID.Pillars:
                            if (IsBottom(j))
                            {
                                if (i % 4 == 0)
                                {
                                    Terraria.WorldGen.PlaceTile(i, j, TileID.Statues, style: 36);
                                }
                            }
                            break;
                        case RoomID.Webbed:
                            if (Terraria.WorldGen.genRand.NextBool(4))
                            {
                                Terraria.WorldGen.PlaceTile(i, j - 1, TileID.Sand, true);
                                Terraria.WorldGen.PlaceTile(i, j, TileID.Cobweb, true);
                            }
                            break;
                        case RoomID.MonsterDen:
                            if (IsBottom(i) && i % 3 == 0)
                            {
                                Terraria.WorldGen.PlaceStatueTrap(i, j);
                            }
                            break;
                        case RoomID.Camp:
                            if (IsCenter(i - 1) && IsBottom(j))
                            { 
                                Terraria.WorldGen.Place3x2(i, j, TileID.Campfire, 7);
                                return;
                            }
                            break;
                        case RoomID.Lighted:
                            if (IsTop(j) && i > X1 + 3 && i < X2 - 4)
                            {
                                Terraria.WorldGen.PlaceTile(i, j, ModContent.TileType<Tiles.m_chandelier>());
                            }
                            break;
                        case RoomID.Dais:
                            if (IsTop(j) && i > X1 + 3 && i < X2 - 4)
                            {
                                Terraria.WorldGen.PlaceTile(i, j, ModContent.TileType<Tiles.m_chandelier>());
                            }
                            if (IsRight(i) && IsBottom(j))
                            { 
                                offX = 2;
                                Terraria.WorldGen.PlaceTile(i     - offX, j, ArchaeaWorld.factoryBrick);
                                Terraria.WorldGen.PlaceTile(i - 1 - offX, j, ArchaeaWorld.factoryBrick);
                                Terraria.WorldGen.PlaceTile(i - 2 - offX, j, ArchaeaWorld.factoryBrick);
                                Terraria.WorldGen.PlaceTile(i     - offX, j - 1, ArchaeaWorld.factoryBrick);
                                Terraria.WorldGen.PlaceTile(i - 1 - offX, j - 1, ArchaeaWorld.factoryBrick);
                                Terraria.WorldGen.Place2x1(i, j - 2, (ushort)ModContent.TileType<Tiles.m_chair>());
                                return;
                            }
                            break;
                        case RoomID.Mausoleum:
                            if (IsBottom(j))
                            {
                                if (i % 2 == 0)
                                {
                                    if (Main.rand.NextBool(3))
                                    {
                                        Terraria.WorldGen.Place2x2(i, j - 1, TileID.Tombstones, Main.rand.Next(11));
                                    }
                                }    
                            }
                            break;
                        case RoomID.Heated:
                            isHeated = true;
                            Main.tile[i, j].WallType = WallID.HellstoneBrickUnsafe;
                            break;
                    }
                }
            }
        }
        private bool IsBorder(int i, int j)
        {
            return i == Left || i == Right || j == Top || j == Bottom;
        }
        private bool IsTop(int j)
        {
            return j == Top;
        }
        private bool IsBottom(int j)
        {
            return j == Bottom;
        }
        private bool IsRight(int i)
        {
            return i == Right;
        }
        private bool IsLeft(int i)
        {
            return i == Left;
        }
        private bool IsCenter(int i)
        {
            return i == bound.X + bound.Width / 2;
        }
        public void Update(Player player)
        {
            if (isHeated)
            {
                if (hitbox.Contains(player.Hitbox))
                {
                    player.AddBuff(BuffID.OnFire, 300, false);
                    if (ArchaeaItem.Elapsed(180))
                    {
                        SoundEngine.PlaySound(SoundID.Item8); 
                        player.Hurt(PlayerDeathReason.LegacyDefault(), 10, 0);
                    }
                }
            }
        }
    }

    public sealed class RoomID
    {
        public const byte Total = 12;
        public const byte
            Empty = 0,
            Simple = 1,
            Trapped = 2,
            Challenge = 4,
            Pillars = 5,
            Webbed = 6,
            MonsterDen = 7,
            Camp = 8,
            Lighted = 9,
            Dais = 10,
            Mausoleum = 11,
            Heated = 12;
    }
}
