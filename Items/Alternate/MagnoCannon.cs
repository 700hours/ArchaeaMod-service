using System;
using System.Collections.Generic;
using PointF = System.Drawing.PointF;
using Bitmap = System.Drawing.Bitmap;
using Graphics = System.Drawing.Graphics;
using System.IO;
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
        public override string Texture => "ArchaeaMod/Gores/Null";
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //tooltips.Add(new TooltipLine(Mod, "ItemName", "Magno Cannon"));
            //tooltips.Add(new TooltipLine(Mod, "Tooltip0", ""));
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
        public static Texture2D tex;
        public static float angle;
        public static int height = 100;
        SpriteBatch sb => Main.spriteBatch;
        public override bool? UseItem(Player player)
        {                      
            return false;
            angle = NPCs.ArchaeaNPC.AngleTo(player.Center, Main.MouseWorld);
            float distance = Vector2.Distance(player.Center, Main.MouseWorld);
            var point = Fx.GenerateImage(height, (int)distance, true, System.Drawing.Color.Black);
            for (int i = 0; i < point.Length; i++)
            {
                point[i].X = (float)(player.Center.X + point[i].X + i * Math.Cos(angle));
                point[i].Y = (float)(player.Center.Y + point[i].Y + i * Math.Sin(angle));
            }
            var fx = Fx.GenerateImage(FxID.WaveForm, (int)distance, height, System.Drawing.Brushes.Black, System.Drawing.Color.Black, point);
            tex = Texture2D.FromStream(Main.graphics.GraphicsDevice, fx);

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (tex != null)
            {
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                sb.Draw(tex, player.Center - Main.screenPosition + zero, null, Color.White, (float)angle, default(Vector2), 1f, SpriteEffects.None, 0f);
            }
            sb.End();
            fx.Dispose();
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
