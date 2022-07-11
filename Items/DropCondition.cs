using ArchaeaMod.Mode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;

namespace ArchaeaMod.Items
{
    public class ArchaeaModeDrop : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return ModeToggle.archaeaMode;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return "";
        }
    }
    public class Drop : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            throw new NotImplementedException();
        }

        public bool CanShowItemDropInUI()
        {
            throw new NotImplementedException();
        }

        public string GetConditionDescription()
        {
            throw new NotImplementedException();
        }
    }
}
