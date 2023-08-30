using System;
using System.IO;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using tModPorter;

namespace ArchaeaMod.Projectiles
{
    public class ArchaeaProjectiles
    {
        public static void CircleEffect(Projectile projectile, ref float distance, float maxDistance, float growthRate, int alpha = 255, int width = 1, int height = 1, short dustID = 6)
        {
            projectile.alpha = alpha;
            distance += growthRate;
            for (float n = 0f; n < MathHelper.ToRadians(360f); n += Draw.radian * growthRate)
            {
                Vector2 c = NPCs.ArchaeaNPC.AngleBased(new Vector2(projectile.ai[0], projectile.ai[1]), n, distance);
                var speed = NPCs.ArchaeaNPC.AngleToSpeed(NPCs.ArchaeaNPC.AngleTo(new Vector2(projectile.ai[0], projectile.ai[1]), c), 2f);
                var dust = Dust.NewDust(c, width, height, dustID, speed.X, speed.Y);
                Main.dust[dust].noGravity = true;
            }
            if (distance > maxDistance)
            {
                projectile.Kill();
            }
        }
        public static void Explode(Projectile projectile, int type, int size, int damage, float knockBack, bool hurtNpc = true, int debuffType = -1, int debuffTime = 300, bool debuff = false, int count = 20)
        {
            int num = size * 3;
            SoundEngine.PlaySound(SoundID.Item14, projectile.Center);
            for (int i = 0; i < count; i++)
            {
                Dust.NewDust(projectile.Center - new Vector2(num / 2, num / 2), num, num, type, 0, 0, 0, default, 2);
            }
            if (hurtNpc)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc.Distance(projectile.Center) < num)
                    {
                        ArchaeaNPC.StrikeNPC(npc, damage, knockBack, npc.position.X < projectile.position.X ? -1 : 1, Main.rand.NextBool());
                        NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, damage, knockBack, 0, 0);
                        if (debuff)
                        {
                            npc.AddBuff(debuffType, debuffTime);
                            NetMessage.SendData(MessageID.AddNPCBuff, -1, -1, null, npc.whoAmI, debuffType, debuffTime);
                        }
                    }
                }
            }
        }
        public static void Vector8(NPC npc, int projType, int npcOtherIndex, bool flag, ref int TimerAI)
        {
            if (!flag && TimerAI == 20)
            {
                float num48 = 8f;
                Vector2 vector8 = new Vector2((npc.position.X - 50 + (npc.width * 0.5f)) * npc.spriteDirection, npc.position.Y - 10 + (npc.height / 2));
                int damage = 30;
                int type = projType;
                float rotation = (float)Math.Atan2(vector8.Y - (Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)), vector8.X - (Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)));
                rotation += -0.6f + (Main.rand.Next(10) / 100);
                int num54 = Projectile.NewProjectile(Projectile.GetSource_None(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * num48) * -1), (float)((Math.Sin(rotation) * num48) * -1), type, damage, 0f, Main.myPlayer);
                Main.projectile[num54].timeLeft = 300;
            }
            if (!flag && TimerAI == 35)
            {
                float num48 = 8f;
                if (!Main.npc[npcOtherIndex].active) num48 = 12f;
                Vector2 vector8 = new Vector2((npc.position.X - 50 + (npc.width * 0.5f)) * npc.spriteDirection, npc.position.Y - 10 + (npc.height / 2));
                int damage = 30;
                int type = projType;
                float rotation = (float)Math.Atan2(vector8.Y - (Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)), vector8.X - (Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)));
                rotation += -0.3f + (Main.rand.Next(10) / 100);
                int num54 = Projectile.NewProjectile(Projectile.GetSource_None(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * num48) * -1), (float)((Math.Sin(rotation) * num48) * -1), type, damage, 0f, Main.myPlayer);
                Main.projectile[num54].timeLeft = 300;
            }
            if (!flag && TimerAI == 50)
            {
                float num48 = 8f;
                if (!Main.npc[npcOtherIndex].active) num48 = 12f;
                Vector2 vector8 = new Vector2((npc.position.X - 50 + (npc.width * 0.5f)) * npc.spriteDirection, npc.position.Y - 10 + (npc.height / 2));
                int damage = 30;
                int type = projType;
                float rotation = (float)Math.Atan2(vector8.Y - (Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)), vector8.X - (Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)));
                rotation += +(Main.rand.Next(10) / 100);
                int num54 = Projectile.NewProjectile(Projectile.GetSource_None(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * num48) * -1), (float)((Math.Sin(rotation) * num48) * -1), type, damage, 0f, Main.myPlayer);
                Main.projectile[num54].timeLeft = 300;
            }
            if (!flag && TimerAI == 65)
            {
                float num48 = 8f;
                if (!Main.npc[npcOtherIndex].active) num48 = 12f;
                Vector2 vector8 = new Vector2((npc.position.X - 50 + (npc.width * 0.5f)) * npc.spriteDirection, npc.position.Y - 10 + (npc.height / 2));
                int damage = 30;
                int type = projType;
                float rotation = (float)Math.Atan2(vector8.Y - (Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)), vector8.X - (Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)));
                rotation += +0.3f + (Main.rand.Next(10) / 100);
                int num54 = Projectile.NewProjectile(Projectile.GetSource_None(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * num48) * -1), (float)((Math.Sin(rotation) * num48) * -1), type, damage, 0f, Main.myPlayer);
                Main.projectile[num54].timeLeft = 300;
            }
            if (!flag && TimerAI >= 80)
            {
                float num48 = 8f;
                if (!Main.npc[npcOtherIndex].active) num48 = 12f;
                Vector2 vector8 = new Vector2((npc.position.X - 50 + (npc.width * 0.5f)) * npc.spriteDirection, npc.position.Y - 10 + (npc.height / 2));
                int damage = 30;
                int type = projType;
                float rotation = (float)Math.Atan2(vector8.Y - (Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)), vector8.X - (Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)));
                rotation += +0.6f + (Main.rand.Next(10) / 100);
                int num54 = Projectile.NewProjectile(Projectile.GetSource_None(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * num48) * -1), (float)((Math.Sin(rotation) * num48) * -1), type, damage, 0f, Main.myPlayer);
                Main.projectile[num54].timeLeft = 300;
                TimerAI = 0;
            }
        }
        public static bool IsNotOldPosition(Projectile proj)
        {
            return proj.position.X < 0f && proj.oldPosition.X >= 0f || proj.position.X > 0f && proj.oldPosition.X <= 0f || proj.position.Y < 0f && proj.oldPosition.Y >= 0f || proj.position.Y > 0f && proj.oldPosition.Y <= 0f;
        }
    }
    public class MathProjectile
    {
        // first time run
        bool Initialised = false;

        // vectors for start and end point of projectile
        Microsoft.Xna.Framework.Vector2 Start;
        Microsoft.Xna.Framework.Vector2 End;

        // current point between start and end (0.0f = start, 1.0f = end)
        float CurrentPoint = 0.0f;

        // how long the projectile lasts
        float MaxTime = 0.0f;

        // sin and cosine of the angle the projectile will travel at
        float Cos = 0.0f;
        float Sin = 0.0f;

        // timer for moving the projectile in a wave pattern
        float WaveTimer = 0.0f;

        // the amplitude of the wave
        float Offset = 15.0f;

        // 360 degrees in radians
        float Revolution = 6.28308f;

        // speed the projectile travels at
        float Speed = 17.0f;

        // how many waves are completed per second
        float WavesPerSecond = 2.0f;

        // timer for creating dust
        float DustTimer = 0.0f;

        // delay for creating dust
        float DustDelay = 0.1f;

        // first time run
        private void Initialise(Projectile projectile)
        {
            Initialised = true;

            // get initial time left
            MaxTime = projectile.timeLeft;

            // passed in from the item cs file
            float Angle = projectile.ai[0];

            // get cosine and sine of angle
            Cos = (float)Math.Cos(Angle);
            Sin = (float)Math.Sin(Angle);

            // centre the projectile on the player
            float PlayerHalfWidth = Main.player[(int)projectile.ai[1]].width * 0.5f;
            float PlayerHalfHeight = Main.player[(int)projectile.ai[1]].height * 0.5f;

            // set start position
            Start.X = projectile.position.X + Cos * PlayerHalfWidth;
            Start.Y = projectile.position.Y + Sin * PlayerHalfHeight;

            // set end position
            End.X = Start.X + Cos * MaxTime * Speed;
            End.Y = Start.Y + Sin * MaxTime * Speed;
        }

        public void AI(Projectile projectile)
        {
            // do once
            if (!Initialised)
            {
                Initialise(projectile);
            }

            // do terraria's base projectile ai
            projectile.AI();

            // get time between updates
            float Time = 1.0f / Main.frameRate;

            // increase wave timer
            WaveTimer += Time * Revolution * WavesPerSecond;

            // keep to a simple value
            if (WaveTimer >= Revolution)
            {
                WaveTimer -= Revolution;
            }

            // get current point along line from start to end
            CurrentPoint = (MaxTime - projectile.timeLeft) / MaxTime;

            // set position to the point on the line
            projectile.position = Microsoft.Xna.Framework.Vector2.Lerp(Start, End, CurrentPoint);

            float WaveOffset = (float)Math.Sin(WaveTimer) * Offset;

            // add wave offset
            projectile.position.X -= Sin * WaveOffset;
            projectile.position.Y += Cos * WaveOffset;

            DustTimer += Time;

            // create dust
            if (DustTimer >= DustDelay)
            {
                DustTimer -= DustDelay;
                Color color = new Color();
                int dust = Dust.NewDust(new Vector2((float)projectile.position.X, (float)projectile.position.Y), projectile.width, projectile.height, 59, 0, 0, 100, color, 2.0f);
                Main.dust[dust].noGravity = true;
            }
            foreach (NPC N in Main.npc)
            {
                if (!N.active) continue;
                if (N.life <= 0) continue;
                if (N.friendly) continue;
                if (N.dontTakeDamage) continue;
                if (N.type == 143 || N.type == 144 || N.type == 145 || N.type == 146) continue;
                Rectangle MB = new Rectangle((int)projectile.position.X + (int)projectile.velocity.X, (int)projectile.position.Y + (int)projectile.velocity.Y, projectile.width, projectile.height);
                Rectangle NB = new Rectangle((int)N.position.X, (int)N.position.Y, N.width, N.height);
                if (MB.Intersects(NB))
                {
                    N.AddBuff(ModContent.BuffType<Buffs.frozen>(), 600, false);
                }
            }
        }
    }
}
