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
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Dark Aura"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip0", "For use in the dark sky portal chamber."));
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
            bossType = ModContent.NPCType<Sky_boss>();
            return player.GetModPlayer<ArchaeaPlayer>().SkyPortal && !NPC.AnyNPCs(bossType);
        }
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            if (player.whoAmI == Main.myPlayer)
            {
                bossType = ModContent.NPCType<Sky_boss>();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, bossType);
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: bossType);
                }
                SoundEngine.PlaySound(SoundID.Roar, player.Center);
                return true;
            }
            return false;
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
