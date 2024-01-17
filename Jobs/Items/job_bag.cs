using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Items
{
    public class job_bag : ModItem
    {
        //  Catalogue
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", $"[c/0088ff:{JobID.Name[Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().jobChoice]} catalogue]"));
            tooltips.Add(new TooltipLine(Mod, "ItemName", "[c/ff0000:Choose a job first!]"));
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = 8;
            Item.maxStack = 1;
            Item.consumable = true;
        }
        public override bool CanRightClick()
        {
            int jobChoice = Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().jobChoice;
            if (jobChoice == -1)
                return false;
            return true;
        }
        public override void RightClick(Player player)
        {
            int jobChoice = player.GetModPlayer<ArchaeaPlayer>().jobChoice;
            if (jobChoice == -1) 
                return;
            switch (jobChoice)
            {
                case JobID.ALL_BusinessMan:
                    Jobs.Global.business_man.GiveGear(player);
                    break;
                case JobID.ALL_Entrepreneur:
                    Jobs.Global.entrepreneur.GiveGear(player);
                    break;
                case JobID.ALL_Merchant:
                    Jobs.Global.merchant.GiveGear(player);
                    break;
                case JobID.MAGE_Botanist:
                    Jobs.Global.botanist.GiveGear(player);
                    break;
                case JobID.MAGE_Witch:
                    Jobs.Global.witch.GiveGear(player);
                    break;
                case JobID.MAGE_Wizard:
                    Jobs.Global.wizard.GiveGear(player);
                    break;
                case JobID.MELEE_Smith:
                    Jobs.Global.smith.GiveGear(player);
                    break;
                case JobID.MELEE_Warrior:
                    Jobs.Global.warrior.GiveGear(player);
                    break;
                case JobID.MELEE_WhiteKnight:
                    Jobs.Global.white_knight.GiveGear(player);
                    break;
                case JobID.RANGED_Bowsman:
                    Jobs.Global.bowsman.GiveGear(player);
                    break;
                case JobID.RANGED_CorperateUsurper:
                    Jobs.Global.corperate_usurper.GiveGear(player);
                    break;
                case JobID.RANGED_Outlaw:
                    Jobs.Global.outlaw.GiveGear(player);
                    break;
                case JobID.SUMMONER_Alchemist:
                    Jobs.Global.alchemist.GiveGear(player);
                    break;
                case JobID.SUMMONER_Scientist:
                    Jobs.Global.scientist.GiveGear(player);
                    break;
                case JobID.SUMMONER_Surveyor:
                    Jobs.Global.surveyor.GiveGear(player);
                    break;
                default:
                    break;
            }
        }
    }
}
