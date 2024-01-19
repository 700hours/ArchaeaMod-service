using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cotf.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = System.Drawing.Rectangle;

namespace cotf.Base
{
    public class Lightmap
    {
        public bool active;
        private int i, j;
        public Color color;
        public Color DefaultColor = _DefaultColor;
        public static Color _DefaultColor => Color.Black;
        public Vector2 position;
        public Vector2 Center => position + new Vector2(Size.Width / 2, Size.Height / 2);
        public Rectangle Hitbox => new Rectangle((int)position.X, (int)position.Y, Size.Width, Size.Height);
        public static Size Size => new Size(16, 16);
        public float alpha;
        private Entity parent;
        bool keepLit = false;
        public int ScaleX, ScaleY;
        private Lightmap()
        {
        }
        public Lightmap(int i, int j)
        {
            this.i = i;
            this.j = j;
            position = new Vector2(i * Size.Width, j * Size.Height);
            active = true;
            color = DefaultColor;
        }
        public override string ToString()
        {
            return $"Color:{color}, Alpha:{alpha}, Active:{active}";
        }
    }
}
