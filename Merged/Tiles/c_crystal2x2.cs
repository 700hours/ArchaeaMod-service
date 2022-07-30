using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArchaeaMod.Merged.Tiles
{
    public class c_crystal2x2 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorInvalidTiles = new int[] { 127 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(0, 1);
            TileObjectData.addAlternate(0);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Cinnabar Crystal");
            AddMapEntry(new Color(210, 110, 110), name);
            TileID.Sets.DisableSmartCursor[Type] = true;
            HitSound = SoundID.Item27;
            MineResist = 2f;
            MinPick = 45;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.604f;
            g = 0.161f;
            b = 0.161f;
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            int x = i * 16;
            int y = j * 16;
            if (Main.rand.NextBool(60))
            {
                Dust.NewDust(new Vector2(x, y), 16, 16, Main.rand.NextBool(2) ? ModContent.DustType<ArchaeaMod.Dusts.Shimmer_1>() : ModContent.DustType<ArchaeaMod.Dusts.Shimmer_2>());
            }
        }
        public override bool KillSound(int i, int j, bool fail)
        {
            SoundEngine.PlaySound(SoundID.Item27, new Vector2(i * 16, j * 16));
            return false;
        }
        public override bool Slope(int i, int j)
        {
            return false;
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return ModContent.GetInstance<ArchaeaWorld>().downedMagno;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            type = ModContent.DustType<Dusts.cinnabar_dust>();
            return true;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            noItem = true;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int random = Main.rand.Next(2, 5);
            for (int k = 0; k < random; k++)
            {
                Item.NewItem(Item.GetSource_NaturalSpawn(), new Vector2(i * 16, j * 16), ModContent.ItemType<Merged.Items.Materials.cinnabar_crystal>(), random, true, 0, true, false);
            }
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return ModContent.GetInstance<ArchaeaWorld>().downedMagno;
        }
        public override bool CanExplode(int i, int j)
        {
            return ModContent.GetInstance<ArchaeaWorld>().downedMagno;
        }

        //public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        //{
        //    if(i % 2 == 1)
        //    {
        //        spriteEffects = SpriteEffects.FlipHorizontally;
        //    }
        //}
        //bool tileCheckFlip = false;
        Texture2D texture;
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            Tile tile = Main.tile[i, j];

            texture = TextureAssets.Tile[Type].Value;

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Main.spriteBatch.Draw(texture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
                Lighting.GetColor(i, j), 0f, default(Vector2), 1f, effects, 0f);

            return false;
        }
    }
}
