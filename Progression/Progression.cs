using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Timers;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using tUserInterface.Extension;

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
            ALL_FallingStarAtk = 29,
            MELEE_Job = 30,
            RANGED_Job = 31,
            MAGE_Job = 32,
            SUMMONER_Job = 33,
            ALL_Job = 34;
    }
}
namespace ArchaeaMod.Progression.Global
{
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
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            deathProjType = projectile.type;
            lastHitOwner = projectile.owner;
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
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
            if (ArchaeaPlayer.CheckHasTrait(TraitID.MELEE_DoubleKb, ClassID.Melee, player.whoAmI))
            {
                hit.Knockback *= 2f;
            }
            if (ArchaeaPlayer.CheckHasTrait(TraitID.MELEE_Stun, ClassID.Melee, player.whoAmI))
            {
                npc.AddBuff(ModContent.BuffType<Buffs.stun>(), 90);
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.AddNPCBuff, -1, -1, null, npc.whoAmI, ModContent.BuffType<Buffs.stun>(), 90);
                }
            }
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
    [CloneByReference]
    public class ClassItem : GlobalItem
    {
        Timer timer;
        float rand;
        int useCount = 0;
        int count = 0;
        public override bool InstancePerEntity => true;
        //  Assigning trait effects
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.frameRate <= 0)
            {
                return false;
            }
            if (ArchaeaPlayer.CheckHasTrait(TraitID.RANGED_DoubleFire, ClassID.Ranged, player.whoAmI))
            {
                timer = new Timer(Math.Max((float)item.useTime / Math.Max(Main.frameRate, 60f) * 1000f / 2f, 100f));
                timer.AutoReset = false;
                timer.Enabled = true;
                timer.Elapsed += (object sender, ElapsedEventArgs e) => 
                {
                    timer.Stop();
                    player.ApplyItemAnimation(item, 0.5f);
                    SoundEngine.PlaySound(item.UseSound.Value, player.Center);
                    var proj = Projectile.NewProjectileDirect(Projectile.GetSource_None(), position, velocity, type, damage, knockback, player.whoAmI);
                    proj.netUpdate = true;
                    proj.friendly = true;
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
                        timer = new Timer(Math.Max((float)item.useTime / Math.Max(Main.frameRate, 60f) * 1000f / 2f, 100f));
                        timer.Enabled = true;
                        timer.AutoReset = false;
                        timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                        {
                            timer.Stop();
                            player.ApplyItemAnimation(item, 0.5f);
                            SoundEngine.PlaySound(item.UseSound.Value, player.Center);
                            var proj = Projectile.NewProjectileDirect(Projectile.GetSource_None(), position, velocity, type, damage, knockback, player.whoAmI);
                            proj.netUpdate = true;
                            proj.friendly = true;
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
                        reduce -= item.mana;
                    }
                }
            }
            if (item.DamageType == DamageClass.Summon)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.SUMMONER_NoManaCost, ClassID.Summoner, player.whoAmI))
                {
                    reduce -= item.mana;
                }
            }
        }

        //  Custom UseItem
        public void BeginUseItem(Item item, Player player)
        {
            if (useCount > 0)
                return;

            timer = new Timer(Math.Max((float)item.useTime / Math.Max(Main.frameRate, 60f) * 1000f * 0.75f + 1f, 100f));
            timer.Enabled = true;
            timer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                useCount = 0;
            };
            timer.Start();
        }
        public void EndUseItem(Item item, Player player)
        {
            if (useCount > 0)
                return;
            useCount = 1;
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
        public override void OnCreated(Item item, ItemCreationContext context)
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
            float value = 1f;
            if (item.DamageType == DamageClass.Melee)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.MELEE_DoubleSwing, ClassID.Melee, player.whoAmI))
                {
                    value = 0.75f;
                }
            }
            if (item.DamageType == DamageClass.Melee)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.MELEE_Job, ClassID.Melee, player.whoAmI))
                {
                    value *= 0.9f;
                }
            }
            if (item.DamageType == DamageClass.Magic && (rand = Main.rand.NextFloat()) < 0.5f)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.MAGE_DoubleAttack, ClassID.Magic, player.whoAmI))
                {
                    return 0.75f;
                }
            }
            return value;
        }
        public override bool? UseItem(Item item, Player player)
        {
            BeginUseItem(item, player);
            //  Assigning trait effects
            bool notTile = item.damage > 0 && !item.noMelee;
            if (notTile && Main.rand.NextFloat() < 0.01f)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.SUMMONER_ThrowBoulder, ClassID.Summoner, player.whoAmI))
                {
                    Projectile.NewProjectile(item.GetSource_FromThis(), player.position, new Vector2(2f, 3f) * NPCs.ArchaeaNPC.AngleToSpeed(Main.MouseWorld.AngleFrom(player.Center), 4f), ProjectileID.Boulder, Main.hardMode ? 20 : 10, 5f, player.whoAmI);
                }
            }
            if (notTile && Main.rand.NextFloat() < 0.01f)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.SUMMONER_ThrowBones, ClassID.Summoner, player.whoAmI))
                {
                    Projectile.NewProjectile(item.GetSource_FromThis(), player.position, new Vector2(2f, 3f) * NPCs.ArchaeaNPC.AngleToSpeed(Main.MouseWorld.AngleFrom(player.Center), 4f), ProjectileID.BoneGloveProj, Main.hardMode ? 25 : 12, 3f, player.whoAmI);
                }
            }
            if (notTile && Main.rand.NextFloat() < 0.01f)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.SUMMONER_ThrowStar, ClassID.Summoner, player.whoAmI))
                {
                    Projectile.NewProjectile(item.GetSource_FromThis(), player.position, new Vector2(2f, 3f) * NPCs.ArchaeaNPC.AngleToSpeed(Main.MouseWorld.AngleFrom(player.Center), 4f), ProjectileID.StarCannonStar, Main.hardMode ? 30 : 15, 2f, player.whoAmI);
                }
            }
            if (notTile && Main.rand.NextFloat() < 0.5f)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.ALL_FallingStarAtk, ClassID.All, player.whoAmI))
                {
                    Projectile.NewProjectile(item.GetSource_FromThis(), Main.MouseWorld - new Vector2(0, Main.screenHeight * 2), new Vector2(Main.rand.NextFloat() - 0.5f * 2f, 8f), ProjectileID.Starfury, Main.hardMode ? 60 : 30, 2f, player.whoAmI);
                }
            }
            //  Assigning trait values
            ArchaeaPlayer.SetClassTrait(TraitID.ALL_IncJumpHeight, ClassID.All, item.buffType == BuffID.WellFed3, player.whoAmI);
            EndUseItem(item, player);
            return null;
        }
    }
    public class ClassTile : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
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
                if (item.Name.ToLower().Contains("brick"))
                {
                    modPlayer.TRAIT_PlacedBricks++;
                }
            }
            if (modPlayer.classChoice == ClassID.Melee)
            {
                modPlayer.placedTiles++;
            }
            if (modPlayer.classChoice == ClassID.All)
            {
                if (item.createTile == TileID.TeleportationPylon)
                {
                    int frame = Main.tile[i, j].TileFrameX / 18;
                    modPlayer.placedPylon[frame - 1] = true;
                    ArchaeaPlayer.SetClassTrait(TraitID.ALL_DoubleJump, ClassID.All, modPlayer.placedPylon.Count(t => t == true) >= 2, player.whoAmI);
                }
            }
        }
        public override void NearbyEffects(int i, int j, int type, bool closer)
        {
            //  Find TileID of sword shrine
            if (type == TileID.LargePiles2)
            {
                if (Main.tile[i, j].TileFrameX == 918 && Main.tile[i, j].TileFrameY == 0)
                {
                    if (Main.LocalPlayer.IsTileTypeInInteractionRange(type, TileReachCheckSettings.Simple))
                    { 
                        ArchaeaPlayer.SetClassTrait(TraitID.SUMMONER_ThrowBones, ClassID.Summoner, true, Main.LocalPlayer.whoAmI);
                    }
                }
            }
        }
    }
    public class ClassProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void AI(Projectile projectile)
        {
            if (projectile.friendly && projectile.active)
            { 
                if (ArchaeaPlayer.CheckHasTrait(TraitID.RANGED_Tracking, ClassID.Ranged, projectile.owner))
                {
                    NPC npc = Main.npc.Where(t => t.active && !t.friendly).OrderBy(t => t.Center.Distance(projectile.Center)).FirstOrDefault();
                    //direction = projectile.Center.X < npc.Center.X ? 1 : -1;
                    if (npc is default(NPC))
                        return;
                    else 
                    {
                    }
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
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            //  Summoner minion buff
            if (projectile.minion)
            {
                if (ArchaeaPlayer.CheckHasTrait(TraitID.SUMMONER_MinionDmg, ClassID.Summoner, projectile.owner))
                {
                    modifiers.FinalDamage *= 1.2f;
                }
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
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
