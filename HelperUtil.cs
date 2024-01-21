using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArchaeaMod.NPCs;
using Terraria.GameContent;
using Terraria;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ArchaeaMod
{
    internal class HelperUtil
    {
        public static Vector2 OrbitAngle(Vector2 origin, float angle, float radius)
        {
            float cos = (float)(radius * Math.Cos(angle));
            float sine = (float)(radius * 1.5f * Math.Sin(angle));
            return origin + new Vector2(cos, sine);
        }
        public static void DrawWeightedBar(Texture2D tex, float x, float y, ref int num, int width, int height, SpriteBatch sb)
        {
            int max = height;
            int cX = (int)x;
            int cY = (int)y;
            x -= Main.screenPosition.X;
            y -= Main.screenPosition.Y;
            int h = (int)(Math.Abs((float)num / Math.Max(max, 1)) * height);
            sb.Draw(tex, new Rectangle((int)x, (int)y, width, h), new Rectangle(0, 0, width, height), Lighting.GetColor(cX / 16, cY / 16));
        }
    }
}
