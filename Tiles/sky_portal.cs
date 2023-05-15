using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArchaeaMod.Tiles
{
    public class sky_portal : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Portal");
            AddMapEntry(new Color(0.6f, 0.6f, 0f), name);
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {                            
            fail = true;
            num = 0;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            fail = true;
            effectOnly = true;
            noItem = true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        float ticks = -300;
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Main.dedServ)
                return false;
            if (ticks++ >= 300)
                ticks = -300;
            if ((int)Main.time % 10 == 0)
            {
                int x = i * 16 + 24;
                int y = j * 16 + 24;
                Vector2 v2 = new Vector2(x, y);
                double radius = Vector2.Lerp(new Vector2(100, 0), new Vector2(300, 0), Math.Abs(ticks)).X;
                double cos  = v2.X + radius * Math.Cos(Math.PI * 2f * Main.rand.NextFloat());
                double sine = v2.Y + radius * Math.Sin(Math.PI * 2f * Main.rand.NextFloat());
                ArchaeaPlayer.RadialDustDiffusion(v2, cos, sine, (float)radius, ModContent.DustType<Dusts.Shimmer_1>(), 0, true);
            }
            return false;
        }
    }
}
