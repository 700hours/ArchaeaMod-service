using ArchaeaMod.Mode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace ArchaeaMod.Items
{
    public class ArchaeaModeDrop : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return ModContent.GetInstance<ModeToggle>().archaeaMode;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return "Archaea mode";
        }
    }
    public class HardModeDrop : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return Main.hardMode;
        }

        public bool CanShowItemDropInUI()
        {
            return false;
        }

        public string GetConditionDescription()
        {
            return "Hard mode";
        }
    }
}
