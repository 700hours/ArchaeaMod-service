using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items
{
    public class magno_treasurebag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = 11;
            Item.maxStack = 250;
            Item.consumable = true;
            Item.expert = true;
        }
        public override int BossBagNPC
        {
            get { return Mod.Find<ModNPC>("boss_magnohead").Type;}
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void OpenBossBag(Player player)
        {
            player.QuickSpawnItem(Item.GetSource_Loot(), ItemID.GoldCoin, 5);
            //  player.QuickSpawnItem(ModContent.ItemType<magno_shieldacc>(), 1);
            player.QuickSpawnItem(Item.GetSource_Loot(), ModContent.ItemType<Vanity.magno_mask>(), 1);
            player.QuickSpawnItem(Item.GetSource_Loot(), ModContent.ItemType<Tiles.magno_ore>(), Main.rand.Next(34, 68));
            //  player.QuickSpawnItem(ModContent.ItemType<magno_fragment>(), Main.rand.Next(12, 24));
        }
    }
}
