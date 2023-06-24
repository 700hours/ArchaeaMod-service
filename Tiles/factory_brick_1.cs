using static Terraria.ID.ContentSamples.CreativeHelper;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using ArchaeaMod.Items;
using Terraria.GameContent.Liquid;
using System.Linq;

namespace ArchaeaMod.Tiles
{
    internal class factory_brick_1 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            ItemDrop = ModContent.ItemType<Items.Tiles.sky_brick>();
            AddMapEntry(Color.LightSlateGray);
            // soundStyle = 0;
            HitSound = SoundID.Tink;
            MineResist = 1.5f;
            MinPick = 100;
        }
        public override bool HasWalkDust()
        {
            return true;
        }
        public override void WalkDust(ref int DustType, ref bool makeDust, ref Color color)
        {
            DustType = ModContent.DustType<Merged.Dusts.c_silver_dust>();
            makeDust = true;
            color = Color.LightGray;
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        bool flag = false;
        public override void NearbyEffects(int i, int j, bool closer)
        {
            for (int l = 1; l < 5; l++)
                if (Main.tile[i, j + l].HasTile)
                {
                    return;
                }
            int x = i * 16 + 8;
            int y = j * 16 + 18;
            if (Main.rand.NextBool(900))
            {     
                Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), new Vector2(x, y), Vector2.Zero, ModContent.ProjectileType<Projectiles.Pixel>(), 0, 0f, Main.myPlayer, 0f, Projectiles.Pixel.Drip);
            }
            if (Main.rand.NextBool(400))
            {
                if (!Main.tile[i - 1, j].HasTile || !Main.tile[i + 1, j].HasTile)
                { 
                    Vector2 start = new Vector2(x, y);
                    var list = Treasures.GetFloor(i - 10, j, 20, 30, Type).ToList();
                    if (list.Count < 2 || list[0] == Vector2.Zero)
                    { 
                        return;
                    }
                    Vector2 end = list[Main.rand.Next(list.Count)] * 16;
                    ArchaeaItem.Bolt(ref start, end, 20, 4, -100f);
                }
            }
        }
    }
}
