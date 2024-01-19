using ArchaeaMod.Interface.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Button = tUserInterface.ModUI.Button;
using Terraria.ObjectData;
using System.Security.Policy;
using Terraria.GameContent.ItemDropRules;
using Composite = ArchaeaMod.Composite.Composite;
using System.Drawing;
using ArchaeaMod.NPCs;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using static tModPorter.ProgressUpdate;

namespace ArchaeaMod.Structure
{
    internal class Keypad : ModTile
    {
        public override string Texture => $"ArchaeaMod/Gores/Null";
        public static List<Code> code = new List<Code>();
        public bool 
            interact,
            display,
            send,
            init;
        public int
            i,
            j,
            x, 
            y,
            width,
            height,
            num2;
        public readonly float 
            Range = 16 * 10;
        private string
            complete = "";
        public Portal 
            begin = new Portal(),
            end = new Portal();
        public Code
            _lock;
        public Button[,] 
            input = new Button[3, 3];
        public TextBox[]
            textbox = new TextBox[4];
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(210, 110, 110));
            TileID.Sets.DisableSmartCursor[Type] = true;
            MineResist = 1f;
            MinPick = 45;
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            _lock = new Code();
            _lock.newInput = true;
            _lock.owner = item.playerIndexTheItemIsReservedFor;
            _lock.portal = new Portal() { entrance = new Vector2(i * 16, j *16) };
            code.Add(_lock);
        }
        public override bool RightClick(int i, int j)
        {
            this.i = i;
            this.j = j;
            if (!init)
            {
                x = (int)(Main.screenWidth / 2f - 150);
                y = (int)(Main.screenHeight / 2f - 200);
                width = 300 - 20;
                height = 400;
                int num = 0;
                for (int n = 2; n >= 0; n--)
                {
                    for (int m = 0; m < 3; m++)
                    {
                        input[m, n] = new Button((++num).ToString(), new Rectangle(x + 10 * (m + 1) + m * 80, y + 10 * (n + 1) + n * 80, 80, 80), null) { active = true, drawMagicPixel = true };
                    }
                }
                for (int k = 0; k < textbox.Length; k++)
                {
                    textbox[k] = new TextBox(new Rectangle(x + 10 * (k + 1) + k * 60,  y + height - 90, 50, 50)) { active = true };
                }
                init = true;
            }
            Array.ForEach(textbox, t => t.text = "");
            display = Main.player[Main.myPlayer].InInteractionRange(i, j, TileReachCheckSettings.Simple);
            if (display)
            {
                SoundEngine.PlaySound(SoundID.MenuOpen, Main.player[Main.myPlayer].Center);
            }
            return display;
        }
        public void DrawKeyPad(SpriteBatch sb)
        {
            interact = Main.player[Main.myPlayer].Center.Distance(new Vector2(i * 16, j * 16)) < 64f;
            bool close = (!interact && display) || Main.playerInventory;
            if (close)
            {
                _lock.Clear();
                num2 = 0;
                complete = "";
                display = false;
                return;
            }
            if (!_lock.connected && display)
            {
                int m       = (int)Main.MouseWorld.X; 
                int n       = (int)Main.MouseWorld.Y;
                Rectangle r = new Rectangle(m, n, 16, 16);
                foreach (Code c in code)
                {
                    if (c.portal.Hitbox(false).Intersects(r))
                    {
                        ArchaeaNPC.DrawChain(Mod.Assets.Request<Texture2D>("Gores/chain").Value, sb, _lock.portal.entrance + new Vector2(12, 12), c.portal.entrance + new Vector2(12, 12));
                        if (Main.mouseLeft)
                        {
                            _lock.portal.exit = c.portal.entrance;
                            _lock.connected = true;
                            break;
                        }
                    }
                    else
                    {
                        ArchaeaNPC.DrawChain(Mod.Assets.Request<Texture2D>("Gores/chain").Value, sb, _lock.portal.entrance + new Vector2(8, 8), Main.MouseWorld);
                        continue;
                    }
                }
                return;
            }
            if (close)
            {
                complete = "";
                _lock.Clear();
                num2 = 0;
                SoundEngine.PlaySound(SoundID.MenuClose, Main.player[Main.myPlayer].Center);
                display = false;
            }
            if (display && interact)
            {
                drawKeyPad(sb);
            }
        }
        public void drawKeyPad(SpriteBatch sb)
        {
            x = (int)(Main.screenWidth / 2 - 150);
            y = (int)(Main.screenHeight / 2 - 200);
            sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(x, y, width, height), Color.PaleVioletRed);
            int num = 0;
            for (int n = 2; n >= 0; n--)
            {
                for (int m = 0; m < 3; m++)
                {
                    num++;
                    string t = num.ToString();
                    Rectangle idk = new Rectangle(input[m, n].box.X, input[m, n].box.Y, input[m, n].box.Width, input[m, n].box.Height);
                    input[m, n].Draw(input[m, n].HoverOver(idk));
                    if (input[m, n].LeftClick(idk))
                    {
                        if (input[m, n].reserved == 0)
                        {
                            input[m, n].reserved = 1;
                            if (!_lock.Complete())
                            {
                                complete += t;
                                textbox[num2].text = _lock.Obsfucated(ref t)[num2].ToString();
                                num2++;
                            }
                        }
                    }
                    else input[m, n].reserved = 0;
                }
            }
            for (int m = 0; m < 4; m++)
            {
                sb.Draw(TextureAssets.MagicPixel.Value, textbox[m].box, Color.White * 0.5f);
                textbox[m].DrawText();
            }
            if (_lock.Compare(complete))
            {
                _lock.Clear();
                num2 = 0;
                display = false;
                complete = "";
                Main.player[Main.myPlayer].Teleport(_lock.portal.exit);
            }
        }
    }
    internal struct Portal
    {
        public Vector2 entrance, exit;
        public Rectangle Hitbox(bool getExit)
        {
            Vector2 v = entrance;
            if (getExit)
            {
                v = this.exit;
            }
            return new Rectangle((int)v.X, (int)v.Y, 16, 16);
        }
    }
    internal struct Code
    {
        public bool connected;
        public bool newInput;
        public int owner;
        public string code;
        public string compare;
        public Portal portal;
        private void Initialize()
        {
            if (string.IsNullOrEmpty(code))
            { 
                code = "";
            }
        }
        public bool Complete()
        {
            if (code?.Length >= 4)
            {
                compare = code;
                return true;
            }
            return false;
        }
        public void Clear()
        {
            code = string.Empty;
        }
        public string Obsfucated(ref string add)
        {
            Initialize();
            code += add;
            add = "";
            int len = code.Length + 1;
            string result = "";
            for (int i = 0; i < len; i++)
            {
                result += "*";
            }
            return result;
        }
        public bool Compare(string input)
        {
            return compare == input;
        }
    }
}
