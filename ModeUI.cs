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
using tUserInterface.ModUI;

namespace ArchaeaMod
{
    public class ModeUI : ModSystem
    {
        private ListBox objective;
        public override void OnModLoad()
        {
            //start = 0, health = 1, mana = 2, bosses = 3, bottom = 4, npcs = 5, week = 6, crafting = 7, downedMagno = 8;
            objective = new ListBox(new Rectangle(200, 200, 200, 250), default, new[] 
            { 
                "World start begins",
                "Heart crystal used",
                "Mana crystal used",
                "N/a",
                "Dug too deep",
                "Townsfolk have arrived",
                "Six weeks passed",
                "Crafting station made",
                "Magnoliac's bane"
            }, null, ModeToggle.unlock);
            objective.scroll = new Scroll(objective.hitbox);
            objective.bgColor = Color.Transparent;
        }

        public override void PostDrawInterface(SpriteBatch sb)
        {
            if (objective.active)
            { 
                objective.Draw(sb, FontAssets.MouseText.Value, 8, 8, 32);
                objective.scroll.Draw(sb, Color.White);
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if (ArchaeaMain.progressKey.JustPressed)
            { 
                objective.active = !objective.active;
            }
            objective.textColor = ModeToggle.unlock;
            objective.Update(true);
            //  Screen bounds
            if (objective.hitbox.X <= 0)
                objective.hitbox.X = 0;
            if (objective.hitbox.X + objective.hitbox.Width >= Main.screenWidth)
                objective.hitbox.X = Main.screenWidth - objective.hitbox.Width;
            if (objective.hitbox.Y <= 0)
                objective.hitbox.Y = 0;
            if (objective.hitbox.Y + objective.hitbox.Height >= Main.screenHeight)
                objective.hitbox.Y = Main.screenHeight - objective.hitbox.Height;
        }
    }
}
