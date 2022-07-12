using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Items
{
    public class magno_book : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Book");
            Tooltip.SetDefault("Indicates magic effects");
        }
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 24;
            Item.scale = 1f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.damage = 15;
            Item.mana = 8;
            Item.crit = 7;
            Item.useStyle = 1;
            Item.value = 2500;
            Item.rare = 2;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.CountsAsClass(DamageClass.Magic);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("magno_orb").Type] < 1)
            {
                return true;
            }
            else return false;
        }
        int Proj1;
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Item14, player.Center);
            Proj1 = Projectile.NewProjectile(Projectile.GetSource_None(), player.position + new Vector2(player.width / 2, player.height / 2), Vector2.Zero, Mod.Find<ModProjectile>("magno_orb").Type, (int)(15f * player.GetDamage(DamageClass.Magic).Multiplicative), 4f, player.whoAmI, 0f, 0f);
            Main.projectile[Proj1].netUpdate = true;
            return true;
        }
    }
}
