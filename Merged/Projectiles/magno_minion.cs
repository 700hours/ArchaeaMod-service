using System;
using System.IO;
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
        public override void SetDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.scale = 1f;
            Projectile.damage = 0;
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

        public void Initialize()
        {
            Projectile.netUpdate = true;
            oldProj = Projectile.whoAmI;
        }
        bool init;
        bool target = false, targeted = false;
        bool flag, flag2, flag3;
        int ticks = 0;
        int Proj1;
        int npcTarget = 0, oldNpcTarget = -1;
        int oldProj;
        int Random;
        float Angle, npcAngle;
        float WaveTimer;
        float degrees = 0;
        const float radians = 0.017f;
        Vector2 orbitPosition;
        Vector2 npcCenter;
        public override void AI()
        {
            if(!init)
            {
                Initialize();
                init = true;
            }

            Player player = Main.player[Projectile.owner];

            ticks++;
            Projectile.damage = 0;

            if (player.dead || !player.HasBuff(ModContent.BuffType<Merged.Buffs.magno_summon>()))
            {
                Projectile.active = false;
            }
            if (player.HasBuff(ModContent.BuffType<Merged.Buffs.magno_summon>()))
            {
                Projectile.timeLeft = 2;
            }
            if (player.ownedProjectileCounts[Projectile.type] > player.maxMinions)
            {
                foreach(Projectile p in Main.projectile)
                {
                    if(p.type == Projectile.type && p.active)
                    {
                        p.Kill();
                        break;
                    }
                }
            }

            if (!flag3)
            {
                Random = Main.rand.Next(-24, 24);
                Projectile.netUpdate = true;
                flag3 = true;
            }
            orbitPosition = player.position + new Vector2(Random * 2f, -64f);
            Angle = (float)Math.Atan2(orbitPosition.Y - Projectile.position.Y, orbitPosition.X - Projectile.position.X);
            if (!target)
            {
                if (Vector2.Distance(orbitPosition - Projectile.position, Vector2.Zero) > 32f && Vector2.Distance(orbitPosition - Projectile.position, Vector2.Zero) <= 128f)
                {
                    Projectile.position += Distance(null, Angle, 4f);
                    Projectile.velocity = Vector2.Zero;
                }
                else if (Vector2.Distance(orbitPosition - Projectile.position, Vector2.Zero) > 128f)
                {
                    Projectile.velocity = Distance(null, Angle, 8f);
                }
                if(Vector2.Distance(orbitPosition - Projectile.position, Vector2.Zero) > 1024)
                {
                    Projectile.position = player.Center - new Vector2(0, player.height);
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
            }
            
            foreach (NPC n in Main.npc)
            {
                if (n.active && !n.friendly && !n.dontTakeDamage && !n.immortal && n.target == player.whoAmI && ((n.lifeMax >= 50 && (Main.expertMode || Main.hardMode)) || (n.lifeMax >= 15 && !Main.expertMode && !Main.hardMode)))
                {
                    bool conditions = n.life <= 0 || !targeted || npcTarget != n.whoAmI;
                    if (conditions && n.Hitbox.Contains(Main.MouseWorld.ToPoint()) && Main.mouseRight)
                    {
                        oldNpcTarget = npcTarget;
                        npcTarget = n.whoAmI;
                        Projectile.netUpdate = true;
                        targeted = true;
                        break;
                    }
                    npcCenter = new Vector2(n.position.X + n.width / 2, n.position.Y + n.height / 2);
                    if (conditions && Vector2.Distance(npcCenter - Projectile.position, Vector2.Zero) < 800f)
                    {
                        oldNpcTarget = npcTarget;
                        npcTarget = n.whoAmI;
                        Projectile.netUpdate = true;
                        targeted = true;
                        break;
                    }
                }
                else
                {
                    Projectile.spriteDirection = player.direction * -1;
                    Projectile.rotation = 0;
                    break;
                }
            }
            if (targeted)
            {
                NPC n = Main.npc[npcTarget];
                npcCenter = new Vector2(n.position.X + n.width / 2, n.position.Y + n.height / 2);
                npcAngle = (float)Math.Atan2(npcCenter.Y - Projectile.position.Y, npcCenter.X - Projectile.position.X);
                //  projectile.rotation = npcAngle;
                if (Projectile.Hitbox.Intersects(n.Hitbox))
                    Projectile.spriteDirection = n.spriteDirection;
                if (Vector2.Distance(npcCenter - Projectile.position, Vector2.Zero) < 800f)
                {
                    if (!Projectile.Hitbox.Intersects(n.Hitbox))
                    {
                        if (!flag2)
                        {
                            Projectile.position += Distance(null, npcAngle, 16f);
                            Projectile.netUpdate = true;
                        }
                    }
                    else 
                    {
                        /*  float radius = 32f;
                            degrees += radians * 9f;
                            projectile.position.X = n.Center.X + (float)(radius * Math.Cos(degrees));
                            projectile.position.Y = n.Center.Y + (float)(radius * Math.Sin(degrees));
                        */
                        flag2 = true;
                        Projectile.position = n.position;
                        if (ticks % 120 == 0)
                        {
                            for (float k = 0; k < MathHelper.ToRadians(360); k += 0.017f * 9)
                            {
                                int d = Dust.NewDust(Projectile.position + new Vector2(Projectile.width / 2, Projectile.height / 2), 4, 4, 6, Distance(null, k, 2f).X, Distance(null, k, 8f).Y, 0, default(Color), 2f);
                                Main.dust[d].noGravity = true;
                            }
                            Main.npc[npcTarget].StrikeNPC((int)(24f * player.GetDamage(DamageClass.Summon).Flat), 4f, 0);
                            int Proj2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<magno_minionexplosion>(), 0, 0f, Projectile.owner, 0f, 0f);
                            Main.projectile[Proj2].position = Projectile.position - new Vector2(15, 15);
                            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                            Projectile.netUpdate = true;
                        }
                    }
                    target = true;
                }
                else target = false;
                if (!n.active || oldNpcTarget != n.whoAmI)
                {
                    flag2 = false;
                    target = false;
                    targeted = false;
                }
            }
            if (Projectile.velocity.X <= Projectile.oldVelocity.X || Projectile.velocity.X > Projectile.oldVelocity.X || Projectile.velocity.Y <= Projectile.oldVelocity.Y || Projectile.velocity.Y > Projectile.oldVelocity.Y)
                Projectile.netUpdate = true;
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
    }
}
