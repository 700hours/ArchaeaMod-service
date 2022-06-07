using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items
{
    public class gray_solution : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scorched Solution");
            Tooltip.SetDefault("Transforms stone into Magno stone");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.rare = 2;
            Item.value = 500;
            Item.maxStack = 999;
            Item.ammo = AmmoID.Solution;
            Item.shoot = ModContent.ProjectileType<GraySolution>() - ProjectileID.PureSpray;
            Item.consumable = true;
        }
    }
    sealed class GraySolution : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gray solution");
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.scale = 0.67f;
            Projectile.timeLeft = 80;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            for (int n = 0; n < Projectile.width; n++)
            {
                int i = (int)Projectile.position.X + n;
                int j = (int)Projectile.position.Y + n;
                Tile tile = Framing.GetTileSafely(new Vector2(i, j));
                if (tile.TileType == TileID.Stone && tile.HasTile)
                {
                    tile.TileType = (ushort)ModContent.TileType<Merged.Tiles.m_stone>();
                    WorldGen.SquareTileFrame(i / 16, j / 16, true);
                }
            }
            int t = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Merged.Dusts.magno_dust>(), 0f, 0f, 0, default(Color), 1.2f);
            Main.dust[t].noGravity = true;
            Main.dust[t].noLight = true;
        }
    }
}
