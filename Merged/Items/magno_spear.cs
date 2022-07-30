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
                "Increases defense\n" +
                "Increases movement speed by 30%\n" +
                "Decreases jump height by 90%\n" +
                "Decreases jump speed by 50%");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 0.875f;
            Item.accessory = true;
            Item.value = 12000;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Orange;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            bool mode = ModContent.GetInstance<Mode.ModeToggle>().archaeaMode;
            player.statDefense = (int)(player.statDefense * (mode ? 2 : 1.1f));
            player.moveSpeed *= 1.3f;
            Player.jumpHeight = (int)(Player.jumpHeight * 0.1f);;
            Player.jumpSpeed *= 0.5f;
        }
    }
}
