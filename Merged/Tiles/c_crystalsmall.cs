using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArchaeaMod.Merged.Tiles
{
    public class c_crystalsmall : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidSide, 1, 0);
            TileObjectData.addAlternate(1);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.addAlternate(0);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidSide, 1, 0);
            TileObjectData.addAlternate(2);
            TileObjectData.addTile(Type);
            ItemDrop = Mod.Find<ModItem>("cinnabar_crystal").Type;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Cinnabar Crystal");
            AddMapEntry(new Color(210, 110, 110), name);
            TileID.Sets.DisableSmartCursor[Type] = true;
            MineResist = 1.2f;
            MinPick = 55;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.604f;
            g = 0.161f;
            b = 0.161f;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return ModContent.GetInstance<ArchaeaWorld>().downedMagno;
        }
        public override bool CanExplode(int i, int j)
        {
            return ModContent.GetInstance<ArchaeaWorld>().downedMagno;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail)
                Item.NewItem(Item.GetSource_NaturalSpawn(), new Vector2(i * 16, j * 16), Mod.Find<ModItem>("cinnabar_crystal").Type, 1, true, 0, true, false);
        }

        bool tileCheckFlip = false;
        int x, y;
        int frame;
        float rotation;
        Texture2D texture;
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {   
            SpriteEffects effects = SpriteEffects.None;
            Tile tile = Main.tile[i, j];

            if (Main.tileSolid[Main.tile[i, j + 1].TileType] && Main.tile[i, j + 1].HasTile && Main.tile[i, j + 1].TileType != 0)
                frame = 3;
            if (Main.tileSolid[Main.tile[i, j - 1].TileType] && Main.tile[i, j - 1].HasTile && Main.tile[i, j - 1].TileType != 0)
                frame = 0;
            if (Main.tileSolid[Main.tile[i + 1, j].TileType] && Main.tile[i + 1, j].HasTile && Main.tile[i + 1, j].TileType != 0)
                frame = 2;
            if (Main.tileSolid[Main.tile[i - 1, j].TileType] && Main.tile[i - 1, j].HasTile && Main.tile[i - 1, j].TileType != 0)
                frame = 1;

            texture = TextureAssets.Tile[Type].Value;

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Main.spriteBatch.Draw(texture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(0, frame * 18, 16, 16),
                Lighting.GetColor(i, j), 0f, default(Vector2), 1f, effects, 0f);
            return false;
        }
    }
}
