using System;
using System.Collections.Generic;
using PointF = System.Drawing.PointF;
using Bitmap = System.Drawing.Bitmap;
using Graphics = System.Drawing.Graphics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items.Alternate
{
    using ArchaeaMod.Effects;

    public class MagnoCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Cannon");
            Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            //Item.mana = 1;
            Item.damage = 10;
            Item.value = 10000;
            Item.channel = true;
            Item.useTime = 3;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 3;
            Item.autoReuse = true;
            Item.reuseDelay = 3;
            //Item.expert = true;
            Item.rare = ItemRarityID.Expert;
        }
        public static Texture2D tex;
        public static float angle;
        public static int height = 100;
        SpriteBatch sb => Main.spriteBatch;
        public override bool? UseItem(Player player)
        {
            return false;
            angle = NPCs.ArchaeaNPC.AngleTo(player.Center, Main.MouseWorld);
            float distance = Vector2.Distance(player.Center, Main.MouseWorld);
            var point = Fx.GenerateImage(height, (int)distance, true);
            //for (int i = 0; i < point.Length; i++)
            //{
                
            //    point[i].X = (float)(player.Center.X + point[i].X + i * Math.Cos(angle));
            //    point[i].Y = (float)(player.Center.Y + point[i].Y + i * Math.Sin(angle));
                
            //}
            var fx = Fx.GenerateImage(100, (int)distance, true);
            tex = Texture2D.FromStream(Main.graphics.GraphicsDevice, fx);
            
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //if (tex != null)
            //{ 
            //    Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            //    if (Main.drawToScreen)
            //    {
            //        zero = Vector2.Zero;
            //    }
            //    sb.Draw(tex, player.Center - Main.screenPosition + zero, null, Color.White, (float)angle, default(Vector2), 1f, SpriteEffects.None, 0f);
            //}
            //sb.End();
            fx.Dispose();
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ItemID.Wood)
                .Register();
        }
    }
}
namespace ArchaeaMod.Effects
{
    public static class Ext
    {
        public static Vector2 ToXnaVector2(in PointF a)
        {
            return new Vector2(a.X, a.Y);
        }
    }
    public class Fx
    { 
        public static MemoryStream GenerateImage(int height, int distance, bool inUse, bool style = true)
        {
            PointF[] oldPoints = new PointF[] { };
            int width = distance;
            MemoryStream mem = new MemoryStream();

            using (Bitmap bitmap = new Bitmap(distance, height))
            {
                using (Graphics graphic = Graphics.FromImage(bitmap))
                { 
                    var data = _Buffer(width);

                    float num = data.Max();
                    float num2 = data.Min();
                    float num3 = data.Average();
                    int[] indexArray = new int[3];
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (num == data[i])
                            indexArray[0] = i;
                        if (num2 == data[i])
                            indexArray[1] = i;
                        if (num3 == data[i])
                            indexArray[2] = i;
                    }
                    int length = indexArray.Max() - indexArray.Min();
                    if (length + indexArray[2] < width)
                        length += indexArray[2];

                    PointF[] points = new PointF[width];
                    if (inUse)
                    {
                        for (int i = 0; i < points.Length; i += points.Length / Math.Max(length, 1))
                        {
                            float y = height / 2 * (float)(data[i] * (style ? Math.Sin((float)i / width * Math.PI) : 1f)) + height / 2;
                            points[i] = new PointF(Math.Min(i, points.Length), y);
                        }
                        PointF begin = new PointF();
                        bool flag = false;
                        int num4 = 0;
                        for (int i = 1; i < points.Length; i++)
                        {
                            if (points[i] == default(PointF) && !flag)
                            {
                                begin = points[i - 1];
                                num4 = i;
                                flag = true;
                            }
                            if ((points[i] != default(PointF) || i == points.Length - 2) && flag)
                            {
                                for (int j = num4; j < i; j++)
                                {
                                    points[j] = new PointF(begin.X, begin.Y);
                                }
                                flag = false;
                            }
                        }
                        for (int i = points.Length - 1; i > 0; i--)
                        {
                            if (points[i].X == 0f)
                                points[i].X = i;
                            if (points[i].Y == 0f)
                                points[i].Y = points[i - 1].Y;
                        }
                        points[points.Length - 1] = points[points.Length - 2];
                    }
                    graphic.FillRectangle(System.Drawing.Brushes.Black, new System.Drawing.Rectangle(0, 0, width, height));
                    if (points.Length > 1)
                    {
                        var pen = new System.Drawing.Pen(System.Drawing.Brushes.White);
                        pen.Width = 1;
                        graphic.DrawCurve(pen, points);
                        oldPoints = points;
                    }
                    bitmap.MakeTransparent(System.Drawing.Color.Black);
                    bitmap.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                    return mem;
                }
            }
            
        }
        private static float[] _Buffer(int length)
        {
            try
            {
                byte[] buffer = new byte[length];
                float[] array = new float[length];
                Main.rand.NextBytes(buffer);
                for (int i = 0; i < length; i++)
                    array[i] = (float)buffer[i] / byte.MaxValue * Main.rand.Next(new[] { -1, 1 });
                return array;
            }
            catch
            {
                return new float[] { 0f };
            }
        }
    }
}