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

namespace ArchaeaMod.Items.Alternate
{
    public class Broadsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Broadsword");
            Tooltip.SetDefault("Shocks weakened enemies");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 20;
            Item.crit = 15;
            Item.value = 3500;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Green;
        }

        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            if (Main.mouseLeftRelease && Main.mouseLeft)
                if (!Main.mouseRight)
                    SetMainFunction();
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            if (Item.mana == 0)
                SetAltFunction();
            return true;
        }
        private Target target;
        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (Item.mana > 0)
            {
                target = Target.GetClosest(player, Target.GetTargets(player, 240f).Where(t => t != null).ToArray());
                if (target != null && ArchaeaItem.Elapsed(90))
                    Shield.ShockTarget(hitbox.Center(), target.npc, Item.damage);
            }
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (crit)
            {
                for (float r = 0; r < Math.PI * 2f + Math.PI / 4f; r += (float)Math.PI / 8f)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_None(), target.Center, NPCs.ArchaeaNPC.AngleToSpeed(r, 6f), ModContent.ProjectileType<Pixel>(), Item.damage, Item.knockBack, player.whoAmI, Pixel.Fire, Pixel.Active);
                    proj.timeLeft = 20;
                    proj.tileCollide = false;
                }
            }
        }
        public void SetMainFunction()
        {
            Item.damage = 20;
            Item.mana = 0;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.channel = false;
            Item.DamageType = DamageClass.Melee;
        }
        public void SetAltFunction()
        {
            Item.damage = 10;
            Item.mana = 1;
            Item.useTime = 30;
            Item.useAnimation = 40;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.channel = true;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
    }
}
