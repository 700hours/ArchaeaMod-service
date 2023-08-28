using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace ArchaeaMod.Merged.Tiles
{
    public class m_stone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            Main.tileMerge[Type][ArchaeaWorld.crystalBlock] = true;
            Main.tileMerge[Type][ArchaeaWorld.magnoOre] = true;
            ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Merged.Items.Tiles.magno_stone>();
            //  UI map tile color
            AddMapEntry(new Color(119, 111, 98));
           // soundStyle = 0;
            HitSound = SoundID.Tink;
            MineResist = 2.5f;
            MinPick = 35;
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            type = ModContent.DustType<Dusts.magno_dust>();
            return true;
        }
        public override bool HasWalkDust()
        {
            return true;
        }
        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            dustType = ModContent.DustType<Merged.Dusts.magno_dust>();
            makeDust = true;
            color = Color.White;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        //  need to add custom waters namespace
        public override void ChangeWaterfallStyle(ref int style)
        {
            style = WaterStyleID.Cavern;
        }   
        public override void RandomUpdate(int i, int j)
        {
        /*  Tile tile = Main.tile[i, j - 1];

            bool random = Main.rand.Next(2) == 0;

            if (random && !Main.tileSolid[Main.tile[i - 1, j].type] && Main.tile[i - 1, j].slope() == 0)
                WorldGen.PlaceTile(i - 1, j, ModContent.TileType<c_crystalwall>(), true, false);
            if (random && !Main.tileSolid[Main.tile[i + 1, j].type] && Main.tile[i + 1, j].slope() == 0)
                WorldGen.PlaceTile(i + 1, j, ModContent.TileType<c_crystalwall>(), true, false);
            if (!Main.tileSolid[tile.type] && !tile.active() && tile.slope() == 0)
            {
                int chance = Main.rand.Next(4);
                switch (chance)
                {
                    case 0:
                        WorldGen.PlaceTile(i, j - 1, ModContent.TileType<c_crystal2x2>(), true, false);
                        break;
                    case 1:
                        WorldGen.PlaceTile(i, j - 1, ModContent.TileType<c_crystal2x1>(), true, false);
                        break;
                }
            }   */
        /*  if(tile.type == ModContent.TileType<c_crystal2x2>() || tile.type == ModContent.TileType<c_crystal2x1>() ||
                Main.tile[i - 1, j].type == ModContent.TileType<c_crystalwall>() || Main.tile[i + 1, j].type == ModContent.TileType<c_crystalwall>())
                Main.NewText("Crystal spawned? i, " + i + " j, " +j, Color.White);  */
        }
    }
}
