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
using Terraria.Audio;
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
    public sealed class TraitID
    {
        public const int
            NONE = -1,
            MELEE_DoubleSwing = 0,
            MELEE_DoubleKb = 1,
            MELEE_Stun = 2,
            MELEE_Flask = 3,
            MELEE_LeapAttack = 4,
            MELEE_Leap = 5,
            RANGED_DoubleFire = 6,
            RANGED_Tracking = 7,
            RANGED_Fire = 8,
            RANGED_Ice = 9,
            RANGED_Ichor = 10,
            RANGED_Cinnabar = 11,
            MAGE_DoubleAttack = 12,
            MAGE_NoManaCost = 13,
            MAGE_DamageReduce = 14,
            MAGE_MoveSpeed = 15,
            MAGE_NoKb = 16,
            MAGE_DecreaseCost = 17,
            SUMMONER_PlusCount = 18,
            SUMMONER_MinionDmg = 19,
            SUMMONER_NoManaCost = 20,
            SUMMONER_ThrowBoulder = 21,
            SUMMONER_ThrowBones = 22,
            SUMMONER_ThrowStar = 23,
            ALL_WallJump = 24,
            ALL_DoubleJump = 25,
            ALL_TenDefense = 26,
            ALL_IncJumpHeight = 27,
            ALL_Dash = 28,
            ALL_FallingStarAtk = 29;
    }
}
namespace ArchaeaMod.Progression.Global
{
    using tUserInterface.Extension;

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
    public class ClassNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int deathProjType;
        public int lastHitOwner;
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            deathProjType = projectile.type;
            lastHitOwner = projectile.owner;
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            if (ArchaeaPlayer.CheckHasTrait(TraitID.MELEE_Flask, ClassID.Melee, player.whoAmI))
            {
                if (Main.rand.NextFloat() < 0.5f)
                { 
                    npc.AddBuff(ModContent.BuffType<Buffs.mercury>(), 180);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.AddNPCBuff, -1, -1, null, npc.whoAmI, BuffID.OnFire, 180);
                    }
                }
            }
            lastHitOwner = player.whoAmI;
        }
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.Nymph)
            {
                ArchaeaPlayer.SetClassTrait(TraitID.MELEE_Stun, ClassID.Melee, true, npc.target);
            }
            if (npc.boss)
            {
                if (deathProjType == ProjectileID.CannonballFriendly) 
                { 
                    ArchaeaPlayer.SetClassTrait(TraitID.SUMMONER_ThrowBoulder, ClassID.Summoner, true, lastHitOwner);
                }
                if (npc.type == NPCID.EyeofCthulhu)
                {
                    ArchaeaPlayer.SetClassTrait(TraitID.ALL_Dash, ClassID.All, true, lastHitOwner);
                }
            }
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (Main.townNPCCanSpawn.Count(t => t == true) >= 10)
            {
                ArchaeaPlayer.SetClassTrait(TraitID.ALL_FallingStarAtk, ClassID.All, true, Main.LocalPlayer.whoAmI);
            }
        }
    }
    public class ClassItem : GlobalItem
    {
        Timer timer;
        float rand;
        public override bool InstancePerEntity => true;
        //  Assigning trait effects
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ArchaeaPlayer.CheckHasTrait(TraitID.RANGED_DoubleFire, ClassID.Ranged, player.whoAmI))
            {
                timer = new Timer(Math.Max((float)item.useTime / Main.frameRate * 1000f / 2f, 100f));
                timer.Enabled = true;
                timer.Elapsed += (object sender, ElapsedEventArgs e) => 
                {
                    player.ApplyItemAnimation(item, 0.5f);
                    SoundEngine.PlaySound(item.UseSound.Value, player.Center);
                    Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
                    timer.Dispose();
                };
                timer.Start();
            }
            if (item.DamageType == DamageClass.Magic)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.MAGE_DoubleAttack, ClassID.Magic, player.whoAmI))
                {
                    if (rand < 0.5f)
                    { 
                        timer = new Timer(Math.Max((float)item.useTime / Main.frameRate * 1000f / 2f, 100f));
                        timer.Enabled = true;
                        timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                        {
                            player.ApplyItemAnimation(item, 0.5f);
                            SoundEngine.PlaySound(item.UseSound.Value, player.Center);
                            Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
                            timer.Dispose();
                        };
                        timer.Start();
                    }
                }
            }
            return true;
        }
        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            if (item.DamageType == DamageClass.Magic)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.MAGE_NoManaCost, ClassID.Magic, player.whoAmI))
                {
                    if (Main.rand.NextFloat() < 0.5f)
                    {
                        reduce += item.mana;
                    }
                }
            }
            if (item.DamageType == DamageClass.Summon)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.SUMMONER_NoManaCost, ClassID.Summoner, player.whoAmI))
                {
                    reduce += item.mana;
                }
            }
        }

        //  Assigning trait values
        public override bool OnPickup(Item item, Player player)
        {
            if (item.type == ItemID.DaedalusStormbow)
            {
                ArchaeaPlayer.SetClassTrait(TraitID.RANGED_Cinnabar, ClassID.Ranged, true, player.whoAmI);
            }
            if (item.type == ItemID.GiantHarpyFeather)
            {
                ArchaeaPlayer.SetClassTrait(TraitID.SUMMONER_PlusCount, ClassID.Summoner, true, player.whoAmI);
            }
            return true;
        }
        public override void OnCreate(Item item, ItemCreationContext context)
        {
            if (item.type == ItemID.StarCannon) 
            {
                ArchaeaPlayer.SetClassTrait(TraitID.RANGED_DoubleFire, ClassID.Ranged, true, item.playerIndexTheItemIsReservedFor);
            }
            if (item.wingSlot > 0)
            {
                ArchaeaPlayer.SetClassTrait(TraitID.MELEE_LeapAttack, ClassID.Melee, true, item.playerIndexTheItemIsReservedFor);
            }
            if (item.type == ItemID.AnkhShield)
            {
                ArchaeaPlayer.SetClassTrait(TraitID.MAGE_DoubleAttack, ClassID.Magic, true, item.playerIndexTheItemIsReservedFor);
            }
            if (item.Name.Contains("Hook"))
            {
                ArchaeaPlayer.SetClassTrait(TraitID.MAGE_MoveSpeed, ClassID.Magic, true, item.playerIndexTheItemIsReservedFor);
            }
            if (item.type == ItemID.AvengerEmblem)
            {
                ArchaeaPlayer.SetClassTrait(TraitID.ALL_WallJump, ClassID.All, true, item.playerIndexTheItemIsReservedFor);
            }
            if (item.type == ItemID.MoltenPickaxe)
            {
                ArchaeaPlayer.SetClassTrait(TraitID.ALL_TenDefense, ClassID.All, true, item.playerIndexTheItemIsReservedFor);
            }
        }
        public override float UseTimeMultiplier(Item item, Player player)
        {
            if (item.DamageType == DamageClass.Melee)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.MELEE_DoubleSwing, ClassID.Melee, player.whoAmI))
                {
                    return 0.75f;
                }
            }
            if (item.DamageType == DamageClass.Magic && (rand = Main.rand.NextFloat()) < 0.5f)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.MAGE_DoubleAttack, ClassID.Magic, player.whoAmI))
                {
                    return 0.75f;
                }
            }
            return 1f;
        }
        public override bool? UseItem(Item item, Player player)
        {
            //  Assigning trait effects
            if (Main.rand.NextFloat() < 0.1f)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.SUMMONER_ThrowBoulder, ClassID.Summoner, player.whoAmI))
                {
                    Projectile.NewProjectile(item.GetSource_ItemUse(item), player.position + new Vector2(Items.ArchaeaItem.StartThrowX(player)), new Vector2(2f * player.direction, -3f) * NPCs.ArchaeaNPC.AngleToSpeed(Main.MouseWorld.AngleFrom(player.Center), 4f), ProjectileID.Boulder, Main.hardMode ? 40 : 20, 5f, player.whoAmI);
                }
            }
            if (Main.rand.NextFloat() < 0.1f)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.SUMMONER_ThrowBones, ClassID.Summoner, player.whoAmI))
                {
                    Projectile.NewProjectile(item.GetSource_ItemUse(item), player.position + new Vector2(Items.ArchaeaItem.StartThrowX(player)), new Vector2(2f * player.direction, -3f) * NPCs.ArchaeaNPC.AngleToSpeed(Main.MouseWorld.AngleFrom(player.Center), 4f), ProjectileID.BoneGloveProj, Main.hardMode ? 50 : 25, 3f, player.whoAmI);
                }
            }
            if (Main.rand.NextFloat() < 0.1f)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.SUMMONER_ThrowStar, ClassID.Summoner, player.whoAmI))
                {
                    Projectile.NewProjectile(item.GetSource_ItemUse(item), player.position + new Vector2(Items.ArchaeaItem.StartThrowX(player)), new Vector2(2f * player.direction, -3f) * NPCs.ArchaeaNPC.AngleToSpeed(Main.MouseWorld.AngleFrom(player.Center), 4f), ProjectileID.StarCannonStar, Main.hardMode ? 60 : 30, 2f, player.whoAmI);
                }
            }
            if (Main.rand.NextFloat() < 0.5f)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.ALL_FallingStarAtk, ClassID.All, player.whoAmI))
                {
                    Projectile.NewProjectile(item.GetSource_ItemUse(item), Main.MouseWorld - new Vector2(0, Main.screenHeight * 2), new Vector2(Main.rand.NextFloat() - 0.5f * 2f, 8f), ProjectileID.Starfury, Main.hardMode ? 60 : 30, 2f, player.whoAmI);
                }
            }
            //  Assigning trait values
            if (ArchaeaPlayer.CheckHasTrait(TraitID.MELEE_DoubleSwing, ClassID.Melee, player.whoAmI))
            {
                timer = new Timer(Math.Max((float)item.useTime / Main.frameRate * 1000f * 0.75f, 100f));
                timer.Enabled = true;
                timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {
                    Helper.LeftMouse();
                    timer.Dispose();
                };
                timer.Start();
            }
            ArchaeaPlayer.SetClassTrait(TraitID.ALL_IncJumpHeight, ClassID.All, item.buffType == BuffID.WellFed3, player.whoAmI);
            return null;
        }
        public override void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (ArchaeaPlayer.CheckHasTrait(TraitID.MELEE_DoubleKb, ClassID.Melee, player.whoAmI))
            {
                knockBack *= 2f;
            }
            if (ArchaeaPlayer.CheckHasTrait(TraitID.MELEE_Stun, ClassID.Melee, player.whoAmI))
            {
                target.AddBuff(ModContent.BuffType<Buffs.stun>(), 90);
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.AddNPCBuff, -1, -1, null, target.whoAmI, ModContent.BuffType<Buffs.stun>(), 90);
                }
            }
        }
    }
    public class ClassTile : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            ArchaeaPlayer.SetClassTrait(TraitID.MELEE_DoubleKb, ClassID.Melee, false, item.playerIndexTheItemIsReservedFor);

            Player player = Main.player[item.playerIndexTheItemIsReservedFor];
            ArchaeaPlayer modPlayer = player.GetModPlayer<ArchaeaPlayer>();
            if (modPlayer.classChoice == ClassID.Magic)
            { 
                if (player.ZoneOverworldHeight && item.type == ItemID.MushroomGrassSeeds)
                {
                    modPlayer.TRAIT_PlantedMushroom++;
                }
                if (item.type == ItemID.MinecartTrack)
                {
                    modPlayer.TRAIT_PlacedRails++;
                }
            }
            if (modPlayer.classChoice == ClassID.Summoner)
            {
                if (item.Name.Contains("Brick"))
                {
                    modPlayer.TRAIT_PlacedBricks++;
                }
            }
            if (modPlayer.classChoice == ClassID.All)
            {
                if (item.createTile == TileID.TeleportationPylon)
                {
                    ArchaeaPlayer.SetClassTrait(TraitID.ALL_DoubleJump, ClassID.All, modPlayer.placedPylon.Count(t => t == true) >= 2, player.whoAmI);
                }
            }
        }
        public override void NearbyEffects(int i, int j, int type, bool closer)
        {
            //  Find TileID of sword shrine
            if (closer && Main.LocalPlayer.IsTileTypeInInteractionRange(type) && type == TileID.LargePiles && Main.tile[i, j].TileFrameX ==  48 * 5 && Main.tile[i, j].TileFrameY == 32)
            {
                ArchaeaPlayer.SetClassTrait(TraitID.SUMMONER_ThrowBones, ClassID.Summoner, true, Main.LocalPlayer.whoAmI);
            }
        }
    }
    public class ClassProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void AI(Projectile projectile)
        {
            if (projectile.friendly)
            { 
                if (ArchaeaPlayer.CheckHasTrait(TraitID.RANGED_Tracking, ClassID.Ranged, projectile.owner))
                {
                    NPC npc = NPCs.ArchaeaNPC.FindClosestNPC(projectile);
                    if (npc is default(NPC))
                        return;
                    else projectile.velocity.MoveTowards(npc.Center, projectile.stepSpeed);
                }
                //  Add arrow debuff signifier trait effects here
                if (ArchaeaPlayer.CheckHasTrait(TraitID.RANGED_Fire, ClassID.Ranged, projectile.owner))
                {
                    Dust.NewDust(projectile.Center, 1, 1, 6);
                }
                if (ArchaeaPlayer.CheckHasTrait(TraitID.RANGED_Ice, ClassID.Ranged, projectile.owner))
                {
                    Dust.NewDust(projectile.Center, 1, 1, DustID.Ice);
                }
                if (ArchaeaPlayer.CheckHasTrait(TraitID.RANGED_Ichor, ClassID.Ranged, projectile.owner))
                {
                    Dust.NewDust(projectile.Center, 1, 1, DustID.Ichor);
                }
                if (ArchaeaPlayer.CheckHasTrait(TraitID.RANGED_Cinnabar, ClassID.Ranged, projectile.owner))
                {
                    Dust.NewDust(projectile.Center, 1, 1, ModContent.DustType<Merged.Dusts.cinnabar_dust>());
                }
            }
        }
        public override void ModifyDamageScaling(Projectile projectile, ref float damageScale)
        {
            //  Summoner minion buff
            if (projectile.minion)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.SUMMONER_MinionDmg, ClassID.Summoner, projectile.owner))
                {
                    damageScale = 1.2f;
                }
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            //  Inflict arrow debuffs here
            if (ArchaeaPlayer.CheckHasTrait(TraitID.RANGED_Fire, ClassID.Ranged, projectile.owner))
            {
                target.AddBuff(BuffID.OnFire, 180);
                if (Main.netMode == NetmodeID.MultiplayerClient) 
                { 
                    NetMessage.SendData(MessageID.AddNPCBuff, -1, -1, null, target.whoAmI, BuffID.OnFire, 180);
                }
            }
            if (ArchaeaPlayer.CheckHasTrait(TraitID.RANGED_Ice, ClassID.Ranged, projectile.owner))
            {
                target.AddBuff(BuffID.Frostburn, 180);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.AddNPCBuff, -1, -1, null, target.whoAmI, BuffID.Frostburn, 180);
                }
            }
            if (ArchaeaPlayer.CheckHasTrait(TraitID.RANGED_Ichor, ClassID.Ranged, projectile.owner))
            {
                target.AddBuff(BuffID.Ichor, 180);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.AddNPCBuff, -1, -1, null, target.whoAmI, BuffID.Ichor, 180);
                }
            }
            if (ArchaeaPlayer.CheckHasTrait(TraitID.RANGED_Cinnabar, ClassID.Ranged, projectile.owner))
            {
                target.AddBuff(ModContent.BuffType<Buffs.mercury>(), 180);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.AddNPCBuff, -1, -1, null, target.whoAmI, ModContent.BuffType<Buffs.mercury>(), 180);
                }
            }
        }
    }
}
