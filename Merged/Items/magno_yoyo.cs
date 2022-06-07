using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Merged.Projectiles;

namespace ArchaeaMod.Merged.Items
{
    public class magno_yoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rubidium Yoyo");
            Tooltip.SetDefault("Throws out a Yoyo");
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.damage = 18;
            Item.knockBack = 6f;
            Item.value = 2500;
            Item.rare = 1;

            //  custom sound?
            //  item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/*");
            //  default usesound
            Item.UseSound = SoundID.Item1;
            Item.channel = true;

            Item.autoReuse = false;
            Item.consumable = false;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Melee;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<magno_yoyoprojectile>();
        }
    }
}
