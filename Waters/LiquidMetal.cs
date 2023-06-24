using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

using ArchaeaMod.GenLegacy;
using ArchaeaMod.Items;
using ArchaeaMod.Items.Alternate;
using ArchaeaMod.Merged;
using ArchaeaMod.Merged.Items;
using ArchaeaMod.Merged.Tiles;
using ArchaeaMod.Merged.Walls;
using Humanizer;
using ArchaeaMod.NPCs.Bosses;
using System.Runtime.CompilerServices;

namespace ArchaeaMod.Waters
{
    internal class LiquidMetal : ModWaterStyle
    {
        //  Missing graphics
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override int ChooseWaterfallStyle()
        {
            return ModContent.GetInstance<LiquidMetalStyle>().Slot;
        }
        public override int GetDropletGore()
        {
            return ModContent.Find<ModGore>("ArchaeaMod/Gores/MagicPixel").Type;
        }
        public override int GetSplashDust()
        {
            return ModContent.DustType<Merged.Dusts.c_silver_dust>();
        }
        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            Color c = Color.Silver;
            r = c.R / 255f;
            g = c.G / 255f;
            b = c.B / 255f;
        }
        public override Color BiomeHairColor()
        {
            return Color.Silver;
        }
    }
    internal class LiquidMetalStyle : ModWaterfallStyle
    {
        //  Missing graphics
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override void AddLight(int i, int j)
        {
            Lighting.AddLight(new Vector2(i * 16 + 8, j * 16 + 8), Color.Silver.ToVector3());
        }
        public override void ColorMultiplier(ref float r, ref float g, ref float b, float a)
        {
            Color c = Color.Silver;
            r = c.R / 255f;
            g = c.G / 255f;
            b = c.B / 255f;
        }
    }
}
