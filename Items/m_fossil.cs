using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class m_fossil : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scorched Fossil");
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
            bossType = ModContent.NPCType<Magnoliac_head>();
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
            if (!player.GetModPlayer<ArchaeaPlayer>().MagnoBiome)
                return false;
            return true;
        }
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Magnoliac_head>());
            SoundEngine.PlaySound(SoundID.Roar, player.Center);
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ModContent.ItemType<Merged.Items.Materials.magno_core>(), 5)
//            recipe.SetResult(this, 1);
                .Register();
        }
    }
}
