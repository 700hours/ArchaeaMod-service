using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchaeaMod.Items
{
    public class PossessedSpiculum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possessed Spiculum");
            Tooltip.SetDefault("Embodied throwing arm");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 15000;
            Item.damage = 50;
            Item.shoot = ModContent.ProjectileType<Projectiles.Spiculum>();
            Item.shootSpeed = 5f;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.reuseDelay = 30;
            Item.autoReuse = false;
            Item.useTime = 90;
            Item.useAnimation = 35;
            Item.channel = false;
            Item.rare = ItemRarityID.LightPurple;
            Item.DamageType = DamageClass.Melee;
            //Item.UseSound = SoundID.?  Throw sound
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Spiculum>()] == 0;
        }
    }
}
