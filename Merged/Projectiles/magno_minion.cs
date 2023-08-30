using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Projectiles
{
    public class magno_minion : ModProjectile
    {
        bool fadeOutFlag => Projectile.localAI[1] == -100f ? true : false;
        int damage => (int)Projectile.localAI[0];
        int leaderIndex
        {
            get { return (int)Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
        public override bool MinionContactDamage() => false;

        public override void SetDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.scale = 1f;
            //Projectile.damage = 8;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 18000;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.netImportant = true;
        }
        bool init;
        bool targeted => !targetCheck;
        bool flag, flag2, flag3;
        bool resetIdleRandom = false;
        int ticks = 0;
        int Proj1;
        int npcTarget
        {
            get { return (int)Projectile.ai[1]; }
            set { Projectile.ai[1] = value; }
        }
        int oldNpcTarget = -1;
        int oldProj;
        int Random;
        float Angle, npcAngle;
        float WaveTimer;
        float degrees = 0;
        const float radians = 0.017f;
        Vector2 orbitPosition;
        Vector2 npcCenter;
        NPC target => Main.npc[npcTarget];
        NPC leader => Main.npc[leaderIndex];
        Player owner => Main.player[Projectile.owner];
        bool targetCheck => IsNotValidNPC(target) || oldNpcTarget == -1;

        private bool IsNotValidNPC(NPC n)
        {
            return !n.active || n.life <= 0 || n.friendly || n.CountsAsACritter || n.Center.Distance(owner.Center) >= Main.screenHeight / 2f;
        }
        private bool GetTarget()
        {
            var n = Main.npc.OrderBy(t => t.Distance(owner.Center)).FirstOrDefault(t => !IsNotValidNPC(t) && t.Distance(owner.Center) < Main.screenHeight / 2);
            if (n != default && targetCheck)
            {
               SetTarget(n);
               return true;
            }
            return false;
        }
        private bool GetTarget(NPC leader)
        {
            var n = Main.npc.OrderBy(t => t.Distance(leader.Center)).FirstOrDefault(t => !IsNotValidNPC(t) && t.Distance(leader.Center) < Main.screenHeight / 2);
            if (n != default && targetCheck)
            {
                SetTarget(n);
                return true;
            }
            return false;
        }
        private void SetTarget(NPC n)
        {
            oldNpcTarget = npcTarget;
            npcTarget = n.whoAmI;
        }
        private void ResetToIdle()
        { 
            Projectile.spriteDirection = owner.direction * -1;
            Projectile.rotation = 0; 
        }
        public void Initialize(bool flag = false)
        {
            if (!init || flag)
            {
                //  Add npc buff
                if (leader.TypeName == "Mechanic")
                {
                    leader.AddBuff(ModContent.BuffType<Merged.Buffs.magno_summon>(), 18000);
                }
                Random = Main.rand.Next(-24, 24);
                oldProj = Projectile.whoAmI;
                if (fadeOutFlag)
                {
                    Projectile.alpha = 0;
                }
                Projectile.netUpdate = true;
                init = true;
            }
        }
        private void CheckDespawn(bool flag = false)
        {
            if (fadeOutFlag)
            {
                Projectile.position = target.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
                Projectile.scale += 0.02f;
                if ((Projectile.alpha += 2) >= 255)
                {
                    Projectile.timeLeft = 0;
                }
            }
            if (owner.dead || !owner.HasBuff(ModContent.BuffType<Merged.Buffs.magno_summon>()))
            {
                Projectile.localAI[1] = -100f;
            }
            if (flag)
            {
                Projectile.timeLeft = 0;
            }
            if (owner.ownedProjectileCounts[Projectile.type] > owner.maxMinions ||
                owner.numMinions > owner.maxMinions)
            {
                foreach (Projectile p in Main.projectile)
                {
                    if (p.type == Projectile.type && p.active)
                    {
                        p.timeLeft = 0;
                        break;
                    }
                }
            }
        }
        private void CheckDespawn(NPC leader, bool flag = false)
        {
            if (fadeOutFlag)
            {
                Projectile.position = target.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
                Projectile.scale += 0.02f;
                if ((Projectile.alpha += 2) >= 255)
                {
                    Projectile.timeLeft = 0;
                }
            }
            if (!leader.active || leader.life <= 0 || !leader.HasBuff(ModContent.BuffType<Merged.Buffs.magno_summon>()))
            {
                Projectile.localAI[1] = -100f;
            }
            if (flag)
            {
                Projectile.timeLeft = 0;
            }
            /*
            if (leader.ownedProjectileCounts[Projectile.type] > owner.maxMinions ||
                owner.numMinions > owner.maxMinions)
            {
                foreach (Projectile p in Main.projectile)
                {
                    if (p.type == Projectile.type && p.active)
                    {
                        p.timeLeft = 0;
                        break;
                    }
                }
            }*/
        }
        private void ForceKeepActive()
        {
            if (owner.HasBuff(ModContent.BuffType<Merged.Buffs.magno_summon>()))
            {
                Projectile.timeLeft = 2;
            }
        }
        private void ForceKeepActive(NPC leader)
        {
            if (leader.HasBuff(ModContent.BuffType<Merged.Buffs.magno_summon>()))
            {
                Projectile.timeLeft = 2;
            }
        }
        private void CheckResetDistance()
        {
            if (Vector2.Distance(orbitPosition - Projectile.position, Vector2.Zero) > Main.screenWidth)
            {
                Projectile.position = owner.Center - new Vector2(0, owner.height);
            }
        }
        private void CheckResetDistance(NPC leader)
        {
            if (Vector2.Distance(orbitPosition - Projectile.position, Vector2.Zero) > Main.screenWidth)
            {
                Projectile.position = leader.Center - new Vector2(0, leader.height);
            }
        }
        private new bool CanDamage()
        {
            return !Main.npc[npcTarget].friendly && Main.npc[npcTarget].type != NPCID.FairyCritterPink && Main.npc[npcTarget].type != NPCID.FairyCritterGreen && Main.npc[npcTarget].type != NPCID.FairyCritterBlue && !Main.npc[npcTarget].CountsAsACritter && !Main.npc[npcTarget].townNPC;
        }

        public override bool PreAI()
        {
            Initialize(); 
            if (ticks++ > 1000)
            {
                ticks = 0;
            }
            NPC t = Main.npc.FirstOrDefault(_t => _t.Hitbox.Contains(Main.MouseWorld.ToPoint()));
            if (t != default && Main.mouseRight)
            {
                SetTarget(t);
            }
            return true;
        }
        public override void AI()
        {
            if (leader.TypeName == "Mechanic")
            {
                AI(leader);
                return;
            }
            Player player = owner;
            ForceKeepActive(); 
            CheckDespawn();
            CheckResetDistance();
            GetTarget();

            if (Projectile.velocity.X <= Projectile.oldVelocity.X || Projectile.velocity.X > Projectile.oldVelocity.X || Projectile.velocity.Y <= Projectile.oldVelocity.Y || Projectile.velocity.Y > Projectile.oldVelocity.Y)
            {
                Projectile.netUpdate = true;
            }

            orbitPosition = player.position + new Vector2(Random * 2f, -64f);
            Angle = (float)Math.Atan2(orbitPosition.Y - Projectile.position.Y, orbitPosition.X - Projectile.position.X);
            if (IsNotValidNPC(target))
            {
                ResetToIdle();
                if (Vector2.Distance(orbitPosition - Projectile.position, Vector2.Zero) > 32f && Vector2.Distance(orbitPosition - Projectile.position, Vector2.Zero) <= 128f)
                {
                    Projectile.position += Distance(null, Angle, 4f);
                    Projectile.velocity = Vector2.Zero;
                }
                else if (Vector2.Distance(orbitPosition - Projectile.position, Vector2.Zero) > 128f)
                {
                    Projectile.velocity = Distance(null, Angle, 8f);
                }
                #region float
                float Revolution = 6.28308f;
                float WavesPerSecond = 1.0f;
                float Time = 1.0f / Main.frameRate;
                WaveTimer += Time * Revolution * WavesPerSecond;
                float Cos = (float)Math.Cos(180);
                float WaveOffset = (float)Math.Sin(WaveTimer) * 5f;

                Projectile.position.Y += Cos * WaveOffset;
                #endregion
                return;
            }
            NPC n = target;
            npcCenter = new Vector2(n.position.X + n.width / 2, n.position.Y + n.height / 2);
            npcAngle = (float)Math.Atan2(npcCenter.Y - Projectile.Center.Y, npcCenter.X - Projectile.Center.X);
            if (Projectile.Hitbox.Intersects(n.Hitbox))
            { 
                Projectile.spriteDirection = n.spriteDirection;
            }
            if (Vector2.Distance(npcCenter - owner.Center, Vector2.Zero) < Main.screenHeight / 2)
            {
                if (!n.Hitbox.Contains(Projectile.Center.ToPoint()))
                {
                    Projectile.velocity = Distance(null, npcAngle, 8f);
                }
                else 
                {
                    Projectile.Center = n.Hitbox.Center();
                    if (ticks % 120 == 0)
                    {
                        if (CanDamage())
                        {
                            for (float k = 0; k < MathHelper.ToRadians(360); k += 0.017f * 9)
                            {
                                int d = Dust.NewDust(Projectile.position + new Vector2(Projectile.width / 2, Projectile.height / 2), 4, 4, 6, Distance(null, k, 2f).X, Distance(null, k, 8f).Y, 0, default(Color), 2f);
                                Main.dust[d].noGravity = true;
                            }
                            ArchaeaNPC.StrikeNPC(Main.npc[npcTarget], (int)(damage * player.GetDamage(DamageClass.Summon).Additive), 4f, 0, false);
                            int Proj2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<magno_minionexplosion>(), 0, 0f, Projectile.owner, 0f, 0f);
                            Main.projectile[Proj2].position = Projectile.position - new Vector2(15, 15);
                            Main.projectile[Proj2].minion = true;
                            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                            Projectile.netUpdate = true;
                        }
                    }
                }
            }
        }
        public void AI(NPC leader)
        {
            ForceKeepActive(leader);
            CheckDespawn(leader);
            CheckResetDistance(leader);
            GetTarget(leader);

            if (Projectile.velocity.X <= Projectile.oldVelocity.X || Projectile.velocity.X > Projectile.oldVelocity.X || Projectile.velocity.Y <= Projectile.oldVelocity.Y || Projectile.velocity.Y > Projectile.oldVelocity.Y)
            {
                Projectile.netUpdate = true;
            }

            orbitPosition = leader.position + new Vector2(Random * 2f, -64f);
            Angle = (float)Math.Atan2(orbitPosition.Y - Projectile.position.Y, orbitPosition.X - Projectile.position.X);
            if (IsNotValidNPC(target))
            {
                ResetToIdle();
                if (Vector2.Distance(orbitPosition - Projectile.position, Vector2.Zero) > 32f && Vector2.Distance(orbitPosition - Projectile.position, Vector2.Zero) <= 128f)
                {
                    Projectile.position += Distance(null, Angle, 4f);
                    Projectile.velocity = Vector2.Zero;
                }
                else if (Vector2.Distance(orbitPosition - Projectile.position, Vector2.Zero) > 128f)
                {
                    Projectile.velocity = Distance(null, Angle, 8f);
                }
                #region float
                float Revolution = 6.28308f;
                float WavesPerSecond = 1.0f;
                float Time = 1.0f / Main.frameRate;
                WaveTimer += Time * Revolution * WavesPerSecond;
                float Cos = (float)Math.Cos(180);
                float WaveOffset = (float)Math.Sin(WaveTimer) * 5f;

                Projectile.position.Y += Cos * WaveOffset;
                #endregion
                return;
            }
            NPC n = target;
            npcCenter = new Vector2(n.position.X + n.width / 2, n.position.Y + n.height / 2);
            npcAngle = (float)Math.Atan2(npcCenter.Y - Projectile.Center.Y, npcCenter.X - Projectile.Center.X);
            if (Projectile.Hitbox.Intersects(n.Hitbox))
            {
                Projectile.spriteDirection = n.spriteDirection;
            }
            if (Vector2.Distance(npcCenter - owner.Center, Vector2.Zero) < Main.screenHeight / 2)
            {
                if (!n.Hitbox.Contains(Projectile.Center.ToPoint()))
                {
                    Projectile.velocity = Distance(null, npcAngle, 8f);
                }
                else
                {
                    Projectile.Center = n.Hitbox.Center();
                    if (ticks % 120 == 0)
                    {
                        if (CanDamage())
                        {
                            for (float k = 0; k < MathHelper.ToRadians(360); k += 0.017f * 9)
                            {
                                int d = Dust.NewDust(Projectile.position + new Vector2(Projectile.width / 2, Projectile.height / 2), 4, 4, 6, Distance(null, k, 2f).X, Distance(null, k, 8f).Y, 0, default(Color), 2f);
                                Main.dust[d].noGravity = true;
                            }
                            ArchaeaNPC.StrikeNPC(Main.npc[npcTarget], damage, 4f, 0, false);
                            int Proj2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<magno_minionexplosion>(), 0, 0f, Projectile.owner, 0f, 0f);
                            Main.projectile[Proj2].position = Projectile.position - new Vector2(15, 15);
                            Main.projectile[Proj2].minion = true;
                            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                            Projectile.netUpdate = true;
                        }
                    }
                }
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }
        public Vector2 DistanceV2(Player player, float Angle, float Radius)
        {
            float VelocityX = Radius + (float)Math.Cos(Angle);
            float VelocityY = Radius + (float)Math.Sin(Angle);

            return new Vector2(VelocityX, VelocityY);
        }
    }
}
