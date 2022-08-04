using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ArchaeaMod.Items
{
    public class GhostlyChains : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ghostly Chains");
            Tooltip.SetDefault("Chain gang");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 20000;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.channel = false;
            Item.noMelee = true;
            Item.shootSpeed = 5f;
            Item.knockBack = 1f;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.ghostly_chains>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.LightPurple;
            Item.DamageType = DamageClass.Summon;
            //Item.UseSound = SoundID.?     Throw sound
        }
        int ticks = 0;
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.ghostly_chains>()] == 0;
        }
        public override bool? UseItem(Player player)
        {
            if (ticks++ > 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(Item.GetSource_ItemUse(Item), player.Center + new Vector2(ArchaeaItem.StartThrowX(player), 0), NPCs.ArchaeaNPC.AngleToSpeed(player.Center.AngleTo(Main.MouseWorld), Item.shootSpeed), ModContent.ProjectileType<Projectiles.ghostly_chains>(), 0, 0f, player.whoAmI);
                }
                ticks = 0;
                return true;
            }
            return false;
        }
    }
}
