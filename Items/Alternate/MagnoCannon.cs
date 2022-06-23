using System;
using System.Collections.Generic;
using PointF = System.Drawing.PointF;
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
        public override bool? UseItem(Player player)
        {
            float distance = Vector2.Distance(player.Center, Main.MouseWorld);
            var point = Fx.GenerateImage(100, (int)distance, true);
            for (int i = 0; i < point.Length; i++)
            {
                double angle = NPCs.ArchaeaNPC.AngleTo(player.Center, Main.MouseWorld);
                point[i].X = (float)(player.Center.X + point[i].X + i * Math.Cos(angle));
                point[i].Y = (float)(player.Center.Y + point[i].Y + i * Math.Sin(angle));
                if (i % 2 == 1)
                { 
                    for (float n = point[i - 1].X; n < point[i].X; n++)
                    {
                        for (float m = point[i - 1].Y; m < point[i].Y; m++)
                        {
                            angle = NPCs.ArchaeaNPC.AngleTo(Ext.ToXnaVector2(point[i - 1]), Ext.ToXnaVector2(point[i]));
                            int d = (int)Vector2.Distance(Ext.ToXnaVector2(point[i - 1]), Ext.ToXnaVector2(point[i]));
                            for (int j = 0; j < d; j++)
                            { 
                                double cos  = n + j * Math.Cos(angle);
                                double sine = m + j * Math.Sin(angle);
                                Dust.NewDustPerfect(new Vector2((float)cos, (float)sine), 6);
                            }
                        }
                    }
                }
            }
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
    public class Ext
    {
        public static Vector2 ToXnaVector2(in PointF a)
        {
            return new Vector2(a.X, a.Y);
        }
    }
    public class Fx
    { 
        public static PointF[] GenerateImage(int height, int distance, bool inUse, bool style = true)
        {
            PointF[] oldPoints = new PointF[] { };
            int width = distance;
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
            if (points.Length > 1)
            {
                oldPoints = points;
            }
            return points;
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