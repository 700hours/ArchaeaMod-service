using ArchaeaMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ModLoader.Default;
using ArchaeaMod.Interface.UI;
using tUserInterface.ModUI;
using Button = tUserInterface.ModUI.Button;
using TextBox = tUserInterface.ModUI.TextBox;
using ArchaeaMod.Jobs.Buffs;
using ArchaeaMod.Jobs.Global;
using static Humanizer.On;
using System.Diagnostics.Metrics;

namespace ArchaeaMod.Jobs
{
    internal class DialogUI : ModSystem
    {
        bool showedDialog
        { 
            get { return Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().showedDialog; }
            set { Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().showedDialog = value; }
        }
        int num3 = 0;
        string noticeText = // Legacy to display amount of text
            "There are some unloaded items. Please manage\n" +
            "these items, either acquired from MP servers\n" +
            "or otherwise, or enable any missing mods before\n" +
            "using AnotherSSC. The SP data will reload on\n" +
            "world join upon completion.";
        Button close => new Button("Close", default, Color.Blue) { drawMagicPixel = true };                                                     //200 (legacy value)
        TextBox unloadedNotice => new TextBox(new Rectangle(Main.screenWidth / 2 - 400 / 2 + 120, (int)(Main.screenHeight / 2.5f) - 200 / 2, 400, 320), Color.Blue);
        Rectangle listRect => new Rectangle(Main.screenWidth / 2 - 400 / 2 - 120, (int)(Main.screenHeight / 2.5f) - 200 / 2 - 8, 190, 320 - 24);
        Scroll listScroll => new Scroll(listRect);
        ListBox dialog => new ListBox(listRect, listScroll, default(string[]));
        SpriteBatch sb => Main.spriteBatch;
        public static readonly string JobText =
            "Jobs can be selected per class. There are three\n" +
            "jobs per class, and they are not locked. Each\n" +
            "class can be empowered by using a certain class-\n" +
            "typeset of items. Once the jobs are maxed out,\n" +
            "then a class specific attribute is gained. The\n" +
            "attribute consists of stat points and a small\n" +
            "buff. This can be displatyed in the class mini-\n" +
            "quest screen.";
        public static readonly string[] Content = new string[]
        {
            "Melee: Rusty Plate",
            "   smith",
            "   warrior",
            "   white knight",
            "Ranged: Black Lens",
            "   bowsman",
            "   corperate usurper",
            "   outlaw",
            "Mage: Magno Core",
            "   botanist",
            "   witch",
            "   wizard",
            "Summoner: Bird Statue",
            "   alchemist",
            "   scientist",
            "   surveyor",
            "All: Gold Coin",
            "   business man",
            "   entrepreneur",
            "   merchant"
        };
        public void PrintDualListText(string text, string[] content)
        {
            if (!unloadedNotice.active || showedDialog)
                return;
            if (num3 == 0)
            {
                dialog.content = content;
            }
            string caret = "|";
            string notice = text; //\nMissing mod: {ModName}.";
            if (num3 < notice.Length)
            {
                if (num3 % 3 == 0)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
                num3++;
            }
            else caret = "";
            close.active = true;
            close.box = new Rectangle(unloadedNotice.box.Right - 60, unloadedNotice.box.Bottom - 40 - 32, 50, 30);
            unloadedNotice.text = notice.Substring(0, num3) + caret;
            close.HoverPlaySound(SoundID.MenuTick);
            if (close.LeftClick())
            {
                dialog.active = false;
                close.active = false;
                unloadedNotice.active = false;
                showedDialog = true;
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            PrintDualListText(JobText, Content);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (!unloadedNotice.active || showedDialog)
                return;
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            bool hover = close.HoverOver();
            int offset = 8;
            var bounds = unloadedNotice.box;
            Utils.DrawInvBG(Main.spriteBatch, new Rectangle(bounds.X - offset, bounds.Y - offset, bounds.Width + offset * 2, bounds.Height - 40 + offset * 2));
            //sb.Draw(TextureAssets.MagicPixel.Value, box, Color.Gray);
            dialog.bgColor = default;
            if (dialog.scroll != null)
            {
                dialog.Draw(Main.spriteBatch, FontAssets.MouseText.Value, 8, 8, 32);
            }
            listScroll.Draw(Main.spriteBatch, Color.Gray);
            unloadedNotice.DrawText(null);
            close.Draw(hover);
            sb.End();
        }
    }
}
