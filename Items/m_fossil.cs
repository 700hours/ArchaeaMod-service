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
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Scorched Fossil"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip0", ""));
            tooltips.Add(new TooltipLine(Mod, "Tooltip1", "ItemID.Sets.SortingPriorityBossSpawns[Type] = 12"));
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.rare = 2;
            Item.value = 2000;
            Item.maxStack = 99;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = 4;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.noMelee = true;
        }
        private int bossType;
        public override bool CanUseItem(Player player)
        {
            bossType = ModContent.NPCType<Magnoliac_head>();
            return player.GetModPlayer<ArchaeaPlayer>().MagnoBiome && !NPC.AnyNPCs(bossType);
        }
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            if (player.whoAmI == Main.myPlayer)
            {
                bossType = ModContent.NPCType<Magnoliac_head>();
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
                .AddTile(TileID.Anvils)
                .AddIngredient(ModContent.ItemType<Merged.Items.Materials.magno_core>(), 5)
//            recipe.SetResult(this, 1);
                .Register();
        }
    }
}
