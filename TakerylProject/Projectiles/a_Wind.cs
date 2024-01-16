using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.TakerylProject.Projectiles
{
	public class a_Wind : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Wind");
		}
		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
            Projectile.alpha = 255;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 300;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.scale = 1f;
		}
        private bool[] beenHit = new bool[256];
        private bool[] npcHit = new bool[Main.npc.Length];
        private int Direction()
        {
            return Projectile.oldPosition.X < Projectile.position.X ? 1 : -1;
        }
        public override void AI()
        {
            Dust.NewDust(Projectile.position, 1, 1, DustID.Smoke, Projectile.velocity.X / 1.2f, Projectile.velocity.Y / 1.2f);
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];
                if (!npcHit[i] && npc.active && !npc.friendly && !npcHit[npc.whoAmI] && npc.life > 0 && !npc.boss)
                {
                    if (npc.Center.Distance(Projectile.Center) <= 10f)
                    { 
                        if (Direction() == -1 && npc.Center.X < Projectile.Center.X)
                        { 
                            npcHit[i] = true;
                            npc.velocity.X += -6f;
                            npc.velocity.Y = -6f;
                            npc.netUpdate = true;
                        }
                        else if (Direction() == 1 && npc.Center.X > Projectile.Center.X)
                        {
                            npcHit[i] = true;
                            npc.velocity.X += 6f;
                            npc.velocity.Y = -6f;
                            npc.netUpdate = true;
                        }
                    }
                }
            }
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (!beenHit[i] && Main.player[i].active && i != Projectile.owner && Main.player[i].hostile && Main.player[i].team != Main.player[Projectile.owner].team && !Main.player[i].dead)
                {
                    if (Main.player[i].Center.Distance(Projectile.Center) <= 10f)
                    {
                        if (Direction() == -1 && Main.player[i].Center.X < Projectile.Center.X)
                        {
                            npcHit[i] = true;
                            Main.player[i].velocity *= 6f * -1;
                        }
                        else if (Direction() == 1 && Main.player[i].Center.X > Projectile.Center.X)
                        {
                            npcHit[i] = true;
                            Main.player[i].velocity *= 6f;
                        }
                    }
                }
            }
        }
    }
}