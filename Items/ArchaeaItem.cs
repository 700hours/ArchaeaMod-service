﻿using System;
using System.Collections.Generic;
using System.IO;
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
using ArchaeaMod.Items.Alternate;
using ArchaeaMod.Projectiles;
using ArchaeaMod.NPCs;
using Microsoft.CodeAnalysis.Operations;
using ArchaeaMod.TakerylProject;

namespace ArchaeaMod
{
    public class ModItemID
    {
        public static int Deflector
        {
            get { return ModContent.ItemType<Deflector>(); }
        }
        public static int Sabre
        {
            get { return ModContent.ItemType<Sabre>(); }
        }
        public static int PossessedMusket => ModContent.ItemType<Items.PossessedMusket>();
        public static int PossessedSpiculum => ModContent.ItemType<Items.PossessedSpiculum>();
        public static int GhostlyChains => ModContent.ItemType<Items.GhostlyChains>();
    }
}
namespace ArchaeaMod.Items
{
    public class ArchaeaItem
    {
        public static void ActiveChannelStyle(Player player)
        {
            player.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
            float PI = (float)Math.PI;
            float angle = NPCs.ArchaeaNPC.AngleTo(player.Center, Main.MouseWorld);
            player.itemRotation = angle + (player.direction == -1 ? PI : 0);
            player.itemLocation.X = NPCs.ArchaeaNPC.AngleBased(player.position, angle, player.width / 4).X - player.width / 2 - 4;
        }
        public static float StartThrowX(Player player)
        {
            float angle = NPCs.ArchaeaNPC.AngleTo(player.Center, Main.MouseWorld);
            return NPCs.ArchaeaNPC.AngleBased(player.position, angle, player.width / 4).X - player.width / 2 - 4;
        }
        public static bool NotEquipped(Player player, int type)
        {
            int index = 0;
            for (int i = 0; i < player.armor.Length; i++)
            {
                if (player.armor[i].type != type)
                    index++;
                else break;
                if (index == player.armor.Length - 1 && player.armor[i].type != type)
                    return true;
            }
            return false;
        }
        public static bool HasEquipped(Player player, int type)
        {
            for (int i = 0; i < player.armor.Length; i++)
            {
                if (player.armor[i].type == type)
                { 
                    return true;
                }
            }
            return false;
        }
        public static bool Elapsed(int interval)
        {
            return Math.Round(Main.time, 0) % interval == 0;
        }
        public static bool Elapsed(int interval, int remainder)
        {
            return interval % remainder == 0;
        }
        public static bool Elapsed(ref int interval, int time)
        {
            return ++interval >= time;
        }
        public static bool Elapsed(ref float interval, int time)
        {
            return ++interval >= time;
        }
        public static bool ArmorSet(Player player, string head, string body, string legs)
        {
            return player.armor[0].Name == head &&
                   player.armor[1].Name == body &&
                   player.armor[2].Name == legs;
        }
        public static bool ArmorSet(Player player, int head, int body, int legs)
        {
            return player.armor[0].type == head &&
                   player.armor[1].type == body &&
                   player.armor[2].type == legs;
        }
        public static void Bolt(Player owner, NPC target, ref Vector2 start, int damage = 20)
        {
            float max = target.Distance(start);
            for (int k = 0; k < max; k++)
            {
                for (int i = 0; i < 30; i++)
                {
                    if (start.Y > target.position.Y + target.height)
                        return;
                    float angle = Main.rand.NextFloat(0f, (float)Math.PI);
                    start += NPCs.ArchaeaNPC.AngleToSpeed(angle, k);
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_None(), start, Vector2.Zero, ModContent.ProjectileType<Pixel>(), damage, 0f, owner.whoAmI, Pixel.Electric, Pixel.Active);
                    proj.timeLeft = 3;
                }
            }
        }
        public static void Bolt(ref Vector2 start, Vector2 end, int damage = 20, int arcs = 30, float localAI0 = 0f, float localAI1 = 0f)
        {
            float max = end.Distance(start);
            for (int k = 0; k < max; k++)
            {
                for (int i = 0; i < arcs; i++)
                {
                    if (start.Y > end.Y)
                        return;
                    float angle = Main.rand.NextFloat(0f, (float)Math.PI);
                    start += NPCs.ArchaeaNPC.AngleToSpeed(angle, k);
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_None(), start, Vector2.Zero, ModContent.ProjectileType<Pixel>(), damage, 0f, Main.myPlayer, Pixel.Electric, Pixel.Active);
                    proj.timeLeft = 3;
                    proj.localAI[0] = localAI0;
                    proj.localAI[1] = localAI1;
                }
            }
        }
        public static void Bolt(ref Vector2 start, Vector2 end, float angle, int arcs = 30, int timeLeft = 3, float ai0 = 0f, float ai1 = 0f)
        {
            bool flag = false;
            List<Vector2> list = new List<Vector2>();
            float max = end.Distance(start);
            for (int k = 1; k < max; k++)
            {
                for (int i = 0; i < arcs; i++)
                {
                    if (start.Y > end.Y)
                    {
                        flag = true;
                        break;
                    }
                    float _angle = Main.rand.NextFloat(0f, (float)Math.PI);
                    start += NPCs.ArchaeaNPC.AngleToSpeed(_angle, k);
                    list.Add(ArchaeaNPC.AngleBased(start, angle, k));
                }
                if (flag)
                {
                    break;
                }
            }
            foreach (Vector2 v2 in list)
            { 
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_None(), v2, Vector2.Zero, ModContent.ProjectileType<Pixel>(), 20, 0f, Main.myPlayer, Pixel.Electric, Pixel.Active);
                proj.timeLeft = timeLeft;
                proj.localAI[0] = ai0;
                proj.localAI[1] = ai1;
            }
        }

        public static void SyncProj(int netID, Projectile Projectile)
        {
            if (Main.netMode == netID)
            {
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI, Projectile.position.X, Projectile.position.Y, Projectile.rotation);
                Projectile.netUpdate = true;
            }
        }
        public static void SyncProj(Projectile Projectile)
        {
            if (Main.netMode == 1)
            {
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
            }
        }
        public static void DustCircle(Vector2 origin, int dustID)
        {
            for (float r = 0f; r < Math.PI * 2f; r += 0.017f * (45f / 15f))
            {
                Vector2 velocity = NPCs.ArchaeaNPC.AngleToSpeed(r, 15f);
                int index = Dust.NewDust(origin, 1, 1, dustID, velocity.X, velocity.Y, 0, default, 1.2f);
                Main.dust[index].noGravity = true;
            }
        }
        public static void DustCircle(Vector2 origin, float radius, int dustID)
        {
            for (float r = 0f; r < Math.PI * 2f; r += 0.017f * (45f / 15f))
            {
                Vector2 position = NPCs.ArchaeaNPC.AngleBased(origin, r, radius);
                int index = Dust.NewDust(position, 1, 1, dustID, 0, 0, 0, default, 1.2f);
                Main.dust[index].noGravity = true;
            }
        }
        public static void ProjectileCircle(Vector2 origin, int damage, int projID, int timeLeft)
        {
            for (float r = 0f; r < Math.PI * 2f; r += 0.017f * (45f / 15f))
            {
                Vector2 velocity = NPCs.ArchaeaNPC.AngleToSpeed(r, 15f);
                int index = Projectile.NewProjectile(Projectile.GetSource_None(), origin, velocity, projID, damage, 0f, Main.myPlayer);
                Main.projectile[index].timeLeft = timeLeft;
                Main.projectile[index].tileCollide = false;
                Main.projectile[index].ignoreWater = true;
            }
        }
        public static void ProjectileCircle(Vector2 origin, int damage, int projID, int timeLeft, int ai0, int ai1, int localai0, int localai1)
        {
            for (float r = 0f; r < Math.PI * 2f; r += 0.017f * (45f / 15f))
            {
                Vector2 velocity = NPCs.ArchaeaNPC.AngleToSpeed(r, 15f);
                int index = Projectile.NewProjectile(Projectile.GetSource_None(), origin, velocity, projID, damage, 0f, Main.myPlayer);
                Main.projectile[index].timeLeft = timeLeft;
                Main.projectile[index].tileCollide = false;
                Main.projectile[index].ignoreWater = true;
                Main.projectile[index].ai[0] = ai0;
                Main.projectile[index].ai[1] = ai1;
                Main.projectile[index].localAI[0] = localai0;
                Main.projectile[index].localAI[1] = localai1;
            }
        }
    }

    public class ArchaeaItem_Global : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (SwordID.swordTypes.Contains(item.type))
            {
                tooltips.Add(new TooltipLine(Mod, "ItemName", "[c/0088ff:Can throw and swing]"));
            }
        }
        public override void HoldItem(Item item, Player player)
        {
            if (player.releaseUseItem && player.controlUseItem && item.DamageType == DamageClass.Throwing)
            {
                float range = 800f;
                Target[] targets = Target.GetTargets(player, range).Where(t => t != null).ToArray();
                if (targets == null)
                    return;
                //  Non-ancient
                if (ArchaeaItem.ArmorSet(player, ModContent.ItemType<Items.Armors.ShockMask>(), ModContent.ItemType<Items.Armors.ShockPlate>(), ModContent.ItemType<Items.Armors.ShockLegs>()))
                { 
                    foreach (Target target in targets)
                    {
                        if (Target.HitByThrown(player, target))
                        {
                            Vector2 start = target.npc.Center - new Vector2(0f, 500f);
                            ArchaeaItem.Bolt(player, target.npc, ref start, 80);
                        }
                    }
                }
                //  Ancient
                if (ArchaeaItem.ArmorSet(player, ModContent.ItemType<Merged.Items.Armors.ancient_shockhelmet>(), ModContent.ItemType<Merged.Items.Armors.ancient_shockplate>(), ModContent.ItemType<Merged.Items.Armors.ancient_shockgreaves>()))
                {
                    foreach (Target target in targets)
                    {
                        if (Target.HitByThrown(player, target))
                        {
                            Vector2 start = target.npc.Center - new Vector2(0f, 500f);
                            ArchaeaItem.Bolt(player, target.npc, ref start, 20);
                        }
                    }
                }
            }
        }
    }

    public class Target
    {
        public int time;
        public NPC npc;
        public Player player;
        public const int
            Default = 0,
            ShockWave = 1,
            Frozen = 2,
            Fire = 3;
        private Mod mod
        {
            get { return ModLoader.GetMod("ArchaeaMod"); }
        }
        public Target(NPC npc, Player player)
        {
            this.npc = npc;
            this.player = player;
        }
        public void AttackEffect(int type)
        {
            switch (type)
            {
                case Default:
                    break;
                case ShockWave:
                    float angle = NPCs.ArchaeaNPC.AngleTo(player.Center, npc.Center);
                    npc.velocity.Y -= 8f;
                    npc.velocity += NPCs.ArchaeaNPC.AngleToSpeed(angle, 12f);
                    NPCs.ArchaeaNPC.VelocityClamp(npc, -10f, 10f);
                    break;
                case Frozen:
                    npc.AddBuff(ModContent.BuffType<frozen>(), 60);
                    break;
                case Fire:
                    npc.AddBuff(BuffID.OnFire, 10);
                    break;
            }
        }
        public bool Elapsed(int interval)
        {
            return time++ % interval == 0 && time != 0;
        }
        public static bool HitByThrown(Player player, Target target)
        {
            foreach (Projectile proj in Main.projectile)
                if (proj.owner == player.whoAmI && (proj.DamageType == DamageClass.Throwing || proj.DamageType == DamageClass.Ranged))
                    if (proj.Center.Distance(target.npc.Center) < proj.width + target.npc.width / 2 + 18f)
                        return true;
            return false;
        }
        public static Target GetClosest(Player owner, Target[] targets)
        {
            List<float> ranges = new List<float>();
            foreach (Target target in targets)
            {
                ranges.Add(target.npc.Distance(owner.Center));
                return targets[ranges.IndexOf(ranges.Min())];
            }
            return null;
        }
        public static Target[] GetTargets(Player player, float range)
        {
            Target[] targets = new Target[255];
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (!Shield.GetOnCondition(Main.npc[i])) continue;
                if (player.Distance(Main.npc[i].Center) > range) continue;
                targets[i] = new Target(Main.npc[i], player);
            }
            return targets;
        }
        public static Target[] GetTargets(NPC npc, float range)
        {
            Target[] targets = new Target[255];
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (!Shield.GetOnCondition(Main.npc[i])) continue;
                if (npc.Distance(Main.npc[i].Center) > range) continue;
                targets[i] = new Target(Main.npc[i], null);
            }
            return targets;
        }
        public static Target[] GetTargets(Projectile projectile, float range)
        {
            Target[] targets = new Target[255];
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (!Shield.GetOnCondition(Main.npc[i])) continue;
                if (projectile.Distance(Main.npc[i].Center) > range) continue;
                targets[i] = new Target(Main.npc[i], null);
            }
            return targets;
        }
    }
}
