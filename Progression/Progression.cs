using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using ArchaeaMod.Interface.ModUI;
using Terraria.DataStructures;
using System.Timers;

namespace ArchaeaMod.Progression
{
    public sealed class ProgressID
    {
        public const int
            ArrowSpeed = 0,
            JumpHeight = 1,
            AttackSpeed = 2,
            MoveSpeed = 3,
            BreathTime = 4,
            Toughness = 5,
            DamageBuff = 6,
            MerchantDiscount = 7,
            PercentDamageReduction = 8,
            AmmoReduction = 9;
    }
    public class ProgressItem : GlobalItem
    {
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            damage = new StatModifier(player.GetModPlayer<ArchaeaPlayer>().damageBuff, 1f);
        }
    }
    public class ProgressProj : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.friendly && projectile.arrow && projectile.owner == Main.LocalPlayer.whoAmI)
            {
                projectile.velocity *= Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().arrowSpeed;
            }
        }
    }
}
