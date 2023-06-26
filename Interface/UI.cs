using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI.Chat;

using ArchaeaMod.Mode;
using Terraria.ID;
using System.Timers;

namespace ArchaeaMod.Interface.UI
{
    public sealed class OptionsID
    {
        public const int
            ClassSelect = 0,
            CordonedBiomes = 1,
            ArchaeaMode = 2;
    }
    public class OptionsUI
    {
        private static string choiceName = "";
        private static string[] classes = new string[] { "Melee", "Ranged", "Magic", "Summoner", "All" };
        private static string[] categories = new string[] { "Class Select", "Cordoned Biomes", "Archaea Mode" };
        private static bool initElements;
        private static int choice = 0;
        private const int offset = 32;
        private static int oldWidth, oldHeight;
        private static Element[] classOptions;
        public static Element[] mainOptions;
        private static Element apply;
        private static Element back;
        private static Element selected;
        private static Element lookup;
        private static Element objectiveBox;
        private static Element toggle;
        private static SpriteBatch sb { get { return Main.spriteBatch; } }
        public static void Initialize()
        {
            mainOptions = new Element[categories.Length];
            classOptions = new Element[classes.Length];
            apply = new Element(Rectangle.Empty);
            back = new Element(Rectangle.Empty);
            oldWidth = Main.screenWidth;
            oldHeight = Main.screenHeight;
            classOptions = new Element[classes.Length];
            mainOptions = new Element[categories.Length];
            for (int i = 0; i < classes.Length; i++)
                classOptions[i] = new Element(Rectangle.Empty);
            for (int j = 0; j < categories.Length; j++)
                mainOptions[j] = new Element(Rectangle.Empty);
            toggle = new Element(new Rectangle(Main.screenWidth / 2 - 24, Main.screenHeight - 80, 48, 48));
            UpdateLocation();
        }
        internal static void UpdateLocation()
        {
            apply.bounds = new Rectangle(Main.screenWidth / 2 - offset, Main.screenHeight / 2 + 48, 64, 64);
            back.bounds = new Rectangle(Main.screenWidth / 2 - offset, Main.screenHeight / 2 + 48, 64, 64);
            for (int i = 0; i < classes.Length; i++)
            {
                float angle = (float)Math.PI * 2f / classes.Length;
                int cos = (int)(Main.screenWidth / 2 + 196f * Math.Cos(angle * i - Math.PI / 2f));
                int sine = (int)(Main.screenHeight / 2 + 196f * Math.Sin(angle * i - Math.PI / 2f));
                classOptions[i].bounds = new Rectangle(cos - offset, sine - offset, 64, 64);
            }
            for (int j = 0; j < categories.Length; j++)
            {
                float angle = (float)Math.PI * 2f / categories.Length;
                int cos = (int)(Main.screenWidth / 2 + 196f * Math.Cos(angle * j - Math.PI / 2f));
                int sine = (int)(Main.screenHeight / 2 + 196f * Math.Sin(angle * j - Math.PI / 2f));
                mainOptions[j].bounds = new Rectangle(cos - offset, sine - offset, 64, 64);
            }
        }
        private static bool reset = true;
        private static bool classSelect;
        private static Mod mod;
        private static ArchaeaPlayer modPlayer;
        internal static bool Toggled = false;
        static bool flag = false;
        static bool flag2 = false;
        public static bool MainOptions(Player player, bool forceDraw = false)
        {
            if (reset)
            {
                Initialize();
                apply.color = Color.Gray;
                mainOptions[2].active = ModContent.GetInstance<ModeToggle>().archaeaMode;
                mainOptions[2].color = mainOptions[2].active ? Color.Blue : Color.White;
                reset = false;
            }
            if (toggle.LeftClick())
            {
                Toggled = true;
                classSelect = false;
            }
            mod = ModLoader.GetMod("ArchaeaMod");
            modPlayer = player.GetModPlayer<ArchaeaPlayer>();
            var modWorld = ModContent.GetInstance<ArchaeaWorld>();
            if (Main.playerInventory)
            { 
                sb.Draw(mod.Assets.Request<Texture2D>("Gores/option").Value, toggle.bounds, null, Color.White);
                if (toggle.HoverOver())
                {
                    sb.DrawString(FontAssets.MouseText.Value, "Archaea Mod options", new Vector2(toggle.bounds.X, toggle.bounds.Bottom), Color.White);
                }
            }
            else if (!forceDraw)
            { 
                Toggled = false;
                return false;
            }
            if (!Toggled && !forceDraw) return false;
            if (classOptions != null && (oldWidth != Main.screenWidth || oldHeight != Main.screenHeight))
            {
                oldWidth = Main.screenWidth;
                oldHeight = Main.screenHeight;
                UpdateLocation();
            }
            if (flag)
            {
                flag = false;
                return false;
            }
            if (classSelect)
            {
                ClassSelect(player);
                return false;
            }
            else
            {
                //  Way too laggy
                //float distance = 196f;
                //for (float r = 0f; r < distance; r += Draw.radians(distance))
                //{
                //    Vector2 c = NPCs.ArchaeaNPC.AngleBased(new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), r, distance);
                //    Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)c.X - 1, (int)c.Y - 1, 2, 2), Color.White * 0.50f);
                //}
                for (int i = 0; i < categories.Length; i++)
                {
                    sb.Draw(mod.Assets.Request<Texture2D>("Gores/config_icons").Value, mainOptions[i].bounds, new Rectangle(44 * i, 0, 44, 44), mainOptions[i].color);
                    if (mainOptions[i].HoverOver())
                    {
                        if (i == 0 && mainOptions[0].LeftClick())
                        {
                            flag = true;
                            classSelect = true;
                            break;
                        }
                        sb.DrawString(FontAssets.MouseText.Value, categories[i], new Vector2(mainOptions[i].bounds.X, mainOptions[i].bounds.Bottom), Color.White);
                        switch (i)
                        { 
                            case 0:
                                string text = "Class selection is set once per character.";
                                int width = (int)FontAssets.MouseText.Value.MeasureString(text).X;
                                sb.DrawString(FontAssets.MouseText.Value, text, new Vector2(Main.screenWidth / 2 - width / 2, Main.screenHeight - 32), Color.White);
                                break;
                            case 1:
                                string text2 = "Not implemented.";
                                int width2 = (int)FontAssets.MouseText.Value.MeasureString(text2).X;
                                sb.DrawString(FontAssets.MouseText.Value, text2, new Vector2(Main.screenWidth / 2 - width2 / 2, Main.screenHeight - 32), Color.White);
                                break;
                            case 2:
                                string text3 = "Damage and life scaling for all NPCs and Players. Set once per world!";
                                int width3 = (int)FontAssets.MouseText.Value.MeasureString(text3).X;
                                sb.DrawString(FontAssets.MouseText.Value, text3, new Vector2(Main.screenWidth / 2 - width3 / 2, Main.screenHeight - 32), Color.White);
                                break;
                        }
                    }
                }
                //currently without designation
                foreach (Element opt in mainOptions)
                {
                    if (opt.LeftClick() && opt.ticks++ == 0)
                    {
                        opt.active = !opt.active;
                        opt.color = opt.active ? Color.Blue : Color.White;
                        break;
                    }
                    opt.ticks = 0;
                }
                //first player joined code
                /*
                { 
                    mainOptions[1].active = ModeToggle.archaeaMode;
                    mainOptions[2].active = modWorld.cordonBounds;
                    mainOptions[1].color = mainOptions[1].active ? Color.Blue : Color.White;
                    mainOptions[2].color = mainOptions[2].active ? Color.Blue : Color.White;
                }
                */
                sb.Draw(mod.Assets.Request<Texture2D>("Gores/config_icons").Value, apply.bounds, new Rectangle(44 * 4, 0, 44, 44), apply.color = selected != null ? Color.White : Color.Gray);
                if (apply.HoverOver())
                    sb.DrawString(FontAssets.MouseText.Value, "Apply", new Vector2(apply.bounds.X, apply.bounds.Bottom), Color.White);
                if (apply.LeftClick() && apply.color != Color.Gray && back.ticks == 0)
                {
                    float useTime = player.HeldItem.useTime;
                    useTime += useTime % 100;
                    Timer timer = new Timer(Math.Max(useTime / Main.frameRate * 1000f, 1000f));
                    timer.AutoReset = false;
                    timer.Enabled = true;
                    timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                    {
                        modPlayer.classChoice = choice + 1;
                        timer.Dispose();
                    };
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetHandler.Send(Packet.SyncClass, 256, -1, player.whoAmI, choice + 1, player.GetModPlayer<ArchaeaPlayer>().playerUID);
                    //if (player == ArchaeaWorld.firstPlayer)
                    //{
                    ModContent.GetInstance<ModeToggle>().SetCordonedBiomes(mainOptions[1].active);
                    ModContent.GetInstance<ModeToggle>().SetArchaeaMode(mainOptions[2].active);
                    //}
                    if (Main.netMode != NetmodeID.Server)
                    {
                        player.GetModPlayer<ArchaeaPlayer>()
                            .SetModeStats(ModContent.GetInstance<ModeToggle>().archaeaMode, player.whoAmI);
                    }
                    Toggled = false;
                    return true;
                }
                back.ticks = 0;
            }
            return false;
        }
        public static void SetMainOption(Element opt, bool flag)
        {
            opt.active = flag;
            opt.color = opt.active ? Color.Blue : Color.White;
        }
        public static void ClassSelect(Player player)
        {
            float distance = 196f;
            //for (float r = 0f; r < distance; r += Draw.radians(distance))
            //{
            //    Vector2 c = NPCs.ArchaeaNPC.AngleBased(new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), r, distance);
            //    Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)c.X - 1, (int)c.Y - 1, 2, 2), Color.White * 0.50f);
            //}
            for (int i = 0; i < classes.Length; i++)
            {
                if (i != 4) sb.Draw(mod.Assets.Request<Texture2D>("Gores/class_icons").Value, classOptions[i].bounds, new Rectangle(44 * i, 0, 44, 44), classOptions[i].color);
                else        sb.Draw(mod.Assets.Request<Texture2D>("Gores/ClassIcon_UI").Value, classOptions[i].bounds, new Rectangle(44 * i, 0, 44, 44), classOptions[i].color);
                if (classOptions[i].HoverOver())
                {
                    if (classOptions[i].LeftClick())
                    {
                        selected = classOptions[i];
                        choiceName = classes[i];
                        choice = i;
                    }
                    sb.DrawString(FontAssets.MouseText.Value, classes[i], new Vector2(classOptions[i].bounds.X, classOptions[i].bounds.Bottom), Color.White);
                }
            }
            sb.Draw(mod.Assets.Request<Texture2D>("Gores/config_icons").Value, back.bounds, new Rectangle(44 * 3, 0, 44, 44), selected != null ? Color.White : Color.Gray);
            if (back.HoverOver())
                sb.DrawString(FontAssets.MouseText.Value, "Go Back", new Vector2(back.bounds.X, back.bounds.Bottom), Color.White);
            if (selected != null)
            {
                if (selected.LeftClick())
                {
                    foreach (Element opt in classOptions)
                    {
                        if (opt != null && opt != selected)
                        {
                            opt.active = false;
                            opt.color = Color.White;
                        }
                    }
                    selected.active = true;
                    selected.color = Color.Blue;
                }
            }
            if (selected != null && back.LeftClick() && back.ticks++ == 0)
            {
                classSelect = false;
                return;
            }
            back.ticks = 0;
        }
        public class Element
        {
            public Element(Rectangle bounds)
            {
                this.bounds = bounds;
            }
            public bool active;
            public Rectangle bounds;
            public Color color = Color.White;
            public int ticks;
            public Texture2D texture
            {
                get { return TextureAssets.MagicPixel.Value; }
            }
            private SpriteBatch sb
            {
                get { return Main.spriteBatch; }
            }
            public bool HoverOver()
            {
                return bounds.Contains(Main.MouseScreen.ToPoint());
            }
            public bool LeftClick()
            {
                Main.isMouseLeftConsumedByUI = true;
                return HoverOver() && ArchaeaPlayer.LeftClick();
            }
            public void Draw()
            {
                sb.Draw(texture, bounds, color);
            }
        }
    }
    public class TextBox
    {
        public bool active;
        public string text = "";
        public Color color
        {
            get { return active ? Color.DodgerBlue * 0.67f : Color.DodgerBlue * 0.33f; }
        }
        public Rectangle box;
        private KeyboardState oldState;
        private KeyboardState keyState
        {
            get { return Keyboard.GetState(); }
        }
        private SpriteBatch sb
        {
            get { return Main.spriteBatch; }
        }
        public TextBox(Rectangle box)
        {
            this.box = box;
        }
        public bool LeftClick()
        {
            return box.Contains(Main.MouseScreen.ToPoint()) && ArchaeaPlayer.LeftClick();
        }
        public bool HoverOver()
        {
            return box.Contains(Main.MouseScreen.ToPoint());
        }
        public void UpdateInput()
        {
            if (active)
            {
                foreach (Keys key in keyState.GetPressedKeys())
                {
                    if (oldState.IsKeyUp(key))
                    {
                        if (key == Keys.F3)
                            return;
                        if (key == Keys.Back)
                        {
                            if (text.Length > 0)
                                text = text.Remove(text.Length - 1);
                            oldState = keyState;
                            return;
                        }
                        else if (key == Keys.Space)
                            text += " ";
                        else if (key == Keys.OemPeriod)
                            text += ".";
                        else if (key == Keys.OemMinus)
                            text += "_";
                        else if (text.Length < 24 && key != Keys.OemPeriod)
                        {
                            string n = key.ToString().ToLower();
                            if (n.StartsWith("d") && n.Length == 2)
                                n = n.Substring(1);
                            if (n.Length == 1)
                                text += n;
                        }
                    }
                }
                oldState = keyState;
            }
        }
        public void DrawText()
        {
            sb.Draw(TextureAssets.MagicPixel.Value, box, color);
            sb.DrawString(FontAssets.MouseText.Value, text, new Vector2(box.X + 2, box.Y + 1), Color.White);
        }
    }
    public class Button
    {
        public string text = "";
        public Color color(bool select = true)
        {
            if (select)
                return box.Contains(Main.MouseScreen.ToPoint()) ? Color.DodgerBlue * 0.67f : Color.DodgerBlue * 0.33f;
            else
            {
                return box.Contains(Main.MouseScreen.ToPoint()) ? Color.White : Color.White * 0.67f;
            }
        }
        public Rectangle box;
        private SpriteBatch sb
        {
            get { return Main.spriteBatch; }
        }
        public Texture2D texture;
        public bool LeftClick()
        {
            return box.Contains(Main.MouseScreen.ToPoint()) && ArchaeaPlayer.LeftClick();
        }
        public Button(string text, Rectangle box, Texture2D texture = null)
        {
            this.texture = texture;
            if (texture == null && !Main.dedServ)
                this.texture = TextureAssets.MagicPixel.Value;
            this.text = text;
            this.box = box;
        }
        public void Draw(bool select = true)
        {
            sb.Draw(texture, box, color(select));
            sb.DrawString(FontAssets.MouseText.Value, text, new Vector2(box.X + 2, box.Y + 2), Color.White * 0.90f);
        }
    }
}