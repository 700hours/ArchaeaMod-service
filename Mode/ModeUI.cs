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
        private ListBox[] trait = new ListBox[5];
        private ListBox[] page;
        private Button[] tab;
        private Button close;
        public static Color innactiveColor = Color.Gray;
        public static Color activeColor = Color.LightBlue;
        public static Color bgColor = default;
        public Color textColor = default;
        private int classChoice = 0;
        public int ticks = 300;
        public int ticks2 = 300;
        public string text;

        public override void OnModLoad()
        {
            if (Main.dedServ) return;
            //start = 0, health = 1, mana = 2, bosses = 3, bottom = 4, npcs = 5, week = 6, crafting = 7, downedMagno = 8;
            objective = new ListBox(new Rectangle(200, 200, 240, 200), default, new[] 
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
            objective.bgColor = bgColor * 0.5f;
            
            stat = new ListBox(objective.hitbox, default, new Button[]
            {
                new Button("", default, Color.White) { text2 = "Arrow speed", innactiveDrawText = true },         // ranged
                new Button("", default, Color.White) { text2 = "Jump Height", innactiveDrawText = true },         // summoner
                new Button("", default, Color.White) { text2 = "Attack speed", innactiveDrawText = true },        // melee
                new Button("", default, Color.White) { text2 = "Move speed",  innactiveDrawText = true },         // summoner
                new Button("", default, Color.White) { text2 = "Underwater breath", innactiveDrawText = true },   // all
                new Button("", default, Color.White) { text2 = "Toughness", innactiveDrawText = true },           // melee
                new Button("", default, Color.White) { text2 = "Item damage", innactiveDrawText = true },         // mage
                new Button("", default, Color.White) { text2 = "Price discount", innactiveDrawText = true },      // all
                new Button("", default, Color.White) { text2 = "% damage reduction", innactiveDrawText = true },  // mage
                new Button("", default, Color.White) { text2 = "Ammo use reduction", innactiveDrawText = true }   // ranged
            });
            for (int i = 0; i < stat.item.Length; i++) {
                stat.item[i].texture = TextureAssets.MagicPixel.Value;
            }
            stat.bgColor = bgColor * 0.5f;
            stat.scroll = new Scroll(objective.hitbox);
            stat.active = false;

            var mode = new ListBox(objective.hitbox, default, new[]
            {
                "Optional: Archaea Mode",
                "enables objectives",
                "that scale difficulty."
            }, null, new [] { Color.Gray });
            mode.scroll = new Scroll(mode.hitbox);
            mode.bgColor = bgColor * 0.5f;

            trait[ClassID.Melee - 1] = new ListBox(objective.hitbox, default,   ClassArray(ClassID.Melee - 1, 0), null, new Color[5]);
            trait[ClassID.Ranged - 1] = new ListBox(objective.hitbox, default,  ClassArray(ClassID.Ranged - 1, 0), null, new Color[5]);
            trait[ClassID.Magic - 1] = new ListBox(objective.hitbox, default,   ClassArray(ClassID.Magic - 1, 0), null, new Color[5]);
            trait[ClassID.Summoner -1] = new ListBox(objective.hitbox, default, ClassArray(ClassID.Summoner - 1, 0), null, new Color[5]);;
            trait[ClassID.All - 1] = new ListBox(objective.hitbox, default,     ClassArray(ClassID.All - 1, 0), null, new Color[5]);
            for (int i = 0; i < trait.Length; i++)
            {
                trait[i].bgColor = Color.Transparent;
                trait[i].scroll = new Scroll(objective.hitbox);
                trait[i].active = false;
                trait[i].textColor = new Color[] { innactiveColor, innactiveColor, innactiveColor, innactiveColor, innactiveColor, innactiveColor };
            }
            tab = new Button[]
            {
                new Button("1", default, Color.LightGray),
                new Button("2", default, Color.LightGray),
                new Button("3", default, Color.LightGray)
            };
            page = new ListBox[] { objective, stat, default, mode };
        }
        private string[] ClassArray(int index, int num = 0, int num2 = 0)
        {
            switch (index) 
            {
                case 0:     // Melee
                    return new[] 
                    { 
                        "Defeat each invasion once\n" +
                        "   Double swing",
                        $"Place {Math.Min(num, 250)}/250 solid tiles\n" +
                        "   Double knockback",
                        "Defeat a Nymph\n" +
                        "   Stun NPCs on hit",
                        "Reach maximum mana\n" +
                        "   50% Cinnabar flask",
                        "Craft wings\n" +
                        "   Leap attack",
                        $"Survive a {num2}/1000 tile fall\n" +
                        "   Leap"
                    };
                case 1:     // Ranged
                    return new[]
                    {
                        "Make a Star Cannon\n" +
                        "   Double fire arrows",
                        "Defeat Old One's Army\n" +
                        "   Subtle arrow tracking",
                        "Reach the underworld\n" +
                        "   Fire arrows",
                        $"Hold max dirt stack for {Math.Min(num / 60 / 60 / 60, 20)}/{20} minutes\n" +
                        "   Ice arrows",
                        "Defeat all Mech bosses\n" +
                        "   Ichor arrows",
                        "Acquire Daedalus\n" +
                        "   Cinnabar arrows"
                    };
                case 2:     // Magic
                    return new[] 
                    {
                        "Make Ankh Shield\n" +
                        "   50% double attack chance",
                        $"Plant {num}/80 o.w. mushroom seeds\n" +
                        "   50% no mana cost chance",
                        $"Place {num2}/1000 rails\n" +
                        "   10% damage reduction",
                        "Craft a hook\n" +
                        "   20% increased move speed",
                        "Reach the ocean\n" +
                        "   50% no knockback chance",
                        "Reach fallen meteor\n" +
                        "   20% reduced mana cost"
                    };
                case 3:     // Summoner
                    return new[] 
                    {
                        "Acquire Giant Harpy Feather\n" +
                        "   +2 minion count",
                        "Reach maximum life\n" +
                        "   +20% minion damage",
                        $"Place {num}/500 of any bricks\n" +
                        "   No mana cost",
                        "Last-hit a boss with cannonball\n" +
                        "   1% chance attack throws boulder",
                        "Find a sword shrine\n" +
                        "   1% chance attack throws bones",
                        "Reach space\n" +
                        "   1% chance attack throws star"
                    };
                case 4:     // All
                    return new[]
                    {
                        "Craft Avenger Emblem\n" +
                        "   Wall jump",
                        "Place two different pylons\n" +
                        "   Extra double jump",
                        "Craft Molten Pickaxe\n" +
                        "   +10 defense",
                        "Get max Well Fed buff\n" +
                        "   +25% jump height",
                        "Last-hit Eye of Cthulhu\n" +
                        "   Dash",
                        $"Get {num}/10 villagers\n" +
                        "   50% chance falling star"
                    };
                default:
                    return new[] { "" };
            }
        }

        private int TraitIndex()
        {
            return (int)Math.Max(Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().classChoice - 1, 0);
        }
        private Rectangle AdjustY(Rectangle box, int offY)
        {
            return new Rectangle(box.X, box.Y + offY, box.Width, box.Height);
        }
        public override void PostDrawInterface(SpriteBatch sb)
        {
            if (ModContent.GetInstance<ModeToggle>().loading)
            {
                Utils.DrawBorderString(sb, "Loading checklist . . .", new Vector2(objective.hitbox.X, objective.hitbox.Top - 24), Color.CornflowerBlue);
                return;
            }
            for (int i = 0; i < page.Length - 1; i++)
            {
                if (page[0].active && !ModContent.GetInstance<ModeToggle>().archaeaMode)
                {
                    page[3].Draw(sb, FontAssets.MouseText.Value, 8, 8, 32);
                    break;
                }
                else if (page[0].active)
                {
                    page[0].Draw(sb, FontAssets.MouseText.Value, 8, 8, 32);
                    page[0].scroll.Draw(sb, Color.White);
                }
                if (page[1].active)
                {
                    page[1].DrawItems(sb, FontAssets.MouseText.Value, 8, 8, 32);
                    page[1].scroll.Draw(sb, Color.White);
                }
                if (trait[TraitIndex()].active)
                {
                    trait[TraitIndex()].Draw(sb, FontAssets.ItemStack.Value, 8, 8, 48);
                    trait[TraitIndex()].scroll.Draw(sb, Color.White);
                }
            }
            int offY = 8;
            page[2] = trait[TraitIndex()];
            if (tab[0].active)
            {
                Utils.DrawInvBG(sb, new Rectangle(tab[0].box.X - 8, tab[0].box.Y, tab[0].box.Width + 16, tab[0].box.Height * 3 + 36));
                tab[0].HoverPlaySound(SoundID.MenuTick);
                if (TextureAssets.Item[ItemID.Book].Value.Name.Contains("Dummy")) { 
                    int t =Item.NewItem(Item.GetSource_None(), Rectangle.Empty, ItemID.Book);
                    if (Main.netMode != 0) 
                    { 
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, t);
                    }
                }
                sb.Draw(TextureAssets.Item[ItemID.Book].Value, AdjustY(tab[0].box, offY), page[0].active ? Color.White : Color.White * 0.5f);
            }
            if (tab[1].active)
            {
                tab[1].HoverPlaySound(SoundID.MenuTick);
                if (TextureAssets.Item[ItemID.Book].Value.Name.Contains("Dummy")) { 
                    int t = Item.NewItem(Item.GetSource_None(), Rectangle.Empty, ItemID.IronBroadsword);
                    if (Main.netMode != 0) 
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, t);
                    }
                }
                sb.Draw(TextureAssets.Item[ItemID.IronBroadsword].Value, AdjustY(tab[1].box, offY), page[1].active ? Color.White : Color.White * 0.5f);
            }
            if (tab[2].active)
            {
                tab[2].HoverPlaySound(SoundID.MenuTick);
                if (TextureAssets.Item[ItemID.AvengerEmblem].Value.Name.Contains("Dummy"))
                {
                    int t = Item.NewItem(Item.GetSource_None(), Rectangle.Empty, ItemID.AvengerEmblem);
                    if (Main.netMode != 0)
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, t);
                    }
                }
                sb.Draw(TextureAssets.Item[ItemID.AvengerEmblem].Value, AdjustY(tab[2].box, offY), page[2].active ? Color.White : Color.White * 0.5f);
            }
                                         
            if (page[0].active)
            {
                Utils.DrawBorderString(sb, "Progress checklist", new Vector2(objective.hitbox.X, objective.hitbox.Top - 24), Color.CornflowerBlue);
            }
            if (page[1].active)
            {
                Utils.DrawBorderString(sb, "Player stat bonuses", new Vector2(objective.hitbox.X, objective.hitbox.Top - 24), Color.CornflowerBlue);
                if (Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().remainingStat >= 0) 
                {
                    Utils.DrawBorderString(sb, $"Stat points: {Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().remainingStat} / {Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().overallMaxStat}", new Vector2(objective.hitbox.X, objective.hitbox.Bottom), Color.MediumPurple);
                    Utils.DrawBorderString(sb, "Click items to assign points", new Vector2(objective.hitbox.X, objective.hitbox.Bottom + 24), Color.Gray);
                }
            }
            if (page[2].active)
            {
                Utils.DrawBorderString(sb, $"{ClassName()} class traits", new Vector2(objective.hitbox.X, objective.hitbox.Top - 24), Color.CornflowerBlue);
            }
            DrawTextUI(sb, 0, "Trait acquired!", ref ticks, color: Color.CornflowerBlue);
            DrawTextUI(sb, 0, text, ref ticks2, color: textColor);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            #region active states
            if (ArchaeaMain.progressKey.JustPressed)
            {
                if (!stat.active && !trait[TraitIndex()].active) 
                { 
                    objective.active = !objective.active;
                }
                else stat.active = false;
                if (!objective.active) 
                {
                    stat.active = false;
                }
                if (trait[TraitIndex()].active)
                {
                    objective.active = false;
                    trait[TraitIndex()].active = false;
                }
            }
            tab[0].active = objective.active || stat.active || trait[TraitIndex()].active;
            tab[1].active = objective.active || stat.active || trait[TraitIndex()].active;
            tab[2].active = objective.active || stat.active || trait[TraitIndex()].active;
            if (tab[0].LeftClick()) 
            {
                objective.active = true;
                stat.active = false;
                trait[TraitIndex()].active = false;
            }
            if (tab[1].LeftClick()) 
            {
                objective.active = false;
                stat.active = true;
                trait[TraitIndex()].active = false;
            }
            if (tab[2].LeftClick())
            {
                objective.active = false;
                stat.active = false;
                trait[TraitIndex()].active = true;
            }
            if (objective.active) 
            { 
                stat.hitbox = objective.hitbox;
                trait[TraitIndex()].hitbox = objective.hitbox;
            }
            else if (stat.active)
            {
                objective.hitbox = stat.hitbox;
                trait[TraitIndex()].hitbox = stat.hitbox;
            }
            else if (trait[TraitIndex()].active)
            {
                stat.hitbox = trait[TraitIndex()].hitbox;
                objective.hitbox = trait[TraitIndex()].hitbox;
            }
            #endregion
            int xOffset = 48;
            int yOffset = 42;
            tab[0].box = new Rectangle(objective.hitbox.X - xOffset, objective.hitbox.Top, 32, 32);
            tab[1].box = new Rectangle(objective.hitbox.X - xOffset, objective.hitbox.Top + yOffset, 32, 32);
            tab[2].box = new Rectangle(objective.hitbox.X - xOffset, objective.hitbox.Top + yOffset * 2, 32, 32);

            for (int i = 0; i < page.Length; i++)
            {
                if (page[i] == null) continue;
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
                            page[i].item[n].active = true;
                            switch (Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().classChoice)
                            {
                                case ClassID.None:
                                    //classChoice = Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().classChoice;
                                    break;
                                case ClassID.Melee:
                                    if (n == 2 || n == 5)
                                        break;
                                    goto default;
                                case ClassID.Ranged:
                                    if (n == 0 || n == 9)
                                        break;
                                    goto default;
                                case ClassID.Magic:
                                    if (n == 6 || n == 8)
                                        break;
                                    goto default;
                                case ClassID.Summoner:
                                    if (n == 1 || n == 3)
                                        break;
                                    goto default;
                                case ClassID.All:
                                    if (n == 4 || n == 7)
                                        break;
                                    goto default;
                                default:
                                    page[i].item[n].active = false;
                                    break;
                            }
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
                    case 2:
                        for (int n = 0; n < 6; n++)
                        {
                            trait[TraitIndex()].textColor[n] = 
                                    ArchaeaPlayer.CheckHasTrait(n + TraitIndex() * 6, TraitIndex() + 1, Main.LocalPlayer.whoAmI)
                                    ? activeColor : innactiveColor;
                        }
                        break;
                }
            }
            ArchaeaPlayer modPlayer = Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>();
            switch (TraitIndex())
            {
                case 0:
                    trait[0].content = ClassArray(0, modPlayer.placedTiles, modPlayer.comparison);
                    break;
                case 1:
                    trait[1].content = ClassArray(1, modPlayer.TRAIT_TIME_MaxDirtStack);
                    break;
                case 2:
                    trait[2].content = ClassArray(2, modPlayer.TRAIT_PlantedMushroom, modPlayer.TRAIT_PlacedRails);
                    break;
                case 3:
                    trait[3].content = ClassArray(3, modPlayer.TRAIT_PlacedBricks);
                    break;
                case 4:
                    trait[4].content = ClassArray(4, Main.townNPCCanSpawn.Count(t => t == true));
                    break;
            }
            InBoundsUI(objective);
            InBoundsUI(stat);
            InBoundsUI(trait[TraitIndex()]);
            InBoundsUI(page[3]);
        }
        public static void DrawTextUI(SpriteBatch sb, int y, string text, ref int ticks, int maxTicks = 300, Color color = default)
        {
            if (text == null)
                return;
            if (ticks <= maxTicks)
            { 
                ticks++;
                string placeholder = "Trait acquired!";
                float width = 140 * text.Length / placeholder.Length;
                int cOffset = Main.player[Main.myPlayer].height * 2 + y;
                int xOffset = 8;
                int yOffset = 8;
                var rect = UiHelper.CenterBox(Main.screenWidth, Main.screenHeight + cOffset, (int)width, 36);
                if (Main.mouseLeft && rect.Contains(Main.MouseScreen.ToPoint()))
                {
                    ticks = maxTicks;
                }
                Utils.DrawInvBG(sb, rect, Color.Transparent);
                Utils.DrawBorderString(sb, text, new Vector2(rect.X + xOffset, rect.Y + yOffset), Color.CornflowerBlue);
            }
        }
        public static void NewText(string text, Color color)
        {
            ModContent.GetInstance<ModeUI>().text = text;
            ModContent.GetInstance<ModeUI>().ticks2 = 0;
            ModContent.GetInstance<ModeUI>().textColor = color;
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
        private string ClassName()
        {
            switch (TraitIndex() + 1)
            { 
                case ClassID.Melee:     return "Melee";
                case ClassID.Ranged:    return "Ranged";
                case ClassID.Magic:     return "Magic";
                case ClassID.Summoner:  return "Summoner";
                default:                return "All";
            }
        }
    }
}
