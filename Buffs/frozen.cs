using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Buffs
{
    public class frozen : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Frozen");
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            tip = "\"I can't move!\"";
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.velocity.X /= 2f;
            npc.damage = 0;
            Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.t_Frozen);
            dust.noGravity = true;
            dust.noLight = false;
        }
    }
}
