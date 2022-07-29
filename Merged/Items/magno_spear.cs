using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items
{
    public class magno_spear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rubidium Staff");
            Tooltip.SetDefault("Magnoliac artifact\n" +
                "Increases ranged armor piercing\n" +
                "Increases movement speed by 30%\n" +
                "Decreases jump height by 67%");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 0.67f;
            Item.accessory = true;
            Item.value = 12000;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Orange;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            bool mode = ModContent.GetInstance<Mode.ModeToggle>().archaeaMode;
            player.GetArmorPenetration(DamageClass.Ranged) += mode ? 50 : 10;
            player.moveSpeed *= 1.3f;
            Player.jumpHeight /= 3;
        }
    }
}
