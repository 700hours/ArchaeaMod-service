using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

using ArchaeaMod.Entities;

namespace ArchaeaMod.Items
{
    public class m_shield : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Magno Shield"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip0", ""));
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.value = 10000;
            Item.rare = -12;
            Item.accessory = true;
            Item.expert = true;
        }
        private bool generate = true;
        private int time;
        private const int regen = 420;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            int count = ArchaeaEntity.entity.Where(e => e != null && e.owner == player.whoAmI && e.active && e.type == ArchaeaEntity.ID.Shield).Count();
            if (count < 4)
            {
                if (time++ > regen)
                {
                    foreach (var e in ArchaeaEntity.entity.Where(e => e != null && e.owner == player.whoAmI))
                    {
                        e.Kill(false);
                    }
                }
            }
            if ((generate && count == 0) || time > regen)
            {
                foreach (var e in ArchaeaEntity.entity.Where(e => e != null && e.owner == player.whoAmI))
                {
                    e.Kill(false);
                }
                for (int i = 0; i < 4; i++)
                {
                    var entity = ArchaeaEntity.NewEntity(player.Center, Vector2.Zero, 0, player.whoAmI, (i + 1) * 90f * 0.017f);
                    entity.netUpdate2 = true;
                }
                time = 0;
                generate = false;
            }
        }
    }
}
