using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Terraria;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace tMod.UI
{
    public class Element
    {
        public string text = "";
        public bool active, draw = true;
        public bool LeftClick()
        {
            return Main.mouseLeftRelease && Main.mouseLeft && HoverOver();
        }
        public bool KeyPress(Keys key)
		{
			return Main.oldKeyState.IsKeyUp(key) && Main.keyState.IsKeyDown(key);
		}
        public bool KeyHold(Keys key)
        {
            return Main.keyState.IsKeyDown(key);
        }
        public virtual bool HoverOver()
        {
            return bounds.Contains(Main.MouseScreen.ToPoint());
        }
        public int x, y, width, height, padding;
        public const int fontHeight = 16;
        public float alpha = 1f;
        public Rectangle bounds
        {
            get { return new Rectangle(x, y, width, height); }
        }
        public Color color = Color.DodgerBlue;
        public virtual void Update()
        {
        }
        public virtual void Draw()
        {
            SpriteBatch sb = Main.spriteBatch;
            if (draw)
            {
                sb.Draw(TextureAssets.MagicPixel.Value, bounds, color);
                sb.DrawString(FontAssets.MouseText.Value, text, new Vector2(bounds.X + padding, bounds.Y + padding), Color.White);
            }
        }
    }
    public class Textbox : Element
    {
        public Textbox(int x, int y, int width, int height, string text, int padding = 4, bool readOnly = true, bool multiLine = true)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.text = text;
            this.padding = padding;
            this.readOnly = readOnly;
            this.multiLine = multiLine;
        }
        private bool init;
        public bool readOnly;
        public bool multiLine;
        public int textLength
        {
            get { return 10 * text.Length; }
        }
        public override void Update()
        {
            if (!init)
            {
                if (readOnly)
                    WrapText();
                init = true;
            }
            if (LeftClick())
                active = !active;
            color = active ? Color.DodgerBlue * 0.67f : Color.DodgerBlue * 0.33f;
            InputText();
        }
        public void WrapText()
        {
            text = text.Replace("\n", "");
            int W = width - (width % 10);
            if (textLength > width) {
                for (int i = 0; i < text.Length; i++)
                if (i * 10 % W == 0 && i != 0)
                {
                    int index = text.Substring(i).IndexOf(" ") + i + 1;
                    text = text.Insert(index, "\n");
                }
            }
        }
        public void InputText()
        {
            if (!readOnly && active && Main.oldKeyState != Main.keyState)
            {
                foreach (Keys key in Main.keyState.GetPressedKeys())
                {
                    foreach (Keys k in new Keys[] { Keys.Tab, Keys.CapsLock, Keys.LeftAlt, Keys.LeftControl, Keys.LeftWindows, Keys.RightAlt, Keys.RightControl, Keys.RightWindows })
                        if (key == k)
                            return;
                    if (key.ToString().Contains('D') && key.ToString().Length > 1)
                    {
                         text += key.ToString().Trim('D');
                         break;
                    }
                    if (key == Keys.Enter)
                    {
                        text += "\n";
                        break;
                    }
                    if (key == Keys.Back)
                    {
                        text = text.Substring(0, text.Length - 1);
                        break;
                    }
                    if (key == Keys.Space)
                    {
                        text += " ";
                        break;
                    }
                    if (key != Keys.LeftShift && key != Keys.RightShift)
                    {
                        if (KeyHold(Keys.LeftShift) || KeyHold(Keys.RightShift))
                        {
                            if (key == Keys.OemQuestion)
                            {
                                text += "?";
                                break;
                            }
                            text += key.ToString().ToUpper();
                            break;
                        }
                        text += key.ToString().ToLower();
                    }
                }
                WrapText();
            }
        }
    }
    public class Button : Element
    {
        public Button(int x, int y, string text, float alpha = 0.67f, Color bg = default(Color))
        {
            this.x = x;
            this.y = y;
            this.text = text;
            this.width = text.Length * 10;
            this.height = 16;
            this.alpha = alpha;
            this.padding = 0;
            this.bg = bg;
        }
        public Color bg;
        public override void Update()
        {
            color = HoverOver() ? bg * 0.67f : bg * 0.33f;
        }
    }
    public class ProgressBar : Element
    {
        public ProgressBar(int x, int y, int width, int height, string text)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.text = text;
            this.padding = 2;
        }
        public const float min = 0f, max = 1f;
        public float value
        {
            get { return prereq(); }
        }
        internal Rectangle progress
        {
            get { return new Rectangle(x, y, (int)(width / value), height); }
        }
        public Color valueColor = Color.DodgerBlue;
        public Func<float> prereq;
        public override void Draw()
        {
            SpriteBatch sb = Main.spriteBatch;
            if (draw)
            {   
                sb.Draw(TextureAssets.MagicPixel.Value, bounds, Color.Black);
                sb.Draw(TextureAssets.MagicPixel.Value, progress, valueColor);
                sb.DrawString(FontAssets.MouseText.Value, text, new Vector2(bounds.X, bounds.Y - fontHeight - padding), Color.White);
            }
        }
    }
}