using static Terraria.ID.ContentSamples.CreativeHelper;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using IL.Terraria.DataStructures;

namespace ArchaeaMod.Tiles
{
    internal class factory_oil_leak : ModTile
    {
        public override bool IsTileDangerous(int i, int j, Player player) 
            => true;
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            ItemDrop = ModContent.ItemType<Items.Tiles.factory_brick_1>();
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
        public override void RandomUpdate(int i, int j)
        {
            int x = i * 16 + 8;
            int y = j * 16 + 18;
            Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), new Vector2(x, y), Vector2.Zero, ModContent.ProjectileType<Projectiles.Pixel>(), 0, 0f, ai1: Projectiles.Pixel.Drip);
        }
    }
}
