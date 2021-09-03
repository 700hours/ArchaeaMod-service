using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
                        npc.StrikeNPC(damage, knockBack, npc.position.X < projectile.position.X ? -1 : 1, Main.rand.NextBool());
                        if (debuff)
                        {
                            npc.AddBuff(debuffType, debuffTime);
                        }
                    }
                }
            }
        }
    }
}
