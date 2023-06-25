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
        public void Build(int buffer = 10)
        {
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
                    //  Set wall type for room
                    i = Math.Max(buffer, Math.Min(Main.maxTilesX - buffer, i));
                    j = Math.Max(buffer, Math.Min(Main.maxTilesY - buffer, j));

                    Main.tile[i, j].WallType = Factory.Wall;

                    switch (type)
                    {
                        case RoomID.Empty:
                            goto case RoomID.Challenge;
                        case RoomID.Simple:
                            if (IsBottom(j) && i % 2 == 0)
                            {
                                if (Main.tile[i, j + 1].HasTile)
                                { 
                                    Terraria.WorldGen.PlaceTile(i, j, TileID.Spikes, true, true);
                                }
                            }
                            goto default;
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
                            goto default;
                        case RoomID.Challenge:
                            if (!placed && IsTop(j) && IsCenter(i))
                            {
                                for (int m = 0; m < Structures.cageSafe.GetLength(1); m++)
                                {
                                    for (int n = 0; n < Structures.cageSafe.GetLength(0); n++)
                                    {
                                        int x = i + m;
                                        int y = j + n;
                                        switch (Structures.cageSafe[m, n])
                                        { 
                                            case Structures.TILE_Chain:
                                                for (int l = 0; l < 6; l++)
                                                {
                                                    Terraria.WorldGen.PlaceTile(x, y - l, TileID.Chain, true, true);
                                                }
                                                break;
                                            case Structures.TILE_Brick:
                                                Terraria.WorldGen.PlaceTile(x, y, ArchaeaWorld.factoryBrick, true, true);
                                                break;
                                            default:
                                                Tile tile = Main.tile[x, y];
                                                tile.HasTile = false;
                                                break;
                                        }
                                    }
                                }
                                placed = true;
                            }
                            if (IsBottom(j) && i % 2 == 0)
                            {
                                if (Main.tile[i, j + 1].HasTile)
                                {
                                    Terraria.WorldGen.PlaceTile(i, j, TileID.Spikes, true, true);
                                }
                            }
                            goto default;
                        case RoomID.Pillars:
                            if (IsBottom(j))
                            {
                                if (i % 4 == 0)
                                {
                                    Terraria.WorldGen.PlaceTile(i, j, TileID.Statues, true, true, style: 36);
                                }
                            }
                            goto default;
                        case RoomID.Webbed:
                            if (Terraria.WorldGen.genRand.NextBool(4))
                            {
                                if (i > X1 + 8 && j > Y1 + 8)
                                { 
                                    Terraria.WorldGen.PlaceTile(i, j - 1, TileID.Sand, true, true);
                                    Terraria.WorldGen.PlaceTile(i, j, TileID.Cobweb, true, true);
                                }
                            }
                            goto default;
                        case RoomID.MonsterDen:
                            if (IsBottom(j) && i % 3 == 0)
                            {
                                Terraria.WorldGen.PlaceStatueTrap(i, j);
                            }
                            goto default;
                        case RoomID.Camp:
                            if (IsCenter(i - 1) && IsBottom(j))
                            { 
                                Terraria.WorldGen.Place3x2(i, j, TileID.Campfire, 7);
                                return;
                            }
                            goto default;
                        case RoomID.Lighted:
                            if (IsTop(j) && IsCenter(i))
                            {
                                Terraria.WorldGen.PlaceTile(i, j, ModContent.TileType<Tiles.m_chandelier>(), true, true);
                            }
                            goto default;
                        case RoomID.Dais:
                            if (IsTop(j) && IsCenter(i))
                            {
                                Terraria.WorldGen.PlaceTile(i, j, ModContent.TileType<Tiles.m_chandelier>(), true, true);
                            }
                            if (IsRight(i) && IsBottom(j))
                            { 
                                offX = 2;
                                Terraria.WorldGen.PlaceTile(i     - offX, j, ArchaeaWorld.factoryBrick, true, true);
                                Terraria.WorldGen.PlaceTile(i - 1 - offX, j, ArchaeaWorld.factoryBrick, true, true);
                                Terraria.WorldGen.PlaceTile(i - 2 - offX, j, ArchaeaWorld.factoryBrick, true, true);
                                Terraria.WorldGen.PlaceTile(i     - offX, j - 1, ArchaeaWorld.factoryBrick, true, true);
                                Terraria.WorldGen.PlaceTile(i - 1 - offX, j - 1, ArchaeaWorld.factoryBrick, true, true);
                                Terraria.WorldGen.PlaceTile(i     - offX, j - 2, (ushort)ModContent.TileType<Tiles.m_chair>(), true, true);
                                return;
                            }
                            goto default;
                        case RoomID.Mausoleum:
                            if (IsBottom(j))
                            {
                                if (i % 2 == 0)
                                {
                                    if (Main.rand.NextBool(2))
                                    {
                                        Terraria.WorldGen.PlaceTile(i, j, TileID.Tombstones, true, true, -1, Main.rand.Next(11));
                                    }
                                }    
                            }
                            goto default;
                        case RoomID.Heated:
                            isHeated = true;
                            Main.tile[i, j].WallType = WallID.HellstoneBrickUnsafe;
                            if (j < Bottom - 4)
                            {
                                WorldGen.PlaceLiquid(i, j, LiquidID.Water, 255);
                            }
                            goto default;
                        default:
                            if (!placed && IsBottom(j))
                            { 
                                WorldGen.PlaceChest(i, j, notNearOtherChests: true);
                                if (IsPlaced(i, j, TileID.Containers))
                                { 
                                    placed = true;
                                }
                            }
                            break;
                    }
                }
            }
        }
        private bool IsPlaced(int i, int j, ushort tile)
        {
            return Main.tile[i, j].TileType == tile && Main.tile[i, j].HasTile;
        }
        private bool IsBorder(int i, int j)
        {
            return i == Left || i == Right || j == Top || j == Bottom;
        }
        private bool IsTop(int j)
        {
            return j - 1 == Top;
        }
        private bool IsBottom(int j)
        {
            return j + 1 == Bottom;
        }
        private bool IsRight(int i)
        {
            return i + 1 == Right;
        }
        private bool IsLeft(int i)
        {
            return i - 1 == Left;
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
        public const byte Total = 13;
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
