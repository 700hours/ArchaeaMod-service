using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArchaeaMod.Merged.Tiles
{
    public class m_chest : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSpelunker[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
            Main.tileContainer[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileID.Sets.BasicChest[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new int[] { 127 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Magnoliac Chest");
            AddMapEntry(new Color(110, 110, 210), name);
            TileID.Sets.DisableSmartCursor[Type] = true;
            AdjTiles = new int[] { TileID.Containers };
            //ContainerName/* tModPorter Note: Removed. Override DefaultContainerName instead */.SetDefault("Magno Chest");
            //ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Merged.Items.Tiles.magno_chest>();
        }
        public override LocalizedText DefaultContainerName(int frameX, int frameY) => CreateMapEntryName();

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            //Item.NewItem(Item.GetSource_NaturalSpawn(), i * 16, j * 16, 16, 16, ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */);
            Chest.DestroyChest(i, j);
        }
        public static void ChestSummon(int i, int j)
        {
            if (Main.netMode == 1 || !Main.hardMode || Main.tile[i, j].TileType != ArchaeaWorld.magnoChest)
                return;
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            int x = i * 16;
            int y = j * 16;
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }
            if (tile.TileFrameY != 0)
            {
                top--;
            }
            int chest = Chest.FindChest(left, top);
            
            if (Main.chest[chest].item.Count(t => t.active && !t.IsAir) == 1 && Main.chest[chest].item.Count(t => !t.IsAir && t.type == ItemID.GoldenKey) == 1)
            {
                var key = Main.chest[chest].item.First(t => !t.IsAir && t.type == ItemID.GoldenKey);
                key.TurnToAir();
                WorldGen.KillTile(i, j, noItem: true);
                int n = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x, y, ModNPCID.Mimic);
                Chest.DestroyChest(i, j);
                if (Main.netMode == 2)
                { 
                    NetMessage.SendData(MessageID.ChestUpdates, -1, -1, null, 1, x, y, 0f, chest);
                    NetMessage.SendTileSquare(-1, x, y, 3);
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
                }
            }
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            Main.mouseRightRelease = false;
            int left = i;
            int top = j;
            player.CloseSign();
            player.SetTalkNPC(-1);
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }
            if (tile.TileFrameY != 0)
            {
                top--;
            }
            if (Main.netMode == 1) 
            {
                if (left == player.chestX && top == player.chestY && player.chest >= 0)
                {
                    player.chest = -1;
                    Recipe.FindRecipes();
                    SoundEngine.PlaySound(SoundID.MenuClose);
                }
                else
                {
                    NetMessage.SendData(31, -1, -1, null, left, top, 0f, 0f, 0, 0, 0);
                    Main.stackSplit = 600;
                }
                return true;
            }
            else
            {
                int chest = Chest.FindChest(left, top);
                if (chest >= 0)
                {
                    Main.stackSplit = 600;
                    if (chest == player.chest)
                    {
                        player.chest = -1;
                        SoundEngine.PlaySound(SoundID.MenuClose);
                    }
                    else
                    {
                        //player.chest = chest;
                        //Main.playerInventory = true;
                        //Main.recBigList = false;
                        //player.chestX = left;
                        //player.chestY = top;
                        player.OpenChest(left, top, chest);
                        SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
                    }
                    Recipe.FindRecipes();
                }
                return true;
            }
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }
            if (tile.TileFrameY != 0) 
            {
                top--;
            }
            int chest = Chest.FindChest(left, top);
            player.cursorItemIconID = -1;
            if (chest < 0)
            {
                player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
            }
            else
            {
                player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : "Magno Chest";
                if(player.cursorItemIconText == "Magno Chest")
                {
                    player.cursorItemIconID = ModContent.ItemType<Merged.Items.Tiles.magno_chest>();
                    player.cursorItemIconText = "";
                }
            }
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }
        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if(player.cursorItemIconText == "")
            {
                player.cursorItemIconEnabled = false;
                player.cursorItemIconID = 0;
            }
        }
    }
}
