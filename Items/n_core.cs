using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.NPCs.Bosses;
namespace ArchaeaMod.Items
{
    public class n_core : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Aura");
            Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.rare = 2;
            Item.value = 2000;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = 4;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.noMelee = true;
            bossType = ModContent.NPCType<Sky_boss>();
        }
        private int bossType;
        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.type == bossType && (npc.active || npc.life > 0))
                    return false;
            }
            ArchaeaPlayer modPlayer = player.GetModPlayer<ArchaeaPlayer>();
            if (!modPlayer.SkyFort && !modPlayer.SkyPortal)
                return false;
            return true;
        }
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Sky_boss>());
            SoundEngine.PlaySound(SoundID.Roar, player.Center);
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofLight, 7)
                .AddIngredient(ModContent.ItemType<Materials.r_plate>(), 3)
                .AddIngredient(ModContent.ItemType<Items.Tiles.purple_haze>(), 10)
                .AddTile(TileID.MythrilAnvil)
//            recipe.SetResult(Item.type);
                .Register();
        }
    }
}
