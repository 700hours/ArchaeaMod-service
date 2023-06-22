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

using ArchaeaMod.Buffs;
using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items
{
    [CloneByReference]
    public class r_Catcher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rusty Soul Catcher");
            Tooltip.SetDefault("Metallic minion");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 10;
            Item.knockBack = 2f;
            Item.crit = 5;
            Item.value = 3500;
            Item.rare = 2;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.mana = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Summon;
        }
        private int count;
        private int minions;
        private int buffType
        {
            get { return ModContent.BuffType<buff_catcher>(); }
        }
        [CloneByReference]
        private Projectile minion;
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            count = player.ownedProjectileCounts[ModContent.ProjectileType<CatcherMinion>()];
            minions = count + player.numMinions;
            if (minions < player.maxMinions)
            {
                //if (minion != null)
                //    minion.active = false;
                //player.numMinions++;
                Projectile.NewProjectileDirect(Projectile.GetSource_None(), player.position - new Vector2(0, player.height), Vector2.Zero, ModContent.ProjectileType<CatcherMinion>(), Item.damage, Item.knockBack, player.whoAmI, Item.damage);
                if (Main.netMode == 2)
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, minion.whoAmI);
            }
            else return false;
            if (!player.HasBuff(buffType))
            {
                player.AddBuff(buffType, 36000);
                //minion = Projectile.NewProjectileDirect(Projectile.GetSource_None(), player.position - new Vector2(0, player.height), Vector2.Zero, ModContent.ProjectileType<CatcherMinion>(), Item.damage, Item.knockBack, player.whoAmI);
                if (Main.netMode == 2)
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, minion.whoAmI);
            }
            return true;
        }
    }
}
