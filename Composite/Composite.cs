﻿using Microsoft.Xna.Framework.Graphics;
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
using ArchaeaMod.Effects;
using Terraria.Graphics.Light;
using Point = Microsoft.Xna.Framework.Point;
using ArchaeaMod.NPCs.Bosses;
using System.Threading;

namespace ArchaeaMod.Composite
{
    internal class Composite : ModSystem
    {
        static int width, height;
        int offX = 16, offY = 16;
        Lightmap[,] map = new Lightmap[,] { };
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
                width  = Main.screenWidth + offX * 3;
                height = Main.screenHeight + offY * 3;
                map    = new Lightmap[width / 16, height / 16];
                int originX = (int)(Main.screenPosition.X - offX) / 16;
                int originY = (int)(Main.screenPosition.Y - offY) / 16;
                int w       = (width + offX * 2) / 16;
                int h       = (height + offY * 2) / 16;
                int right   = originX + w;
                int bottom  = originY + h;
                for (int i = originX + 1; i < right - 1; i++)
                {
                    for (int j = originY + 1; j < bottom - 1; j++)
                    {
                        int m = i - originX - 1;
                        int n = j - originY - 1;
                        map[m, n]  = new Lightmap(m, n);
                    }
                }
            }
            Composite2D[,] comp = getTextureArray();
            Lamp[,] lamp = getTorchArray();

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {                                            // Delete offsets for parallax effect
                    float x = Main.screenPosition.X + i * 16f - offX;
                    float y = Main.screenPosition.Y + j * 16f - offY;
                    x -= x % 16;
                    y -= y % 16;
                    if (comp[i, j] != default)
                    {
                        map[i, j].position = new Vector2(x, y) - Main.screenPosition;
                        foreach (Lamp _lamp in lamp)
                        { 
                            if (_lamp != null)
                            { 
                                LampEffect(_lamp.position - Main.screenPosition, ref map[i, j], _lamp.light);
                            }
                        }
                        foreach (Player plr in Main.player)
                        {
                            if (plr.active && ItemID.Sets.Torches[plr.HeldItem.type])
                            {
                                LampEffect(plr.Center - Main.screenPosition, ref map[i, j], Lighting.GetColor((int)plr.Center.X / 16, (int)plr.Center.Y / 16));
                            }
                        }
                        sb.Draw(comp[i, j].texture, new Rectangle((int)(x - Main.screenPosition.X), (int)(y - Main.screenPosition.Y), comp[i, j].tileWidth, comp[i, j].tileHeight), new Rectangle(comp[i, j].tileFrameX, comp[i, j].tileFrameY, comp[i, j].tileWidth, comp[i, j].tileHeight), map[i, j].color, 0, Vector2.Zero, comp[i, j].tileSpriteEffect, 0f);
                        map[i, j].color = map[i, j].DefaultColor;
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
            int originX = (int)(Main.screenPosition.X - offX) / 16;
            int originY = (int)(Main.screenPosition.Y - offY) / 16;
            int w       = (width + offX * 2) / 16;
            int h       = (height + offY * 2) / 16;
            int right   = originX + w;
            int bottom  = originY + h;
            Composite2D[,] comp = new Composite2D[w, h];
            for (int i = originX; i < right - 1; i++)
            {
                for (int j = originY; j < bottom - 1; j++)
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
        public static Lamp[,] getTorchArray()
        {
            Lamp[,] lamp = new Lamp[width / 16, height / 16];
            for (int i = 0; i < width / 16; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int x = i + (int)Main.screenPosition.X / 16;
                    int y = j + (int)Main.screenPosition.Y / 16;
                    if (Main.tile[x, y].HasTile && TileID.Sets.Torch[Main.tile[x, y].TileType])
                    {
                        lamp[i, j] = new Lamp();
                        lamp[i, j].position = new Vector2(x * 16 + 8, y * 16 + 8);
                        lamp[i, j].light = Lighting.GetColor(new Point(x, y));
                    }
                }
            }
            return lamp;
        }
        public static void LampEffect(Vector2 target, ref Lightmap map, Color c, float range = 200f)
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
        public static Color Multiply(Color one, Color two, float alpha)
        {
            int a = (int)Math.Max(Math.Min(255f * Math.Min(alpha, 1f), 255), 1f);
            int r = (int)Math.Min(Math.Max(one.R, 1f) * ((two.R / 255f) + 1), 255);
            int g = (int)Math.Min(Math.Max(one.G, 1f) * ((two.G / 255f) + 1), 255);
            int b = (int)Math.Min(Math.Max(one.B, 1f) * ((two.B / 255f) + 1), 255);
            return new Color(r, g, b, a);
        }
    }
    public class Lamp
    {
        public Vector2 position;
        public Color light;
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
