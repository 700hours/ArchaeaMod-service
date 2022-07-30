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
using ArchaeaMod.Progression;
using Terraria.Audio;
using Terraria.ID;
using tUserInterface.ModUI;

namespace ArchaeaMod.Mode
{
    public static class Extension
    {
        public static void UpdateText(this Button button, object value)
        {
            button.text = $"{value} {button.text2}";
        }
    }
    public class ModeUI : ModSystem
    {
        private ListBox objective;
        private ListBox stat;
        private ListBox[] page;
        private Button[] tab;
        private int tick;

        public override void OnModLoad()
        {
            if (Main.dedServ) return;
            //start = 0, health = 1, mana = 2, bosses = 3, bottom = 4, npcs = 5, week = 6, crafting = 7, downedMagno = 8;
            objective = new ListBox(new Rectangle(200, 200, 240, 300), default, new[] 
            { 
                "World start begins",
                "Heart crystal used",
                "Mana crystal used",
                "Boss downed",
                "Dug too deep",
                "Townsfolk have arrived",
                "Six days passed",
                "Crafting station made",
                "Magnoliac's bane"
            }, null, ArchaeaMode.unlock);
            objective.scroll = new Scroll(objective.hitbox);
            objective.bgColor = Color.Transparent;
            
            stat = new ListBox(objective.hitbox, default, new Button[]
            {
                new Button("", default, Color.White) { text2 = "Arrow speed" },
                new Button("", default, Color.White) { text2 = "Jump Height" },
                new Button("", default, Color.White) { text2 = "Attack speed" },
                new Button("", default, Color.White) { text2 = "Move speed" },
                new Button("", default, Color.White) { text2 = "Underwater breath" },
                new Button("", default, Color.White) { text2 = "Toughness" },
                new Button("", default, Color.White) { text2 = "Item damage" },
                new Button("", default, Color.White) { text2 = "Price discount" },
                new Button("", default, Color.White) { text2 = "% damage reduction" },
                new Button("", default, Color.White) { text2 = "Ammo use reduction" }
            });
            for (int i = 0; i < stat.item.Length; i++) {
                stat.item[i].texture = TextureAssets.MagicPixel.Value;
            }
            stat.bgColor = Color.Transparent;
            stat.scroll = new Scroll(objective.hitbox);
            stat.active = false;
            tab = new Button[]
            {
                new Button("1", default, Color.LightGray),
                new Button("2", default, Color.LightGray),
            };
            page = new ListBox[] { objective, stat };
        }

        public override void PostDrawInterface(SpriteBatch sb)
        {
            for (int i = 0; i < page.Length; i++)
            { 
                if (page[0].active)
                {
                    page[0].Draw(sb, FontAssets.MouseText.Value, 8, 8, 32);
                    page[0].scroll.Draw(sb, Color.White);
                }
                if (page[1].active)
                {
                    page[1].DrawItems(sb, FontAssets.MouseText.Value, 8, 8, 32);
                    page[1].scroll.Draw(sb, Color.White);
                }
                if (tab[i].active)
                { 
                    sb.Draw(TextureAssets.MagicPixel.Value, tab[i].box, Color.Gray);
                    tab[i].Draw(FontAssets.MouseText.Value);
                }
            }
            if (page[0].active)
            {
                Utils.DrawBorderString(sb, "Progress checklist", new Vector2(objective.hitbox.X, objective.hitbox.Top - 32), Color.CornflowerBlue);
            }
            if (page[1].active)
            {
                Utils.DrawBorderString(sb, "Player stat bonuses", new Vector2(objective.hitbox.X, objective.hitbox.Top - 24), Color.CornflowerBlue);
                if (Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().remainingStat > 0) {
                    Utils.DrawBorderString(sb, $"Stat points: {Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().remainingStat} / {Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().overallMaxStat}", new Vector2(objective.hitbox.X, objective.hitbox.Bottom), Color.MediumPurple);
                    Utils.DrawBorderString(sb, "Click items to assign points", new Vector2(objective.hitbox.X, objective.hitbox.Bottom + 24), Color.Gray);
                }
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            #region active states
            if (ArchaeaMain.progressKey.JustPressed)
            {
                objective.active = !objective.active;
                if (!objective.active) {
                    stat.active = false;
                }
            }
            tab[0].active = objective.active || stat.active;
            tab[1].active = objective.active || stat.active;
            if (tab[0].LeftClick()) {
                stat.active = false;
                objective.active = true;
            }
            if (tab[1].LeftClick()) {
                objective.active = false;
                stat.active = true;
            }
            #endregion
            if (objective.active)
                stat.hitbox = objective.hitbox;
            else objective.hitbox = stat.hitbox;
            tab[0].box = new Rectangle(objective.hitbox.X - 32, objective.hitbox.Top, 16, 24);
            tab[1].box = new Rectangle(objective.hitbox.X - 32, objective.hitbox.Top + 42, 16, 24);

            for (int i = 0; i < page.Length; i++)
            {
                page[i].Update(true);
                switch (i)
                { 
                    case 0:
                        page[i].textColor = ArchaeaMode.unlock;
                        break;
                    case 1:
                        if (!page[i].active)
                            break;
                        for (int n = 0; n < page[i].item.Length; n++)
                        {
                            if (page[i].scroll.clicked)
                                break;
                            page[i].item[n].HoverPlaySound(SoundID.MenuTick);
                            if (page[i].item[n].LeftClick())
                            {
                                if (page[i].item[n].reserved == 0)
                                { 
                                    if (Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().SpendStatPoint(n)) {
                                        SoundEngine.PlaySound(SoundID.Item4, Main.LocalPlayer.Center);
                                    }
                                    page[i].item[n].reserved = 1;
                                    break;
                                }
                            }
                            else page[i].item[n].reserved = 0;
                            page[i].item[n].UpdateText($"[{Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().spentStat[n]}] ");
                        }
                        break;
                }
                InBoundsUI(objective);
                InBoundsUI(stat);
            }
        }
        private void InBoundsUI(ListBox listBox)
        {
            //  Screen bounds
            if (listBox.hitbox.X <= 0)
                listBox.hitbox.X = 0;
            if (listBox.hitbox.X + listBox.hitbox.Width >= Main.screenWidth)
                listBox.hitbox.X = Main.screenWidth - listBox.hitbox.Width;
            if (listBox.hitbox.Y <= 0)
                listBox.hitbox.Y = 0;
            if (listBox.hitbox.Y + listBox.hitbox.Height >= Main.screenHeight)
                listBox.hitbox.Y = Main.screenHeight - listBox.hitbox.Height;
        }
    }
}
