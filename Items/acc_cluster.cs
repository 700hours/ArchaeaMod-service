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
    public class acc_cluster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Cluster");
            Tooltip.SetDefault("Adds a damage boost per successful enemy hit" +
                "\nResets after taking damage");
        }
        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 32;
            item.rare = -12;
            item.value = 20000;
            item.accessory = true;
            item.expert = true;
        }
        public override void UpdateEquip(Player player)
        {
            if (player.armor.Contains(item))
                player.AddBuff(ModContent.BuffType<Buffs.buff_cluster>(), int.MaxValue, true);
            else player.DelBuff(player.FindBuffIndex(ModContent.BuffType<Buffs.buff_cluster>()));
        }
    }
    public class AccPlayer : ModPlayer
    {
        public int stack;
        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            stack = 0;
        }
        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            stack = 0;
        }
        public override void PostUpdateEquips()
        {
            if (!ArchaeaItem.Elapsed(180))
                return;
            for (int i = 0; i < player.armor.Length; i++)
            {
                if (player.armor[i].type == ModContent.ItemType<acc_cluster>())
                {
                    return;
                }
                if (i == player.armor.Length - 1)
                {
                    player.ClearBuff(ModContent.BuffType<Buffs.buff_cluster>());
                    break;
                }
            }
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            for (int i = 0; i < player.armor.Length; i++)
            {
                if (player.armor[i].type == ModContent.ItemType<acc_cluster>())
                {
                    stack++;
                    break;
                }
            }
            damage += stack;
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            for (int i = 0; i < player.armor.Length; i++)
            {
                if (player.armor[i].type == ModContent.ItemType<acc_cluster>())
                {
                    stack++;
                    break;
                }
            }
            damage += stack;
        }
    }
}
