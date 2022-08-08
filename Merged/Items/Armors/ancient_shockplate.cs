using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Merged.Projectiles;

namespace ArchaeaMod.Merged.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class ancient_shockplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Shock Plate");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = 100;
            Item.rare = 3;
            Item.defense = 5;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Ranged weapons create weak bolts of lightning";
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return
            head.type == ModContent.ItemType<ancient_shockhelmet>() &&
            body.type == Item.type &&
            legs.type == ModContent.ItemType<ancient_shockgreaves>();
        }

        int Proj1;
        int ticks = 0;
        int d = 0;
    }
}
