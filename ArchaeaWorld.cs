using System;
using System.Collections.Generic;
using System.Drawing;
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
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

using ArchaeaMod.Gen;
using ArchaeaMod.GenLegacy;
using ArchaeaMod.Items;
using ArchaeaMod.Items.Alternate;
using ArchaeaMod.Merged;
using ArchaeaMod.Merged.Items;
using ArchaeaMod.Merged.Tiles;
using ArchaeaMod.Merged.Walls;
using System.IO;

namespace ArchaeaMod
{
    public class ArchaeaWorld : ModSystem
    {
        public static ushort magnoStone
        {
            get { return (ushort)ModContent.TileType<m_stone>(); }
        }
        public static ushort magnoBrick
        {
            get { return (ushort)ModContent.TileType<m_brick>(); }
        }
        public static ushort magnoOre
        {
            get { return (ushort)ModContent.TileType<m_ore>(); }
        }
        public static ushort magnoChest
        {
            get { return (ushort)ModContent.TileType<m_chest>(); }
        }
        public static ushort cOre
        {
            get { return (ushort)ModContent.TileType<c_ore>(); }
        }
        public static ushort crystal
        {
            get { return (ushort)ModContent.TileType<c_crystalsmall>(); }
        }
        public static ushort crystal2x1
        {
            get { return (ushort)ModContent.TileType<c_crystal2x1>(); }
        }
        public static ushort crystal2x2
        {
            get { return (ushort)ModContent.TileType<c_crystal2x2>(); }
        }
        public static ushort crystalLarge
        {
            get { return (ushort)ModContent.TileType<Tiles.c_crystal_large>(); }
        }
        public static ushort magnoBrickWall
        {
            get { return (ushort)ModContent.WallType<magno_brick>(); }
        }
        public static ushort magnoStoneWall
        {
            get { return (ushort)ModContent.WallType<magno_stone>(); }
        }
        public static ushort magnoCaveWall
        {
            get { return (ushort)ModContent.WallType<Walls.magno_cavewall_unsafe>(); }
        }
        public static ushort ambientRocksTopLarge
        {
            get { return (ushort)ModContent.TileType<Tiles.ambient_rocks_1>(); }
        }
        public static ushort ambientRocksTopSmall
        {
            get { return (ushort)ModContent.TileType<Tiles.ambient_rocks_4>(); }
        }
        public static ushort ambientRocksLarge
        {
            get { return (ushort)ModContent.TileType<Tiles.ambient_rocks_2>(); }
        }
        public static ushort ambientRocksSmall
        {
            get { return (ushort)ModContent.TileType<Tiles.ambient_rocks_3>(); }
        }
        public static ushort magnoPlantsLarge
        {
            get { return (ushort)ModContent.TileType<Tiles.m_plants_large>(); }
        }
        public static ushort magnoPlantsSmall
        {
            get { return (ushort)ModContent.TileType<Tiles.m_plants_small>(); }
        }
        public static ushort skyBrick
        {
            get { return (ushort)ModContent.TileType<Tiles.sky_brick>(); }
        }
        public static ushort skyBrickWall
        {
            get { return (ushort)ModContent.WallType<Walls.sky_brickwall_unsafe>(); }
        }
        public static ushort Ash
        {
            get { return (ushort)ModContent.TileType<Tiles.m_ash>(); }
        }
        public static ushort magnoBook
        {
            get { return (ushort)ModContent.TileType<m_book>(); }
        }
        public class ColorID
        {
            public static byte
                Empty = 0,
                Ash = 1,
                Ore = 2,
                Stone = 3,
                Plant = 4;
        }
        public override void OnWorldLoad()
        {
            downedMagno = false;
            downedNecrosis = false;
        }
        //public static System.Drawing.Color[] type = new System.Drawing.Color[]
        //{
        //    System.Drawing.Color.Black,
        //    System.Drawing.Color.White,
        //    System.Drawing.Color.Red,
        //    System.Drawing.Color.Brown,
        //    System.Drawing.Color.Green
        //};
        public bool downedMagno;
        public bool downedNecrosis;
        public int MagnoBiomeOriginX;
        public static Miner miner;
        public static List<Vector2> origins = new List<Vector2>();
        private Treasures t;
        public static Vector2[] genPosition;
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int CavesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Granite")); // Granite
            /*  //  New Magno gen--incomplete implementation
            if (CavesIndex != -1)
            {
                miner = new Miner();
                tasks.Insert(CavesIndex, new PassLegacy("Miner", delegate (GenerationProgress progress)
                {
                    progress.Start(1f);
                    progress.Message = "Magno miner";
                    int width = 400, height = 600;
                    int top = (int)(Main.maxTilesY * 0.75f);
                    int left = WorldGen.genRand.Next(200, Main.maxTilesX - width);
                    Bitmap bmp = (Bitmap)Bitmap.FromFile(@"C:\Users\nolan\OneDrive\Documents\My Games\Terraria\ModLoader\Mods\output.bmp");
                    progress.Message = "Render biome";
                    for (int i = left; i < left + width; i++)
                    {
                        progress.Value += 1f / width;
                        for (int j = top; j < top + height; j++)
                        {
                            if (bmp.GetPixel(i - left, j - top) == type[ColorID.Plant])
                            {
                                Main.tile[i, j].active(true);
                                Main.tile[i, j].type = TileID.JungleGrass;
                            }
                            else if (bmp.GetPixel(i - left, j - top) == type[ColorID.Ash])
                            {
                                Main.tile[i, j].active(true);
                                Main.tile[i, j].type = TileID.Ash;
                            }
                            else if (bmp.GetPixel(i - left, j - top) == type[ColorID.Empty])
                            {
                                Main.tile[i, j].active(false);
                            }
                            else if (bmp.GetPixel(i - left, j - top) == type[ColorID.Stone])
                            {
                                Main.tile[i, j].active(true);
                                Main.tile[i, j].type = magnoStone;
                            }
                            else if (bmp.GetPixel(i - left, j - top) == type[ColorID.Ore])
                            {
                                Main.tile[i, j].active(true);
                                Main.tile[i, j].type = magnoOre;
                            }
                        }
                    }
                    progress.End();
                }));
            }*/
            int originX = 0, originY = 0, mWidth = 800, mHeight = 450;
            int Magno = tasks.FindIndex(genpass => genpass.Name.Equals("Corruption"));
            if (Magno != -1)
            {
                //  NEW implementation
                //tasks.Insert(CavesIndex, new PassLegacy("Magno Lair", delegate (GenerationProgress progress)
                //{
                //    Biome.MagnoBiome.Generate(progress);
                //}));
                //  LEGACY implementation
                //  miner = new Miner();
                tasks.Insert(CavesIndex, new PassLegacy("Magno Caver", delegate (GenerationProgress progress, GameConfiguration c)
                {
                    originX = WorldGen.genRand.Next(200, Main.maxTilesX - 1000);
                    originY = Main.maxTilesY - 650;
                    //  Getting hint 
                    MagnoBiomeOriginX = originX;
                    MagnoV2 magno = MagnoV2.NewBiome(ref originX, ref originY);
                    magno.tGenerate(progress);
                    //  Legacy Magno gen
                    //progress.Start(1f);
                    //progress.Message = "MINER";
                    //miner.active = true;
                    //miner.Reset();
                    //while (miner.active)
                    //    miner.Update();
                    //genPosition = miner.genPos;
                    //progress.End();
                }, 1f));
            }
            int shinies = tasks.FindIndex(pass => pass.Name.Equals("Altars"));
            if (shinies != -1)
            {
                tasks.Insert(shinies, new PassLegacy("Mod Shinies", delegate (GenerationProgress progress, GameConfiguration c)
                {
                    progress.Start(1f);
                    for (int k = 0; k < (int)((4200 * 1200) * 6E-05); k++)
                    {
                        //  WorldGen.TileRunner(WorldGen.genRand.Next((int)(genPosition[0].X / 16) - miner.edge / 2, (int)(genPosition[1].X / 16) + miner.edge / 2), WorldGen.genRand.Next((int)genPosition[0].Y / 16 - miner.edge / 2, (int)genPosition[1].Y / 16 + miner.edge / 2), WorldGen.genRand.Next(15, 18), WorldGen.genRand.Next(2, 6), magnoDirt, false, 0f, 0f, false, true);
                        //MINER Legacy
                        //int randX = WorldGen.genRand.Next((int)(genPosition[0].X / 16) - miner.edge / 2, (int)(genPosition[1].X / 16) + miner.edge / 2);
                        //int randY = WorldGen.genRand.Next((int)genPosition[0].Y / 16 - miner.edge / 2, (int)genPosition[1].Y / 16 + miner.edge / 2);
                        //NEW Magno gen
                        int randX = WorldGen.genRand.Next(originX, originX + mWidth);
                        int randY = WorldGen.genRand.Next(originY, originY + mHeight);
                        if (Main.tile[Math.Max(randX, 10), Math.Max(randY, 10)].TileType == magnoStone)
                        {
                            WorldGen.TileRunner(randX, randY, WorldGen.genRand.Next(9, 12), WorldGen.genRand.Next(2, 6), magnoOre, false, 0f, 0f, false, true);
                        }
                        progress.Value = k / (float)((4200 * 1200) * 6E-05);
                    }
                    progress.End();
                }, 1f));
            }
            int index1 = tasks.FindIndex(pass => pass.Name.Equals("Wet Jungle"));
            if (index1 != -1)
            {
                tasks.Insert(index1, new PassLegacy("Halls", delegate (GenerationProgress progress, GameConfiguration c)
                {
                    progress.Start(1f);
                    progress.Message = "Halls";
                    SkyHall hall = new SkyHall();
                    hall.SkyFortGen(skyBrick, skyBrickWall);
                }, 1f));
            }
            int index2 = tasks.FindIndex(pass => pass.Name.Equals("Wet Jungle"));
            if (index2 != -1)
            {
                tasks.Insert(index2, new PassLegacy("Sky Generation", delegate (GenerationProgress progress, GameConfiguration c)
                {
                    progress.Start(1f);
                    progress.Message = "Fort";
                    //SkyHall hall = new SkyHall();
                    //hall.SkyFortGen();
                    Vector2 position;
                    do
                    {
                        position = new Vector2(WorldGen.genRand.Next(200, Main.maxTilesX - 600), Structures.YCoord);
                    } while (position.X < Main.spawnTileX + 300 && position.X > Main.spawnTileX - 450);
                    var s = new Structures(position, skyBrick, skyBrickWall);
                    s.InitializeFort();
                    progress.Value = 1f;
                    progress.End();
                }, 1f));
            }
            int index3 = tasks.FindIndex(pass => pass.Name.Equals("Remove Water From Sand"));
            if (index3 != -1)
            {
                tasks.Insert(index3, new PassLegacy("Mod Generation", delegate (GenerationProgress progress, GameConfiguration c)
                {
                    progress.Start(1f);
                    progress.Message = "Magno extras";
                    t = new Treasures();
                    int place = 0;
                    int width = Main.maxTilesX - 100;
                    int height = Main.maxTilesY - 100;
                    Vector2[] any = Treasures.FindAll(new Vector2(100, 100), width, height, false, new ushort[] { magnoStone, Ash });
                    foreach (Vector2 floor in any)
                        if (floor != Vector2.Zero)
                        {
                            int i = (int)floor.X;
                            int j = (int)floor.Y;
                            int style = 0;
                            Tile top = Main.tile[i, j - 1];
                            Tile bottom = Main.tile[i, j + 1];
                            Tile bottomLeft = Main.tile[i + 1, j + 1];
                            Tile left = Main.tile[i - 1, j];
                            Tile right = Main.tile[i + 1, j];
                            Tile rock = default;
                            if (top.TileType == magnoStone && top.HasTile)
                            {
                                style = 0;
                                if (WorldGen.genRand.Next(10) == 0)
                                {
                                    if (WorldGen.genRand.NextBool())
                                        t.PlaceTile(i, j, ambientRocksTopLarge, true, false, 4, false, style = WorldGen.genRand.Next(3));
                                    else t.PlaceTile(i, j, ambientRocksTopSmall, true, false, 4, false, style = WorldGen.genRand.Next(3));
                                    //rock = Main.tile[i, j];
                                    //if (rock.type == ambientRocks)
                                    //    Main.tile[i, j].frameX = (short)(18 * WorldGen.genRand.Next(3));
                                }
                            }
                            if (left.TileType == magnoStone && left.HasTile)
                                style = 1;
                            if (right.TileType == magnoStone && right.HasTile)
                                style = 2;
                            if (bottom.TileType == magnoStone && bottom.HasTile)
                            {
                                style = 3;
                                if (WorldGen.genRand.NextBool())
                                    t.PlaceTile(i, j, ambientRocksLarge, true, false, 4, false, style = WorldGen.genRand.Next(3));
                                else t.PlaceTile(i, j, ambientRocksSmall, true, false, 4, false, style = WorldGen.genRand.Next(3));
                                //if (WorldGen.genRand.Next(10) == 0)
                                //{
                                //    t.PlaceTile(i, j, ambientRocks, true, false, 3, false, style = WorldGen.genRand.Next(3));
                                //    rock = Main.tile[i, j];
                                //    if (rock.type == ambientRocks)
                                //        Main.tile[i, j].frameX = (short)(18 * WorldGen.genRand.Next(3));
                                //}
                            }
                            if (bottom.TileType == Ash && bottom.HasTile)
                            {
                                if (WorldGen.genRand.Next(4) == 0)
                                {
                                    //if (!bottomLeft.active() || bottomLeft.slope() != 0)
                                    //{
                                    //    WorldGen.KillTile(i + 1, j + 1, false, false, true);
                                    //    WorldGen.PlaceTile(i + 1, j + 1, Ash, true, true);
                                    //}
                                    bool rand = WorldGen.genRand.NextBool();
                                    if (rand)
                                        t.PlaceTile(i, j, magnoPlantsSmall, true, false, 6, false, WorldGen.genRand.Next(4));
                                    else
                                    {
                                        Tile t1 = Main.tile[i + 1, j];
                                        t1.HasTile = false;
                                        Tile t2 = Main.tile[i + 1, j + 1];
                                        t2.HasTile = true;
                                        t.PlaceTile(i, j, magnoPlantsLarge, true, false, 3, false, WorldGen.genRand.Next(3));
                                    }
                                }
                            }
                            else t.PlaceTile(i, j, crystal, true, false, 10, false, style);
                            if (bottom.TileType == magnoStone && bottom.HasTile)
                            {
                                if (!Main.tile[i + 1, j].HasTile)
                                {
                                    place++;
                                    if (place % 3 == 0)
                                        t.PlaceTile(i, j, crystal2x2, true, false, 8);
                                    else t.PlaceTile(i, j, crystal2x1, true, false, 8);
                                }
                            }
                        }
                    progress.Value = 1f;
                    progress.End();
                }, 1f));
            }
            int index4 = tasks.FindIndex(pass => pass.Name.Equals("Jungle Temple"));
            if (index4 != -1)
            {
                tasks.Insert(index4, new PassLegacy("Sorting Floating Tiles", delegate (GenerationProgress progress, GameConfiguration c)
                {
                    progress.Message = "Magno Sorting";
                    for (int j = 100; j < Main.maxTilesY - 250; j++)
                        for (int i = 100; i < Main.maxTilesX - 100; i++)
                        {
                            Tile t = Main.tile[i, j];
                            Tile[] tiles = new Tile[]
                            {
                                Main.tile[i, j + 1],
                                Main.tile[i - 1, j],
                                Main.tile[i + 1, j]
                            };
                            int count = 0;
                            if (t.TileType == crystal)
                            {
                                foreach (Tile tile in tiles)
                                {
                                    if (!tile.HasTile)
                                        count++;
                                    if (count == 3)
                                    {
                                        t.HasTile = false;
                                        break;
                                    }
                                }
                            }
                        }
                }, 1f));
            }
            int index5 = tasks.FindIndex(pass => pass.Name.Equals("Hives"));
            tasks.Insert(index5, new PassLegacy("Structure Generation", delegate (GenerationProgress progress, GameConfiguration c)
            {
                progress.Start(1f);
                progress.Message = "More Magno";
                var m = new Structures.Magno();
                m.tileID = magnoBrick;
                m.wallID = magnoBrickWall;
                Vector2 origin = new Vector2(100, 100);
                Vector2[] regions = Treasures.GetRegion(origin, Main.maxTilesX - 100, Main.maxTilesY - 250, false, true, new ushort[] { magnoStone });
                int count = 0;
                int total = (int)Math.Sqrt(regions.Length);
                int max = WorldGen.genRand.Next(5, 8);
                for (int i = 0; i < max; i++)
                {
                    m.Initialize();
                    while (!m.MagnoHouse(regions[WorldGen.genRand.Next(regions.Length)]))
                    {
                        if (count < total)
                            count++;
                        else break;
                    }
                    count = 0;
                    progress.Value = (float)i / max;
                }
                progress.Value = 1f;
                progress.End();
            }, 1f));

            float PositionX;
            const int TileSize = 16;
            int buffer = 16, wellBuffer = 96, surfaceBuffer = 0;
            int WellIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Smooth World"));
            if (WellIndex != -1)
            {
                tasks.Insert(WellIndex + 1, new PassLegacy("Digging Well", delegate (GenerationProgress progress, GameConfiguration c)
                {
                    surfaceBuffer = (int)(Main.maxTilesY * 0.167f);
                    progress.Message = "Digging Well";
                    Vector2 Center = new Vector2((Main.maxTilesX / 2) * 16, (Main.maxTilesY / 2) * 16);
                    //MINER Legacy gen
                    //if ((miner.genPos[0].X + wellBuffer / 3) / TileSize > miner.baseCenter.X / TileSize)
                    //{
                    //    PositionX = miner.genPos[1].X / 16;
                    //}
                    //else PositionX = miner.genPos[0].X / 16;
                    int tries = 0;
                    do { 
                        PositionX = WorldGen.genRand.Next(originX, originX + mWidth);
                    } while (PositionX < Main.rightWorld / 16f - mWidth && tries++ < 200);

                    int gap = 5;
                    int MaxTries = (int)(Main.maxTilesY * 0.167f);
                    bool dirtWall = Main.tile[(int)PositionX, surfaceBuffer].WallType == WallID.DirtUnsafe || Main.tile[(int)PositionX, surfaceBuffer].WallType == WallID.DirtUnsafe1 || Main.tile[(int)PositionX, surfaceBuffer].WallType == WallID.DirtUnsafe2 || Main.tile[(int)PositionX, surfaceBuffer].WallType == WallID.DirtUnsafe3 || Main.tile[(int)PositionX, surfaceBuffer].WallType == WallID.DirtUnsafe4;
                    for (int i = 0; i < MaxTries; i++)
                    {
                        if (Main.tile[(int)PositionX + gap, surfaceBuffer].HasTile) 
                        { 
                            surfaceBuffer--;
                        }
                        if (!Main.tile[(int)PositionX + gap, surfaceBuffer].HasTile && Main.tile[(int)PositionX + gap, surfaceBuffer].WallType == 0)
                        {
                            surfaceBuffer++;
                        }
                    }

                    buffer = 3;
                    float distance = Vector2.Distance(new Vector2(PositionX, 10 + surfaceBuffer) - new Vector2(PositionX, originY - (int)Main.worldSurface - 25/*miner.genPos[1].Y / 16*/), Vector2.Zero);
                    // comment out '/ 3' for max well length
                    PlaceWell((int)PositionX, Math.Abs(surfaceBuffer) - buffer, distance);
                }));
            }
            #region Vector2 array
            /* int x = MagnoDen.bounds.X;
            int y = MagnoDen.bounds.Y;
            int right = MagnoDen.bounds.Right;
            int bottom = MagnoDen.bounds.Bottom;
            Vector2[] regions = new Vector2[MagnoDen.bounds.Width * MagnoDen.bounds.Height];
            for (int i = x; i < right; i++)
                for (int j = y; j < bottom; j++)
                {
                    if (Main.tile[i, j].type == TileID.PearlstoneBrick)
                        regions[count] = new Vector2(i, j);
                    count++;
                }*/
            #endregion
        }
        public override void PostWorldGen()
        {
            int[] t0 = new int[]
            {
                //ModContent.ItemType<Broadsword>(),
                //ModContent.ItemType<Calling>(),
                //ModContent.ItemType<Deflector>(),
                //ModContent.ItemType<Sabre>(),
                //ModContent.ItemType<Staff>()
                ModContent.ItemType<r_Catcher>(),
                ModContent.ItemType<r_Flail>(),
                ModContent.ItemType<r_Javelin>(),
                ModContent.ItemType<r_Tomohawk>()
            };
            int[] t1 = new int[]
            {
                ModContent.ItemType<cinnabar_dagger>(),
                ModContent.ItemType<magno_yoyo>(),
                ModContent.ItemType<m_fossil>()
            };
            int[] t2 = new int[]
            {
                ModContent.ItemType<ArchaeaMod.Merged.Items.Materials.magno_bar>(),
                ItemID.SilverBar,
                ItemID.GoldBar
            };
            int[] t3 = new int[]
            {
                ItemID.ArcheryPotion, 
                ItemID.BattlePotion, 
                ItemID.CalmingPotion, 
                ItemID.GravitationPotion, 
                ItemID.HunterPotion, 
                ItemID.LesserHealingPotion, 
                ItemID.IronskinPotion, 
                ItemID.MiningPotion, 
                ItemID.RecallPotion, 
                ItemID.TeleportationPotion
            };
            int[] s0 = t0;
            int[] s1 = t1;
            int[] s2 = new int[]
            {
                ModContent.ItemType<ArchaeaMod.Items.Materials.r_plate>()
            };
            int[] s3 = t3;
            
            for (int i = 0; i < Main.chest.Length; i++)
            {
                Chest chest = Main.chest[i];
                if (chest == null)
                    continue;
                if (Main.tile[chest.x, chest.y].TileType == magnoChest)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int type = 0;
                        int fossils = 0; 
                        switch (j)
                        {
                            case 0:
                                type = t0[Main.rand.Next(t0.Length)]; 
                                chest.item[j].SetDefaults(type);
                                break;
                            case 1:
                                type = t1[Main.rand.Next(t1.Length)]; 
                                if (type == t1[0])
                                {
                                    chest.item[j].SetDefaults(t1[0]);
                                    chest.item[j].stack = Main.rand.Next(8, 15);
                                    break;
                                }
                                if (fossils < 2)
                                {
                                    if (type == t1[2])
                                    {
                                        chest.item[j].SetDefaults(t1[2]);
                                        fossils++;
                                    }
                                }
                                break;
                            case 2:
                                type = t2[Main.rand.Next(t2.Length)]; 
                                chest.item[j].SetDefaults(type);
                                chest.item[j].stack = Main.rand.Next(6, 13);
                                break;
                            case 3:
                                type = t3[Main.rand.Next(t3.Length)]; 
                                chest.item[j].SetDefaults(type);
                                chest.item[j].stack = Main.rand.Next(1, 4);
                                break;
                        }
                    }
                }
                if (chest.y < Main.spawnTileY && Main.tile[chest.x, chest.y].TileFrameX == 0)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int type = 0;
                        switch (j)
                        {
                            case 0:
                                type = s0[Main.rand.Next(s0.Length)]; 
                                chest.item[j].SetDefaults(type);
                                if (type == ModContent.ItemType<r_Javelin>())
                                    chest.item[j].stack = Main.rand.Next(12, 24);
                                else if (type == ModContent.ItemType<r_Tomohawk>())
                                    chest.item[j].stack = Main.rand.Next(15, 24);
                                break;
                            case 1:
                                type = s1[Main.rand.Next(s1.Length)]; 
                                if (type == s1[0])
                                {
                                    chest.item[j].SetDefaults(t1[0]);
                                    chest.item[j].stack = Main.rand.Next(8, 15);
                                }
                                break;
                            case 2:
                                type = s2[Main.rand.Next(s2.Length)]; 
                                chest.item[j].SetDefaults(type);
                                chest.item[j].stack = Main.rand.Next(6, 13);
                                break;
                            case 3:
                                type = s3[Main.rand.Next(s3.Length)]; 
                                chest.item[j].SetDefaults(type);
                                chest.item[j].stack = Main.rand.Next(1, 4);
                                break;
                        }
                    }
                }
            }
        }

        public override void OnWorldUnload()
        {
            if (Effects.Barrier.barrier != null)
            { 
                for (int i = 0; i < Effects.Barrier.barrier.Length; i++)
                {
                    Effects.Barrier.barrier[i] = null;
                }
            }
            Effects.Barrier.barrier = null;
        }
        public override void PostDrawInterface(SpriteBatch sb)
        {
            Effects.Barrier.DrawHint(sb, Main.LocalPlayer);
        }
        int[,] wellShape = new int[,]
        {
            { 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0 },
            { 0, 0, 5, 2, 4, 0, 5, 2, 4, 0, 0 },
            { 0, 0, 0, 6, 0, 0, 0, 6, 0, 0, 0 },
            { 0, 0, 5, 2, 2, 2, 2, 2, 4, 0, 0 },
            { 0, 0, 0, 6, 0, 3, 0, 6, 0, 0, 0 },
            { 0, 0, 0, 6, 0, 3, 0, 6, 0, 0, 0 },
            { 0, 0, 0, 6, 0, 3, 0, 6, 0, 0, 0 },
            { 0, 0, 0, 6, 0, 3, 0, 6, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 3, 0, 1, 0, 0, 0 },
            { 0, 0, 1, 1, 0, 3, 0, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 0, 3, 0, 1, 1, 1, 0 },
            { 7, 7, 7, 1, 0, 3, 0, 1, 7, 7, 7 }
        };
        int[,] wellShapeWall = new int[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 }
        };
        public bool PlaceWell(int i, int j, float length)
        {
            for (int m = 0; m < (int)length; m++)
            {
                for (int n = -2; n < 3; n++)
                {
                    WorldGen.KillTile(i + n, j + m, false, false, true);
                    WorldGen.KillTile(i + n, j + m, false, false, true);
                    if (n == -2 || n == 2)
                    {
                        WorldGen.PlaceTile(i + n, j + m, magnoBrick, true, true);
                    }
                    if (n > -2 && n < 2)
                    {
                        WorldGen.PlaceWall(i + n, j + m, magnoBrickWall, true);
                        Main.tile[i + n, j + m].WallType = magnoBrickWall;
                    }
                }
                WorldGen.PlaceTile(i, j + m, TileID.Rope);
            }
            for (int y = 0; y < wellShape.GetLength(0); y++)
            {
                for (int x = 0; x < wellShape.GetLength(1); x++)
                {
                    int k = i - 5 + x;
                    int l = j - 8 + y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (wellShape[y, x])
                        {
                            case 1:
                                tile.TileType = magnoBrick;
                                tile.HasTile = true;
                                tile.Slope = 0;
                                break;
                            case 2:
                                if (WorldGen.crimson)
                                    tile.TileType = TileID.RedDynastyShingles;
                                else tile.TileType = TileID.BlueDynastyShingles;
                                tile.HasTile = true;
                                tile.Slope = 0;
                                break;
                            case 3:
                                tile.TileType = TileID.Rope;
                                tile.HasTile = true;
                                break;
                            case 4:
                                if (WorldGen.crimson)
                                    tile.TileType = TileID.RedDynastyShingles;
                                else tile.TileType = TileID.BlueDynastyShingles;
                                tile.HasTile = true;
                                tile.Slope = (SlopeType)3;
                                break;
                            case 5:
                                if (WorldGen.crimson)
                                    tile.TileType = TileID.RedDynastyShingles;
                                else tile.TileType = TileID.BlueDynastyShingles;
                                tile.HasTile = true;
                                tile.Slope = (SlopeType)4;
                                break;
                            case 6:
                                tile.TileType = TileID.WoodenBeam;
                                tile.HasTile = true;
                                break;
                            case 7:
                                tile.TileType = magnoBrick;
                                tile.HasTile = true;
                                tile.Slope = 0;
                                for (int o = 0; o < 6; o++)
                                {
                                    Tile tileY = Main.tile[k, l + o];
                                    tileY.TileType = magnoBrick;
                                    tileY.HasTile = true;
                                    tileY.Slope = 0;
                                }
                                break;
                        }
                        switch (wellShapeWall[y, x])
                        {
                            case 1:
                                tile.WallType = WallID.Planked;
                                break;
                        }
                    }
                }
            }
            return true;
        }
        public bool MagnoBiome;
        public bool SkyFort;
        public bool nearMusicBox;
        public bool SkyPortal;
        private void UpdateTileCounts()
        {
            MagnoBiome = Main.SceneMetrics.GetTileCount(magnoStone) >= 80 || Main.SceneMetrics.GetTileCount(Ash) >= 30;
            SkyFort = Main.SceneMetrics.GetTileCount(skyBrick) >= 80;
            SkyPortal = Main.SceneMetrics.GetTileCount((ushort)ModContent.TileType<Tiles.sky_portal>()) != 0;
            nearMusicBox = Main.SceneMetrics.GetTileCount((ushort)ModContent.TileType<Tiles.music_boxes>()) != 0;
        }
        public bool cordonBounds = false;
        private bool spawnedCrystals;
        public static int worldID;
        public static List<int> classes = new List<int>();
        public static List<int> playerIDs = new List<int>();
        public override void SaveWorldData(TagCompound tag)/* Edit tag parameter rather than returning new TagCompound */
        {
            tag.Add("m_downed", downedMagno);
            tag.Add("n_downed", downedNecrosis);
            tag.Add("First", first);
            tag.Add("Classes", classes);
            tag.Add("IDs", playerIDs);
            tag.Add("Crystals", spawnedCrystals);
            tag.Add("OriginX", MagnoBiomeOriginX);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            downedMagno = tag.GetBool("m_downed");
            downedNecrosis = tag.GetBool("n_downed");
            first = tag.GetBool("First");
            classes = tag.Get<List<int>>("Classes");
            playerIDs = tag.Get<List<int>>("IDs");
            spawnedCrystals = tag.GetBool("Crystals");
            MagnoBiomeOriginX = tag.GetInt("OriginX");
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(downedMagno);
            writer.Write(downedNecrosis);
            //  Extra
            writer.Write(spawnedCrystals);
            writer.Write(MagnoBiomeOriginX);
        }
        public override void NetReceive(BinaryReader reader)
        {
            downedMagno = reader.ReadBoolean();
            downedNecrosis = reader.ReadBoolean();
            //  Extra
            spawnedCrystals = reader.ReadBoolean();
            MagnoBiomeOriginX = reader.ReadInt32();
        }
        private bool begin;
        private bool first;
        public static Player firstPlayer;
        public override void PostUpdateEverything()
        {
            UpdateTileCounts();
            if (!first)
            {
                if (firstPlayer == null)
                {
                    foreach (Player p in Main.player.Where(t => t != null))
                    {
                        if (p.active)
                            firstPlayer = p;
                    }
                }
                else if (firstPlayer.active)
                {
                    first = true;
                }
            }
            if (ArchaeaPlayer.KeyPress(Keys.E))
                begin = false;
            if (ArchaeaPlayer.KeyPress(Keys.Q))
            {
                if (!begin)
                {
                    //s = new Structures.Magno();
                    //t = new Treasures();
                    /*
                    Vector2 position;
                    do
                    {
                        position = new Vector2(WorldGen.genRand.Next(200, Main.maxTilesX - 200), 50);
                    } while (position.X < Main.spawnTileX + 150 && position.X > Main.spawnTileX - 150);
                    var s = new Structures(position, skyBrick, skyBrickWall);
                    s.InitializeFort();
                    begin = true;*/
                }
                //s.Initialize();
                //s.tileID = magnoBrick;
                //s.wallID = magnoBrickWall;
                //Player player = Main.LocalPlayer;
                //s.MagnoHouse(new Vector2(player.position.X / 16, player.position.Y / 16));
            }
            if (Main.hardMode && !spawnedCrystals)
            {
                t = new Treasures();
                int place = 0;
                int width = Main.maxTilesX - 100;
                int height = Main.maxTilesY - 100;
                Vector2[] any = Treasures.FindAll(new Vector2(100, 100), width, height, false, new ushort[] { magnoStone });
                foreach (Vector2 floor in any)
                {
                    if (floor != Vector2.Zero)
                    {
                        int i = (int)floor.X;
                        int j = (int)floor.Y;
                        t.PlaceTile(i, j, crystalLarge, true, false, 8);
                    }
                }
                spawnedCrystals = true;
            }
        }
        public static bool Inbounds(int i, int j)
        {
            return i < Main.maxTilesX - 50 && i > 50 && j < Main.maxTilesY - 200 && j > 50;
        }
        public static void Clamp(ref int input, int min, int max, out int result)
        {
            if (input < min)
                input = min;
            if (input > max)
                input = max;
            result = input;
        }
    }
    
    public class Treasures
    {
        public int offset;
        private ushort floorID;
        private ushort newTileID;
        private ushort wallID;
        private List<Vector2> list;
        public void Initialize(int offset, ushort newTileID, ushort floorID, ushort wallID)
        {
            this.offset = offset;
            this.newTileID = newTileID;
            this.floorID = floorID;
            this.wallID = wallID;
            list = ArchaeaWorld.origins;
        }
        public void PlaceChests(int total, int retries)
        {
            int index = 1;
            int count = 0;
            int loop = 0;
            var getFloor = GetFloor();
            int length = list.Count;
            bool[] added = new bool[length];
            while (count < total)
            {
                if (loop < total * retries)
                    loop++;
                else
                {
                    index++;
                    loop = 0;
                }
                foreach (Vector2 ground in getFloor[index - 1])
                {
                    int x = (int)ground.X;
                    int y = (int)ground.Y;
                    if (!ArchaeaWorld.Inbounds(x, y)) continue;
                    if (Main.tile[x, y].WallType == wallID && WorldGen.genRand.Next(8) == 0)
                        WorldGen.PlaceTile(x, y, newTileID, true, true);
                    if (Main.tile[x, y].TileType == newTileID)
                    {
                        added[index] = true;
                        count++;
                        break;
                    }
                }
                if (added[index])
                {
                    index++;
                    loop = 0;
                }
                if (index == length)
                    break;
            }
        }
        public void PlaceTile(Vector2[] region, int total, int retries, ushort newTileID, bool genPlace = true, bool force = false, bool random = false, int odds = 5, bool proximity = false, int radius = 30, bool iterate = false, bool onlyOnWall = false)
        {
            int loop = 0;
            int index = 0;
            var getFloor = region;
            while (index < getFloor.Length)
            {
                if (loop < total * retries)
                    loop++;
                else break;
                if (getFloor[index] == Vector2.Zero)
                {
                    index++;
                    continue;
                }
                int x = (int)getFloor[index].X;
                int y = (int)getFloor[index].Y;
                Tile tile = Main.tile[x, y];
                if (random && WorldGen.genRand.Next(odds) != 0) continue;
                if (onlyOnWall && Main.tile[x, y].WallType != wallID)
                {
                    index++;
                    continue;
                }
                if (proximity && Vicinity(getFloor[index], radius, newTileID))
                {
                    index++;
                    continue;
                }
                if (genPlace)
                    WorldGen.PlaceTile(x, y, newTileID, true, force);
                else
                {
                    tile.HasTile = true;
                    tile.TileType = newTileID;
                }
                if (total == 1 && tile.TileType == newTileID && tile.HasTile)
                    break;
                if (iterate && index == getFloor.Length - 1)
                    index = 0;
                index++;
            }
        }
        public bool PlaceTile(int i, int j, ushort tileType, bool genPlace = false, bool force = false, int proximity = -1, bool wall = false, int style = 0)
        {
            Tile tile = Main.tile[i, j];
            if (proximity != -1 && Vicinity(new Vector2(i, j), proximity, tileType))
                return false;
            if (!genPlace)
            {
                tile.HasTile = true;
                tile.TileType = tileType;
            }
            else
            {
                WorldGen.PlaceTile(i, j, tileType, true, force, -1, style);
            }
            if (tile.TileType == tileType)
                return true;
            return false;
        }
        public Vector2[][] GetFloor()
        {
            int index = 0;
            int count = 0;
            int length = list.Count;
            var tiles = new Vector2[length][];
            for (int k = 0; k < length; k++)
                tiles[k] = new Vector2[length * length];
            foreach (Vector2 v2 in list)
            {
                for (int i = (int)v2.X - offset; i < (int)v2.X + offset; i++)
                    for (int j = (int)v2.Y - offset; j < (int)v2.Y + offset; j++)
                    {
                        Tile floor = Main.tile[i, j];
                        Tile ground = Main.tile[i, j + 1];
                        if ((!floor.HasTile || !Main.tileSolid[floor.TileType]) &&
                            ground.HasTile && Main.tileSolid[ground.TileType] && ground.TileType == floorID)
                        {
                            if (count < tiles[index].Length)
                            {
                                tiles[index][count] = new Vector2(i, j);
                                count++;
                            }
                        }
                    }
                count = 0;
                if (index < length)
                    index++;
                else
                    break;
            }
            return tiles;
        }
        public static Vector2[] FindAll(Vector2 region, int width, int height, bool overflow = false, ushort[] floorIDs = null)
        {
             int index = width * height * floorIDs.Length;
            int amount = (int)Math.Sqrt(index) / 10;
            int count = 0;
            var tiles = new Vector2[index];
            foreach (ushort floorType in floorIDs)
                for (int i = (int)region.X; i < (int)region.X + width; i++)
                    for (int j = (int)region.Y; j < (int)region.Y + height; j++)
                    {
                        if (!ArchaeaWorld.Inbounds(i, j)) continue;
                        if (overflow & WorldGen.genRand.Next(5) == 0) continue;
                        Tile origin = Main.tile[i, j];
                        Tile ceiling = Main.tile[i, j - 1];
                        Tile ground = Main.tile[i, j + 1];
                        Tile right = Main.tile[i + 1, j];
                        Tile ieft = Main.tile[i - 1, j];
                        if (origin.HasTile && Main.tileSolid[origin.TileType]) continue;
                        if (ceiling.HasTile && Main.tileSolid[ceiling.TileType] && ceiling.TileType == floorType || 
                            ground.HasTile && Main.tileSolid[ground.TileType] && ground.TileType == floorType || 
                            right.HasTile && Main.tileSolid[right.TileType] && right.TileType == floorType || 
                            ieft.HasTile && Main.tileSolid[ieft.TileType] && ieft.TileType == floorType)
                        {
                            if (count < tiles.Length)
                            {
                                tiles[count] = new Vector2(i, j);
                                count++;
                            }
                        }
                    }
            return tiles;
        }
        public static Vector2[] GetFloor(Vector2 region, int width, int height, bool overflow = false, ushort[] floorIDs = null)
        {
            int index = width * height * floorIDs.Length;
            int amount = (int)Math.Sqrt(index) / 10;
            int count = 0;
            var tiles = new Vector2[index];
            foreach (ushort floorType in floorIDs)
                for (int i = (int)region.X; i < (int)region.X + width; i++)
                    for (int j = (int)region.Y; j < (int)region.Y + height; j++)
                    {
                        if (!ArchaeaWorld.Inbounds(i, j)) continue;
                        if (overflow & WorldGen.genRand.Next(5) == 0) continue;
                        Tile floor = Main.tile[i, j];
                        Tile ground = Main.tile[i, j + 1];
                        if (floor.HasTile && Main.tileSolid[floor.TileType]) continue;
                        if (ground.HasTile && Main.tileSolid[ground.TileType] && ground.TileType == floorType)
                        {
                            if (count < tiles.Length)
                            {
                                tiles[count] = new Vector2(i, j);
                                count++;
                            }
                        }
                    }
            return tiles;
        }
        public static Vector2[] GetCeiling(Vector2 region, int radius, bool overflow = false, ushort tileType = 0)
        {
            int index = (int)Math.Pow(radius * 2, 2);
            int count = 0;
            var tiles = new Vector2[index];
            for (int i = (int)region.X - radius; i < (int)region.X + radius; i++)
                for (int j = (int)region.Y - radius; j < (int)region.Y + radius; j++)
                {
                    if (!ArchaeaWorld.Inbounds(i, j)) continue;
                    if (overflow & WorldGen.genRand.Next(5) == 0) continue;
                    Tile roof = Main.tile[i, j];
                    Tile ceiling = Main.tile[i, j + 1];
                    if (ceiling.HasTile && Main.tileSolid[ceiling.TileType]) continue;
                    if (roof.HasTile && Main.tileSolid[roof.TileType] && roof.TileType == tileType)
                    {
                        if (count < tiles.Length)
                        {
                            tiles[count] = new Vector2(i, j);
                            count++;
                        }
                    }
                }
            return tiles;
        }
        public static Vector2[] GetCeiling(Vector2 region, int width, int height, bool overflow = false, ushort tileType = 0)
        {
            var tiles = new List<Vector2>();
            for (int i = (int)region.X; i < width; i++)
                for (int j = (int)region.Y; j < height; j++)
                {
                    if (!ArchaeaWorld.Inbounds(i, j)) continue;
                    if (overflow & WorldGen.genRand.Next(5) == 0) continue;
                    Tile roof = Main.tile[i, j];
                    Tile ceiling = Main.tile[i, j + 1];
                    if (ceiling.HasTile && Main.tileSolid[ceiling.TileType]) continue;
                    if (roof.HasTile && Main.tileSolid[roof.TileType] && roof.TileType == tileType)
                        tiles.Add(new Vector2(i, j + 1));
                }
            return tiles.ToArray();
        }
        public static Vector2[] GetRegion(Vector2 region, int width, int height, bool overflow = false, bool attach = false, ushort[] tileTypes = null)
        {
            int index = width * height * tileTypes.Length;
            int count = 0;
            var tiles = new Vector2[index];
            foreach (ushort tileType in tileTypes)
                for (int i = (int)region.X; i < (int)region.X + width; i++)
                    for (int j = (int)region.Y; j < (int)region.Y + height; j++)
                    {
                        if (count >= tiles.Length) continue;
                        if (!ArchaeaWorld.Inbounds(i, j)) continue;
                        if (attach && Main.tile[i, j].TileType != tileType) continue;
                        if (overflow & WorldGen.genRand.Next(5) == 0) continue;
                        tiles[count] = new Vector2(i, j);
                        count++;
                    }
            return tiles;
        }
        public static Vector2[] GetWall(Vector2 region, int width, int height, bool overflow = false, bool attach = false, ushort[] attachTypes = null)
        {
            int index = width * height * attachTypes.Length;
            int count = 0;
            var tiles = new Vector2[index];
            foreach (ushort tileType in attachTypes)
                for (int i = (int)region.X; i < (int)region.X + width; i++)
                    for (int j = (int)region.Y; j < (int)region.Y + height; j++)
                    {
                        if (count >= tiles.Length) continue;
                        if (!ArchaeaWorld.Inbounds(i, j)) continue;
                        if (overflow & WorldGen.genRand.Next(5) == 0) continue;
                        Tile tile = Main.tile[i, j];
                        Tile wallL = Main.tile[i - 1, j];
                        Tile wallR = Main.tile[i + 1, j];
                        if (wallL.HasTile && Main.tileSolid[wallL.TileType])
                            if (!tile.HasTile || !Main.tileSolid[tile.TileType])
                            {
                                if (attach && wallL.TileType != tileType) continue;
                                tiles[count] = new Vector2(i, j);
                            }
                        if (wallR.HasTile && Main.tileSolid[wallR.TileType])
                            if (!tile.HasTile || !Main.tileSolid[tile.TileType])
                            {
                                if (attach && wallR.TileType != tileType) continue;
                                tiles[count] = new Vector2(i, j);
                            }
                        count++;
                    }
            return tiles;
        }
        public static Vector2[] GetWall(int x, int y, int width, int height, ushort[] tileTypes = null, int radius = -1)
        {
            int count = 0;
            List<Vector2> list = new List<Vector2>();
            foreach (ushort tileType in tileTypes)
                for (int i = x; i < width; i++)
                    for (int j = y; j < width; j++)
                    {
                        if (!ArchaeaWorld.Inbounds(i, j))
                            continue;
                        if (radius != -1 && Vicinity(new Vector2(i, j), radius, tileType))
                            continue;
                        Tile up = Main.tile[i, j - 1];
                        Tile left = Main.tile[i - 1, j];
                        Tile right = Main.tile[i + 1, j];
                        if ((left.TileType == tileType || right.TileType == tileType) && !up.HasTile)
                        {
                            list.Add(new Vector2(i, j));
                            count++;
                        }
                    }
            return list.ToArray();
        }
        public static bool Vicinity(Vector2 region, int radius, ushort tileType)
        {
            int x = (int)region.X;
            int y = (int)region.Y;
            for (int i = x - radius; i < x + radius; i++)
                for (int j = y - radius; j < y + radius; j++)
                {
                    if (!ArchaeaWorld.Inbounds(i, j)) continue;
                    if (Main.tile[i, j].TileType == tileType)
                        return true;
                }
            return false;
        }
        public static int Vicinity(Vector2 region, int radius, ushort[] tileType)
        {
            Func<int> count = delegate ()
            {
                int x = (int)region.X;
                int y = (int)region.Y;
                int tiles = 0;
                for (int i = x - radius; i < x + radius; i++)
                    for (int j = y - radius; j < y + radius; j++)
                    {
                        if (!ArchaeaWorld.Inbounds(i, j)) continue;
                        foreach (ushort type in tileType)
                            if (Main.tile[i, j].TileType == type && Main.tile[i, j].HasTile)
                            {
                                tiles++;
                                break;
                            }
                    }
                return tiles;
            };
            return count();
        }
        public static bool Vicinity(Vector2 region, int radius, ushort[] tileType, int limit)
        {
            Func<bool> count = delegate ()
            {
                int x = (int)region.X;
                int y = (int)region.Y;
                int tiles;
                foreach (ushort type in tileType)
                {
                    tiles = 0;
                    for (int i = x - radius; i < x + radius; i++)
                        for (int j = y - radius; j < y + radius; j++)
                        {
                            if (!ArchaeaWorld.Inbounds(i, j)) continue;
                            if (Main.tile[i, j].TileType == type && Main.tile[i, j].HasTile)
                            {
                                if (tiles++ > limit)
                                    return true;
                            }
                        }
                }
                return false;
            };
            return count();
        }
        public static int ProximityCount(Vector2 region, int radius, ushort tileType)
        {
            int x = (int)region.X;
            int y = (int)region.Y;
            int count = 0;
            for (int i = x - radius; i < x + radius; i++)
                for (int j = y - radius; j < y + radius; j++)
                {
                    if (!ArchaeaWorld.Inbounds(i, j)) continue;
                    Tile tile = Main.tile[i, j];
                    if (tile.TileType == tileType)
                        count++;
                }
            return count;
        }
        public static bool ActiveAndSolid(int i, int j)
        {
            return Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType];
        }
    }
    public class Digger
    {
        private int max;
        private ushort tileID;
        private ushort wallID;
        private readonly float radians = 0.017f;
        private Vector2[] centers;
        public Digger(int size, ushort tileID, ushort wallID)
        {
            max = size;
            this.tileID = tileID;
            this.wallID = wallID;
        }
        public void DigSequence(Vector2 center)
        {
            int num = 0;
            int border = 0;
            int size = 10;
            float radius = 0f;
            Relocate(ref center, out centers);
            size = WorldGen.genRand.Next(8, 15);
            border = size + 4;
            List<Vector2> list = new List<Vector2>();
            foreach (Vector2 path in centers)
            {
                num++;
                float weight = 0f;
                while (weight < 1f)
                {
                    list.Add(Vector2.Lerp(centers[num - 1], centers[Math.Min(num, centers.Length - 1)], weight));
                    weight += 0.2f;
                }
            }
            foreach (Vector2 lerp in list)
            {
                radius = size;
                while (radius < border)
                {
                    int offset = border / 2;
                    for (int i = (int)lerp.X - offset; i < (int)lerp.X + offset; i++)
                        for (int j = (int)lerp.Y - offset; j < (int)lerp.Y + offset; j++)
                        {
                            Main.tile[i, j].TileType = tileID;
                            Tile tile = Main.tile[i, j];
                            tile.HasTile = true;
                            //  WorldGen.PlaceTile(i, j, tileID, true, true);
                        }
                    for (int i = (int)lerp.X - offset + 2; i < (int)lerp.X + offset - 1; i++)
                        for (int j = (int)lerp.Y - offset + 2; j < (int)lerp.Y + offset - 1; j++)
                            Main.tile[i, j].WallType = wallID;
                    radius += 0.5f;
                }
            }
            radius = 0f;
            foreach (Vector2 lerp in list)
            {
                while (radius < size)
                {
                    int offset = size / 2;
                    for (int i = (int)lerp.X - offset; i < (int)lerp.X + offset; i++)
                        for (int j = (int)lerp.Y - offset; j < (int)lerp.Y + offset; j++)
                        {
                            Main.tile[i, j].TileType = 0;
                            Tile tile = Main.tile[i, j];
                            tile.HasTile = false;
                            //  WorldGen.KillTile(i, j, false, false, true);
                        }
                    radius += 0.5f;
                }
                radius = 0f;
            }
            list.Clear();
        }
        public void Relocate(ref Vector2 position, out Vector2[] path)
        {
            int x = (int)position.X;
            int y = (int)position.Y;
            int count = 0;
            int max = this.max;
            Vector2[] paths = new Vector2[max];
            while (count < max)
            {
                x = (int)position.X;
                y = (int)position.Y;
                int[] direction = new int[]
                {
                    -2, 5, -8, 10, 3, -1,
                    3, 1, -6, -3, 9, 2
                };
                int randX = direction[WorldGen.genRand.Next(direction.Length)];
                int randY = direction[WorldGen.genRand.Next(direction.Length)];
                position.X += randX;
                position.Y += randY;
                paths[count] = position;
                count++;
            }
            path = paths;
        }
    }

    public class Miner : ModSystem
    {
        public Mod moda = ModLoader.GetMod("ArchaeaMod");
        public static string progressText = "";
        static int numMiners = 0, randomX, randomY, bottomBounds = Main.maxTilesY, rightBounds = Main.maxTilesX, circumference, ticks;
        public int edge = 128;
        float mineBlockX = 256, mineBlockY = 256;
        float RightBounds;
        static bool runner = false, grassRunner = false, fillerRunner = false, russianRoulette = false;
        public Vector2 center = new Vector2((Main.maxTilesX / 2) * 16, (Main.maxTilesY / 2) * 16);
        public int buffer = 1, offset = 200;
        int whoAmI = 0, type = 0;
        int XOffset = 512, YOffset = 384;
        public int jobCount = 0;
        public int jobCountMax = 32;
        static int moveID, lookFurther, size = 1;
        public Vector2 minerPos;
        public Vector2 finalVector;
        static Vector2 oldMinerPos, deadZone = Vector2.Zero;
        Vector2 position;
        Vector2 mineBlock;
        public Vector2 baseCenter;
        bool init = false;
        bool fail;
        bool switchMode = false;
        public bool active = true;
        public Vector2[] genPos = new Vector2[2];
        Vector2[] minePath = new Vector2[800 * 800];
        //  for loop takes care of need to generate new miners
        //  Miner[] ID = new Miner[400];
        public void Init()
        {
            if (whoAmI == 0)
            {
                //  remove these comments for public version
                float offset = XOffset * WorldGen.genRand.Next(-1, 1);
                if (offset == 0)
                {
                    offset = XOffset;
                }
                minerPos = center + new Vector2(offset * 16f, Main.maxTilesY - YOffset);
                center = minerPos;
                baseCenter = minerPos;
            }
            else
            {
                int RandomX = WorldGen.genRand.Next(-2, 2);
                int RandomY = WorldGen.genRand.Next(-2, 2);
                if (RandomX != 0 && RandomY != 0)
                {
                    mineBlock = new Vector2(mineBlockX * RandomX, mineBlockY * RandomY);
                    minerPos += mineBlock;
                }
                else
                {
                    mineBlock = new Vector2(mineBlockX, mineBlockY);
                    minerPos += mineBlock;
                    return;
                }
            }
            minePath[0] = center;
            init = true;
            //  Main.spawnTileX = (int)center.X / 16;
            //  Main.spawnTileY = (int)center.Y / 16;
            progressText = jobCount + " initiated, " + Math.Round((double)((float)jobCount / jobCountMax) * 10, 0) + "%";
        }
        public void Update()
        {
            if (!init) Init();
            if (init && whoAmI == 0)
            {
                for (int k = 0; whoAmI < 800; k++)
                {
                    Mine();
                }
            }
            else if (whoAmI > 0 && whoAmI <= 800)
            {
                for (int k = 0; whoAmI < 800; k++)
                {
                    Mine();
                }
                if (whoAmI == 800)
                {
                    jobCount++;
                    Init();
                    whoAmI = 1;
                }
            }

            if (minerPos.X < center.X)
                center.X = minerPos.X;
            if (minerPos.Y < center.Y)
                center.Y = minerPos.Y;
            if (minerPos.X > oldMinerPos.X)
                oldMinerPos.X = minerPos.X;
            if (minerPos.Y > oldMinerPos.Y)
                oldMinerPos.Y = minerPos.Y;

            if (jobCount > jobCountMax)
            {
                progressText = "Process complete";
                int layer = (int)Main.worldSurface;
                int offset = Main.maxTilesY / 2;
                if (minerPos.X < center.X)
                {
                    genPos[0] = new Vector2(minerPos.X, center.Y);
                    genPos[1] = oldMinerPos;
                }
                if (minerPos.X > center.X)
                {
                    genPos[0] = center;
                    genPos[1] = oldMinerPos;
                }
                if (!switchMode)
                {
                    switchMode = true;
                    Dig();
                }
            }
            if (switchMode)
            {
                //  jobCount--;
                Terminate();
                //  Reset();
            }
        }
        public void AverageMove() // most average path, sometimes most interesting
        {
            size = WorldGen.genRand.Next(1, 3);
            if (WorldGen.genRand.Next(4) == 1)
            {
                minerPos.X += 16;
                Dig();
            }
            if (WorldGen.genRand.Next(4) == 1)
            {
                minerPos.X -= 16;
                Dig();
            }
            if (WorldGen.genRand.Next(4) == 1)
            {
                minerPos.Y += 16;
                Dig();
            }
            if (WorldGen.genRand.Next(4) == 1)
            {
                minerPos.Y -= 16;
                Dig();
            }
            GenerateNewMiner();
        }
        public void DirectionalMove() // tends to stick to a path
        {
            size = WorldGen.genRand.Next(1, 3);
            minerPos.X = Math.Min((int)Main.maxTilesX * 16 - 16, (int)minerPos.X);
            minerPos.Y = Math.Min((int)Main.maxTilesY * 16 - 16, (int)minerPos.Y);
            if (WorldGen.genRand.Next(4) == 1 && Main.tile[Math.Min((int)(minerPos.X + 16 + (16 * lookFurther)) / 16, Main.maxTilesX), (int)minerPos.Y / 16].HasTile)
            {
                minerPos.X += 16;
                Dig();
            }
            if (WorldGen.genRand.Next(4) == 1 && Main.tile[(int)(minerPos.X - 16 - (16 * lookFurther)) / 16, (int)minerPos.Y / 16].HasTile)
            {
                minerPos.X -= 16;
                Dig();
            }
            if (WorldGen.genRand.Next(4) == 1 && Main.tile[(int)minerPos.X / 16, Math.Min((int)(minerPos.Y + 16 + (16 * lookFurther)) / 16, Main.maxTilesY)].HasTile)
            {
                minerPos.Y += 16;
                Dig();
            }
            if (WorldGen.genRand.Next(4) == 1 && Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y - 16 - (16 * lookFurther)) / 16].HasTile)
            {
                minerPos.Y -= 16;
                Dig();
            }
            if (!Main.tile[(int)(minerPos.X + 16 + (16 * lookFurther)) / 16, (int)minerPos.Y / 16].HasTile &&
                !Main.tile[(int)(minerPos.X - 16 - (16 * lookFurther)) / 16, (int)minerPos.Y / 16].HasTile &&
                !Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y + 16 + (16 * lookFurther)) / 16].HasTile &&
                !Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y - 16 - (16 * lookFurther)) / 16].HasTile)
            {
                lookFurther++;
                if (lookFurther % 2 == 0) progressText = "Looking " + lookFurther + " tiles further";
                PlaceWater();
            }
            else lookFurther = 0;
            GenerateNewMiner();
        }
        public void ToTheSurfaceMove() // it likes randomizer = 3
        {
            moveID = 0;
            if (Main.tile[(int)(minerPos.X + 16 + (16 * lookFurther)) / 16, (int)minerPos.Y / 16].HasTile)
            {
                moveID++;
            }
            if (Main.tile[(int)(minerPos.X - 16 - (16 * lookFurther)) / 16, (int)minerPos.Y / 16].HasTile)
            {
                moveID++;
            }
            if (Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y + 16 + (16 * lookFurther)) / 16].HasTile)
            {
                moveID++;
            }
            if (Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y - 16 - (16 * lookFurther)) / 16].HasTile)
            {
                moveID++;
            }
            int randomizer = WorldGen.genRand.Next(0, moveID);
            size = WorldGen.genRand.Next(1, 3);
            if (randomizer == 0)
            {
                lookFurther++;
                int adjust = WorldGen.genRand.Next(1, 4);
                if (adjust == 1)
                {
                    minerPos.X -= 16;
                    PlaceWater();
                    Dig();
                }
                else if (adjust == 2)
                {
                    minerPos.X += 16;
                    PlaceWater();
                    Dig();
                }
                else if (adjust == 3)
                {
                    minerPos.Y -= 16;
                    PlaceWater();
                    Dig();
                }
                else if (adjust == 4)
                {
                    minerPos.Y += 16;
                    PlaceWater();
                    Dig();
                }
                return;
            }
            if (randomizer == 1)
            {
                minerPos.X -= 16;
                Dig();
            }
            if (randomizer == 2)
            {
                minerPos.Y -= 16;
                Dig();
            }
            if (randomizer == 3)
            {
                minerPos.Y += 16;
                Dig();
            }
            if (randomizer == 4)
            {
                minerPos.X += 16;
                Dig();
            }
            GenerateNewMiner();
            lookFurther = 0;
        }
        public void StiltedMove()    // stilted, might work if more iterations of movement, sometimes longest tunnel
        {                                   // best water placer, there's another move that could be extracted from this if the ID segments were removed
            moveID = 0;
            if (Main.tileSolid[Main.tile[(int)(minerPos.X + 16 + (16 * lookFurther)) / 16, (int)minerPos.Y / 16].TileType])
            {
                moveID++;
            }
            if (Main.tileSolid[Main.tile[(int)(minerPos.X - 16 - (16 * lookFurther)) / 16, (int)minerPos.Y / 16].TileType])
            {
                moveID++;
            }
            if (Main.tileSolid[Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y + 16 + (16 * lookFurther)) / 16].TileType])
            {
                moveID++;
            }
            if (Main.tileSolid[Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y - 16 - (16 * lookFurther)) / 16].TileType])
            {
                moveID++;
            }
            int randomizer = WorldGen.genRand.Next(0, moveID);
            size = WorldGen.genRand.Next(1, 3);
            if (randomizer == 0)
            {
                lookFurther++;
                int adjust = WorldGen.genRand.Next(1, 4);
                if (adjust == 1)
                {
                    minerPos.X -= 16 * 2;
                    PlaceWater();
                }
                else if (adjust == 2)
                {
                    minerPos.X += 16 * 2;
                    PlaceWater();
                }
                else if (adjust == 3)
                {
                    minerPos.Y -= 16 * 2;
                    PlaceWater();
                }
                else if (adjust == 4)
                {
                    minerPos.Y += 16 * 2;
                    PlaceWater();
                }
                return;
            }
            if (randomizer == 1 && WorldGen.genRand.Next(6) == 2)
            {
                minerPos.X -= 16;
                Dig();
            }
            if (randomizer == 2 && WorldGen.genRand.Next(10) == 4)
            {
                minerPos.Y -= 16;
                Dig();
            }
            if (randomizer == 3)
            {
                minerPos.Y += 16;
                Dig();
            }
            if (randomizer == 4 && WorldGen.genRand.Next(5) == 4)
            {
                minerPos.X += 16;
                Dig();
            }
            GenerateNewMiner();
            lookFurther = 0;
        }
        public void GenerateNewMiner()
        {
            int randomizer = WorldGen.genRand.Next(0, 100);
            if (randomizer < 20 && whoAmI < 800)
            {
                //  Codable.RunGlobalMethod("ModWorld", "miner.Init", new object[] { 0 });
                //  progressText = "Miner " + whoAmI + " created";
                whoAmI++;

                //  unecessary, jobCount takes care of new mining tasks
                //  miner.whoAmI = whoAmI;
                /*  int newMiner = NewMiner(minerPos.X, minerPos.Y, 0, whoAmI);
                    ID[newMiner].init = false;
                    ID[newMiner].Dig(); */
                //  miner.ID[newID].minerPos = Miner.minerPos;
            }
        }
        public void Dig()
        {
            if (type < 800 * 800)
            {
                type++;
                minePath[type] = minerPos;
            }

            if (!switchMode)
            {
                for (int k = 2; k < 24; k++)
                {
                    int i = (int)minerPos.X / 16;
                    int j = (int)minerPos.Y / 16;
                    Tile[] tiles = new Tile[]
                    {
                        Main.tile[i + k, j + k],
                        Main.tile[i - k, j - k],
                        Main.tile[i + k, j - k],
                        Main.tile[i - k, j + k]
                    };
                    foreach (Tile tile in tiles)
                    {
                        if (tile != null)
                        {
                            tile.TileType = ArchaeaWorld.magnoStone;
                            tile.HasTile = true;
                        }
                    }
                    WorldGen.KillWall((int)minerPos.X / 16 + k, (int)minerPos.Y / 16 + k, false);
                    WorldGen.KillWall((int)minerPos.X / 16 - k, (int)minerPos.Y / 16 - k, false);
                    WorldGen.KillWall((int)minerPos.X / 16 + k, (int)minerPos.Y / 16 - k, false);
                    WorldGen.KillWall((int)minerPos.X / 16 - k, (int)minerPos.Y / 16 + k, false);
                }
            }
            if (switchMode)
            {
                for (int k = 0; k < type; k++)
                {
                    minerPos = minePath[k];
                    if (WorldGen.genRand.Next(60) == 0) PlaceWater();
                    if (size == 1)
                    {
                        int i = (int)minerPos.X / 16;
                        int j = (int)minerPos.Y / 16;
                        Tile[] tiles = new Tile[]
                        {
                            Main.tile[i + circumference, j + circumference],
                            Main.tile[i + circumference, j],
                            Main.tile[i, j + circumference],
                            Main.tile[i, j],
                            Main.tile[i + 1, j],
                            Main.tile[i - 1, j],
                            Main.tile[i, j + 1],
                            Main.tile[i, j - 1]
                        };
                        foreach (Tile tile in tiles)
                        {
                            tile.TileType = 0;
                            tile.HasTile = false;
                        }
                    }
                    else if (size == 2)
                    {
                        //MINER worldgen replaced
                        //Main.tile[(int)(minerPos.X / 16) + circumference, (int)(minerPos.Y / 16) + circumference].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + circumference, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + circumference * 2, (int)(minerPos.Y / 16) + circumference * 2].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + circumference * 2, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference * 2].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) + 1].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) - 1].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16) + 1].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16) - 1].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16) - 1].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16) + 1].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + 2, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) - 2, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) + 2].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) - 2].HasTile = false;
                    }
                    else if (size == 3)
                    {
                        //MINER worldgen replaced
                        //Main.tile[(int)(minerPos.X / 16) + circumference, (int)(minerPos.Y / 16) + circumference].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + circumference, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + circumference * 2, (int)(minerPos.Y / 16) + circumference * 2].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + circumference * 2, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference * 2].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + circumference * 3, (int)(minerPos.Y / 16) + circumference * 3].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + circumference * 3, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference * 3].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) + 1].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) - 1].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16) + 1].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16) - 1].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16) - 1].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16) + 1].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + 2, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) - 2, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) + 2].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) - 2].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + 2, (int)(minerPos.Y / 16) + 2].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) - 2, (int)(minerPos.Y / 16) - 2].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + 2, (int)(minerPos.Y / 16) - 2].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) - 2, (int)(minerPos.Y / 16) + 2].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) + 3, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16) - 3, (int)(minerPos.Y / 16)].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) + 3].HasTile = false;
                        //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) - 3].HasTile = false;
                    }
                    if (WorldGen.genRand.NextFloat() > 0.5f)
                        Main.tile[(int)minerPos.X / 16, (int)minerPos.Y / 16].WallType = ArchaeaWorld.magnoCaveWall;
                }
            }
        }
        private void CaveWalls(int i, int j)
        {
            if (WorldGen.genRand.NextFloat() > 0.50f)
            {
                int radius = WorldGen.genRand.Next(8, 24);
                for (int n = 0; n < radius; n++)
                {
                    for (float r = 0f; r < Math.PI * 2; r += Draw.radians(n))
                    {
                        float cos = i + (float)(n * (Math.Cos(r)));
                        float sine = j + (float)(n * (Math.Sin(r)));
                        Main.tile[(int)cos, (int)sine].WallType = ArchaeaWorld.magnoCaveWall;
                    }
                }
            }
        }
        public void PlaceWater()
        {
            int randomizer = WorldGen.genRand.Next(0, 100);
            if (randomizer < 8)
            { // old randomizer%12 == 0
                //MINER worldgen replaced
                //Main.tile[(int)(minerPos.X / 16) + circumference, (int)(minerPos.Y / 16)].liquid = 255;
                //Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference].liquid = 255;
                WorldGen.SquareTileFrame((int)(minerPos.X / 16), (int)(minerPos.Y / 16));
            }
            if (circumference != 1) return;
        }
        public void Mine()
        {
            //	AverageMove();
            DirectionalMove();
            //	ToTheSurfaceMove();
            //	StiltedMove();
            //  used only in the removal of newly generated miners
            /*          if(russianRoulette){
                            int life = -5;
                            int death = 60;
                            int roulette = WorldGen.genRand.Next(life, death);
                            if(roulette == death && whoAmI > 0){
                                ID[whoAmI] = null;
                                whoAmI++;
                            }
                        }   */
            if (!switchMode && minerPos != Vector2.Zero && jobCount < jobCountMax/* && (minerPos.X < edge * 16 && minerPos.X > (rightBounds - edge) * 16 && minerPos.Y < edge * 16 && minerPos.Y > (bottomBounds - edge) * 16)*/)
            {
                int RandomX = WorldGen.genRand.Next(-2, 2);
                int RandomY = WorldGen.genRand.Next(-2, 2);
                if (RandomX != 0 && RandomY != 0)
                {
                    if (minerPos.Y / 16 > Main.maxTilesY / 3 && minerPos.Y < bottomBounds - edge)
                    {
                        mineBlock = new Vector2(mineBlockX * RandomX, mineBlockY * RandomY);
                        minerPos += mineBlock;
                    }
                    if (minerPos.Y / 16 < Main.maxTilesY / 3)
                    {
                        minerPos.Y += mineBlockY;
                    }
                    if (minerPos.Y / 16 > bottomBounds - edge)
                    {
                        minerPos.Y -= mineBlockY;
                    }
                }
                else return;
            }
        }
        public void Randomizer()
        {
            randomX = WorldGen.genRand.Next(edge, rightBounds - edge);
            randomY = WorldGen.genRand.Next((int)Main.rockLayer, bottomBounds - edge);
            for (int j = -1; j < 1; j++)
            {
                circumference = j;
            }
        }
        public void Terminate()
        {
            jobCount = jobCountMax;
            whoAmI = 800;
            ArchaeaWorld.miner.active = false;
        }
        public void Reset()
        {
            progressText = "";
            type = 0;
            whoAmI = 0;
            jobCount = 0;
            switchMode = false;
            init = false;
            center = new Vector2((Main.maxTilesX / 2) * 16, (Main.maxTilesY / 2) * 16);
            minerPos = center;
            oldMinerPos = default(Vector2);
            genPos[0] = default(Vector2);
            genPos[1] = default(Vector2);
            for (int i = 0; i < minePath.Length - 1; i++)
            {
                minePath[i] = default(Vector2);
            }
            if (Main.maxTilesX == 4200)
                jobCountMax = 32;
            if (Main.maxTilesX == 6400)
                jobCountMax = 48;
            if (Main.maxTilesX == 8400)
                jobCountMax = 64;
        }
    }
}
