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
        public override string Texture => "ArchaeaMod/Items/c_Sword";
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Crystal Broadsword"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip0", "Shocks weakened enemies"));
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 20;
            Item.crit = 15;
            Item.value = 3500;
            Item.useTime = 90;
            Item.useAnimation = 90;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Green;
        }

        int ticks = 0;
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
        [CloneByReference]
        private Target target;
        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (Item.mana > 0)
            {
                if (player.statMana > 0)
                {
                    target = Target.GetClosest(player, Target.GetTargets(player, 240f).Where(t => t != null).ToArray());
                    if (target != null && ArchaeaItem.Elapsed(ref ticks, 3))
                    { 
                        Shield.ShockTarget(hitbox.Center(), target.npc, Item.damage);
                        player.statMana--;
                        ticks = 0;
                    }
                }
            }
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
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
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.noMelee = false;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.channel = false;
            Item.DamageType = DamageClass.Melee;
        }
        public void SetAltFunction()
        {
            Item.damage = 10;
            Item.mana = 1;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.channel = true;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/c_Sword").Value;
            sb.Draw(tex, position, frame, Color.Firebrick * 0.67f, 0f, origin, 1f, SpriteEffects.None, 0f);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/c_Sword").Value;
            sb.Draw(tex, Item.position - Main.screenPosition + new Vector2(16, 32), null, Color.Firebrick * 0.67f, 0f, new Vector2(tex.Width / 2, tex.Height / 2), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
