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

using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items
{
    [CloneByReference]
    public class n_Staff : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Necrosis' Staff"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip0", "Emits orbitals around aiming direction"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip1", "Alt use to disarm orbitals"));
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 80;
            Item.knockBack = 0f;
            Item.crit = 20;
            Item.mana = 10;
            Item.value = 3500;
            Item.useTime = 100;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
        }

        private int index = -1;
        private float angle;
        [CloneByReference]
        private Projectile[] projs = new Projectile[6];
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            foreach (Projectile proj in projs.Where(t => t != null))
                proj.Kill();
            index = 0;
            angle = 0f;
            return true;
        }
        public override void HoldItem(Player player)
        {
            if (index != -1)
            {
                if (index == 6)
                {
                    index = -1;
                    return;
                }
                if (ArchaeaItem.Elapsed(10))
                {
                    Vector2 start = NPCs.ArchaeaNPC.AngleBased(player.Center, angle, 45f);
                    projs[index++] = Projectile.NewProjectileDirect(Projectile.GetSource_None(), start, Vector2.Zero, ModContent.ProjectileType<Orbital>(), Item.damage, Item.knockBack, player.whoAmI, angle);
                    angle += (float)Math.PI / 3f;
                }
            }
        }
        public override bool AltFunctionUse(Player player)
        {
            for (int i = 0; i < projs.Length; i++)
                if (projs[i] != null && projs[i].active)
                    projs[i].Kill();
            return true;
        }
    }
}
