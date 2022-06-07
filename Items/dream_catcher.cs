using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items
{
    public class dream_catcher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dream Catcher");
            Tooltip.SetDefault("Holds the stories of bygone times\n" +
                "Gives a buff during night\n" +
                "A ward against the haze");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = 10000;
            Item.accessory = true;
        }
        bool init;
        int effect = -1;
        public override void UpdateEquip(Player player)
        {
            if (!Main.dayTime)
            {
                if ((int)Main.time % 3600 == 0 || !init)
                {
                    effect = Main.rand.Next(0, 4);
                    init = true;
                }
            }
            switch (effect)
            {
                case -1:
                    goto default;
                case 0:
                    player.AddBuff(BuffID.Gravitation, 60, true);
                    break;
                case 1:
                    player.AddBuff(BuffID.Warmth, 60, true);
                    break;
                case 2:
                    player.AddBuff(BuffID.Honey, 60, true);
                    break;
                case 3:
                    player.AddBuff(BuffID.Tipsy, 60, true);
                    break;
                case 4:
                    player.AddBuff(BuffID.WellFed, 60, true);
                    break;
                default:
                    break;
            }
        }
    }
}
