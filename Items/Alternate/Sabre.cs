using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items.Alternate
{
    public class Sabre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sabre");
            Tooltip.SetDefault("Forged from the materials of magno");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 10;
            Item.knockBack = 3f;
            Item.value = 3000;
            Item.rare = ItemRarityID.Green;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
        }

        private int count;
        private float upward = 0.5f;
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            count = 0;
            if (Main.MouseWorld.X > player.position.X)
                player.direction = 1;
            else player.direction = -1;
            return true;
        }
        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (count++ % 4 == 0)
                Projectile.NewProjectileDirect(Projectile.GetSource_None(), player.direction == 1 ? hitbox.TopRight() : hitbox.TopLeft(), NPCs.ArchaeaNPC.AngleToSpeed(player.direction == 1 ? upward * -1 : (float)Math.PI + upward, 6f), ModContent.ProjectileType<Pixel>(), Item.damage, Item.knockBack, player.whoAmI, Pixel.Fire, Pixel.Sword);
        }
    }
}
