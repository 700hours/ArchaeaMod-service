using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using cotf.Base;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Drawing;
using tUserInterface.ModUI;
using Color = Microsoft.Xna.Framework.Color;
using cotf;
using ArchaeaMod.NPCs;
using Terraria.ID;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ArchaeaMod.Composite
{
    internal class Composite : ModSystem
    {
        int width, height;
        Lightmap[,] map = new Lightmap[,] { };
        //Tile[,] tile = new Tile[,] { };
        SpriteBatch sb => Main.spriteBatch;
        public override void OnWorldLoad()
        {
            width  = 0;
            height = 0;
        }
        public override void PostDrawTiles()
        {
            if (Main.screenWidth != width || Main.screenHeight != height)
            {
                width  = Main.screenWidth;
                height = Main.screenHeight;
                map    = new Lightmap[width / 16, height / 16];
                //tile   = new Tile[width / 16, height / 16];
                int originX = (int)Main.screenPosition.X / 16;
                int originY = (int)Main.screenPosition.Y / 16;
                int w       = width / 16;
                int h       = height / 16;
                int right   = originX + w;
                int bottom  = originY + h;
                for (int i = originX + 1; i < right; i++)
                {
                    for (int j = originY + 1; j < bottom; j++)
                    {
                        int m = i - originX - 1;
                        int n = j - originY - 1;
                        map[m, n]  = new Lightmap(m, n);
                        //tile[m, n] = new Tile(m, n);
                    }
                }
            }
            Composite2D[,] comp = getTextureArray();

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int i = 1; i < comp.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < comp.GetLength(1) - 1; j++)
                {
                    float x = Main.screenPosition.X + i * 16f;
                    float y = Main.screenPosition.Y + j * 16f;
                    x -= x % 16;
                    y -= y % 16;
                    if (comp[i, j] != default)
                    { 
                        sb.Draw(comp[i, j].texture, new Rectangle((int)(x - Main.screenPosition.X), (int)(y - Main.screenPosition.Y), comp[i, j].tileWidth, comp[i, j].tileHeight), new Rectangle(comp[i, j].tileFrameX, comp[i, j].tileFrameY, comp[i, j].tileWidth, comp[i, j].tileHeight), Color.White, 0, Vector2.Zero, comp[i, j].tileSpriteEffect, 0f);
                    }
                }
            }
            sb.End();
        }
        public static Color getXnaColor(System.Drawing.Color c)
        {
            return new Color(c.R, c.G, c.B, c.A);
        }
        private Composite2D[,] getTextureArray()
        {
            int originX = (int)Main.screenPosition.X / 16;
            int originY = (int)Main.screenPosition.Y / 16;
            int w = width / 16;
            int h = height / 16;
            int right = originX + w;
            int bottom = originY + h;
            Composite2D[,] comp = new Composite2D[w, h];
            for (int i = originX; i < right; i++)
            {
                for (int j = originY; j < bottom; j++)
                {
                    int m = i - originX;
                    int n = j - originY;
                    if (Main.tile[i, j].HasTile)
                    {
                        comp[m, n] = new Composite2D();
                        comp[m, n].texture = Main.instance.TilesRenderer.GetTileDrawTexture(Main.tile[i, j], i, j);
                        comp[m, n].tileFrameX = Main.tile[i, j].TileFrameX;
                        comp[m, n].tileFrameY = Main.tile[i, j].TileFrameY;
                        Main.instance.TilesRenderer.GetTileDrawData(i, j, Main.tile[i, j], Main.tile[i, j].TileType, ref Main.tile[i, j].TileFrameX, ref Main.tile[i, j].TileFrameY, out comp[m, n].tileWidth, out comp[m, n].tileHeight, out comp[m, n].tileTop, out comp[m, n].halfBrickHeight, out comp[m, n].addFrX, out comp[m, n].addFrY, out comp[m, n].tileSpriteEffect, out _, out _, out _);
                    }
                    else comp[m, n] = default;
                }
            }
            return comp;
        }
        public static void LampEffect(Vector2 target, Lightmap map, Color c, float range = 200f)
        {
            float num = RangeNormal(target, map.Center, range);
            if (num == 0f)
                return;
            map.alpha = 0f;
            map.alpha += Math.Max(0, num);
            map.alpha = Math.Min(map.alpha, 1f);
            map.color = AdditiveV2(map.color, c, num / 2f);
        }
        public static float RangeNormal(Vector2 to, Vector2 from, float range = 100f)
        {
            return Math.Max((Vector2.Distance(from, to) * -1f + range) / range, 0);
        }
        public static Color AdditiveV2(Color color, Color newColor)
        {
            return new Color(
                (int)Math.Min(color.R + newColor.R, 255),
                (int)Math.Min(color.G + newColor.G, 255),
                (int)Math.Min(color.B + newColor.B, 255),
                color.A);
        }
        public static Color AdditiveV2(Color color, Color newColor, float distance)
        {
            return new Color(
                (int)Math.Min(color.R + newColor.R * distance, 255),
                (int)Math.Min(color.G + newColor.G * distance, 255),
                (int)Math.Min(color.B + newColor.B * distance, 255),
                color.A);
        }
    }
    public class Composite2D
    {
        public Texture2D texture;
        public short 
            tileFrameX,
            tileFrameY;
        public int 
            tileWidth,
            tileHeight,
            tileTop,
            halfBrickHeight,
            addFrX, 
            addFrY;
        public SpriteEffects tileSpriteEffect;
    }
}
