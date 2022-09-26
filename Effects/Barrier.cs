using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Effects
{
    public class Barrier : ModBiome
    {
        //  Copied from MagnoBiome
        private bool swapTracks;
        private bool triggerSwap;
        public override string MapBackground => "Backgrounds/MapBGMagno";
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetModUndergroundBackgroundStyle(Backgrounds.bg_style.Style);
        public override int Music => UpdateMusic();
        public override bool IsBiomeActive(Player player)
        {
            return active;
        }
        public int UpdateMusic()
        {
            if ((int)Main.time == (int)Main.dayLength / 2)
                triggerSwap = true;
            if (triggerSwap)
            {
                swapTracks = !swapTracks;
                triggerSwap = false;
            }
            return swapTracks ? MusicLoader.GetMusicSlot(this.Mod, "Sounds/Music/Dark_and_Evil_with_a_hint_of_Magma") : MusicLoader.GetMusicSlot(this.Mod, "Sounds/Music/Magno_Biome");
        }

        //  Barrier handling
        private Barrier(int x, int y)
        {
            position = new Vector2(x, y);
            hitbox = new Rectangle(x, y, Size, Size);
        }
        public bool active;
        public static bool hintInit = false;
        private float alpha = 0f;
        public static readonly int Size = 96;
        public static readonly int Width = 96;
        public static readonly int Height = 96;
        public Vector2 position;
        public Vector2 Center => new Vector2(position.X + Width / 2, position.Y + Height / 2);
        public Rectangle hitbox;
        public Texture2D tex => TextureAssets.MagicPixel.Value;
        public static Barrier[] barrier;
        private static Mod mod
        {
            get { return ModLoader.GetMod("ArchaeaMod"); }
        }
        public static void Initialize()
        {
            int length = (int)Main.rightWorld / Width;
            int y = Main.UnderworldLayer * 16;
            barrier = new Barrier[length];
            for (int i = 0; i < length; i++)
            {
                barrier[i] = new Barrier(i * Size, y);
            }
        }
        public void Update(Player player)
        {
            int originX = ModContent.GetInstance<ArchaeaWorld>().MagnoBiomeOriginX;
            if (originX == 0)
                return;
            if (!active || ModContent.GetInstance<ArchaeaWorld>().downedMagno)
                return;
            if (player.position.Y + player.height >= position.Y) 
            {
                player.velocity.Y = 0f;
                player.slowFall = true;
            }
            if (player.position.Y + player.height > position.Y) 
            {
                player.position.Y = position.Y - player.height;
                player.slowFall = true;
            }
            if (!Barrier.hintInit && player.position.Y > position.Y - Main.screenHeight / 2)
            {
                int direction = originX > player.position.X ? -1 : 1;
                SoundEngine.PlaySound(SoundID.Roar, new Vector2(player.position.X - Main.screenWidth / 2 * direction, player.Center.Y));
                Main.NewText("Sounds resound from the direction of the magnoliac region...");
                hintInit = true;
            }
        }
        public void Draw(SpriteBatch sb, Player player)
        {
            if (ModContent.GetInstance<ArchaeaWorld>().MagnoBiomeOriginX == 0)
                return;
            if (ModContent.GetInstance<ArchaeaWorld>().downedMagno)
                return;
            float distance = Vector2.Distance(player.Center, Center) / Main.screenHeight;
            if (distance < 1f)
            {
                alpha = Math.Min(Math.Abs(distance - 1f), 1f);
                Color color = Color.FromNonPremultiplied((int)(255 * 0.604f), (int)(255 * 0.161f), (int)(255 * 0.161f), (int)(255 * alpha));
                sb.Draw(tex, position - Main.screenPosition, new Rectangle(0, 0, Width, Height), color);
                active = true;
            }
            else active = false;
        }
        public static void DrawHint(SpriteBatch sb, Player player)
        {
            if (ModContent.GetInstance<ArchaeaWorld>().MagnoBiomeOriginX == 0)
                return;
            if (player.position.X > ModContent.GetInstance<ArchaeaWorld>().MagnoBiomeOriginX * 16 &&
                player.position.X < (ModContent.GetInstance<ArchaeaWorld>().MagnoBiomeOriginX + 1600) * 16)
                return;
            if (ModContent.GetInstance<ArchaeaWorld>().downedMagno || player.GetModPlayer<ArchaeaPlayer>().MagnoBiome)
                return;
            var b = barrier.FirstOrDefault(t => t.active);
            if (b != default && b.active)
            {
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                Color color = Color.FromNonPremultiplied((int)(255 * 0.604f), (int)(255 * 0.161f), (int)(255 * 0.161f), 255);
                if (ModContent.GetInstance<ArchaeaWorld>().MagnoBiomeOriginX < player.Center.X) { 
                    sb.Draw(mod.Assets.Request<Texture2D>("Gores/arrow").Value, new Vector2(50, Main.screenHeight / 2 - Height / 2), default, color, 0f, new Vector2(Width / 2, Height / 2), 0.5f, SpriteEffects.FlipHorizontally, 0f);
                }
                else {
                    sb.Draw(mod.Assets.Request<Texture2D>("Gores/arrow").Value, new Vector2(Main.screenWidth - Width - 50, Main.screenHeight / 2 - Height / 2), default, color, 0f, new Vector2(Width / 2, Height / 2), 0.5f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
