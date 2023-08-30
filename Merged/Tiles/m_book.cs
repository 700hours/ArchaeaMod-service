using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArchaeaMod.Merged.Tiles
{
    public class m_book : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileLighted[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileNoSunLight[Type] = false;

            //DustType = 1;
            //ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ItemID.Book;
            //  UI map tile color
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Books");
            AddMapEntry(new Color(201, 101, 101), name);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Main.tile[i, j].TileFrameX == 90)
                noItem = true;
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            int type = item.createTile;
            if (type < 0)
                return;

            Tile tile = Main.tile[i, j];
            int num = Main.rand.Next(4);
            tile.TileFrameX = (short)(18 * num);
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 90)
            {
                yield return new Item(ModContent.ItemType<Merged.Items.magno_book>());
            }
        }
        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 90)
            {
                int t = Item.NewItem(Item.GetSource_NaturalSpawn(), i * 16, j * 16, 8, 8, ModContent.ItemType<Merged.Items.magno_book>(), 1, false, -1, true, false);
                if (Main.netMode != 0)
                    NetHandler.Send(Packet.SpawnItem, -1, -1, t, i * 16, j * 16);
                WorldGen.KillTile(i, j, false, false, true);
                return true;
            }
            return false;
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 90)
            {
                player.cursorItemIconID = ModContent.ItemType<Merged.Items.magno_book>();
            }
        }
    }
}
