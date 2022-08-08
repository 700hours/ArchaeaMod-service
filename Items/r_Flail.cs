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

namespace ArchaeaMod.Items
{
    [CloneByReference]
    public class r_Flail : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rusty Thrasher");
            Tooltip.SetDefault("Scatters rust");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 14;
            Item.knockBack = 2f;
            Item.value = 3500;
            Item.crit = 8;
            Item.rare = 2;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 8f;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
        }

        [CloneByReference]
        private Projectile proj;
        public override void HoldItem(Player player)
        {
            if (proj != null && proj.active)
                player.controlUseItem = true;
        }
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            if (proj == null || !proj.active)
            {
                proj = Throw(player, Flail.Fling);
                return true;
            }
            return false;
        }
        public override bool AltFunctionUse(Player player)
        {
            if (proj == null || !proj.active)
            {
                proj = Throw(player, Flail.Swing);
                return true;
            }
            return false;
        }
        protected Projectile Throw(Player player, int ai)
        {
            float angle = NPCs.ArchaeaNPC.AngleTo(player.Center, Main.MouseWorld);
            Vector2 velocity = NPCs.ArchaeaNPC.AngleToSpeed(angle, Item.shootSpeed);
            return Projectile.NewProjectileDirect(Projectile.GetSource_None(), new Vector2(ArchaeaItem.StartThrowX(player), player.Center.Y - 24f), velocity, ModContent.ProjectileType<Flail>(), Item.damage, Item.knockBack, player.whoAmI, ai);
        }
    }
}
