using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Gores
{
    internal class MagicPixel : ModGore
    {
        public override void SetStaticDefaults()
        {
            UpdateType = GoreID.WaterDrip;
        }
    }
}
