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
            bossType = ModContent.NPCType<Sky_boss>();
            return player.GetModPlayer<ArchaeaPlayer>().MagnoBiome && !NPC.AnyNPCs(bossType);
        }
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            bossType = ModContent.NPCType<Sky_boss>();
            if (Main.netMode != 2)
            {
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Magnoliac_head>());
            }
            else
            {
                NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: bossType);
            }
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
