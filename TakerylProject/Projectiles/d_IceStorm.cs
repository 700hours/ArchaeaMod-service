using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.TakerylProject.Projectiles
{
	public class d_IceStorm : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Ice Storm");
		}
		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
            Projectile.alpha = 255;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.scale = 1f;
		}
        private float dist = 45f;
        private Vector2[] storm = new Vector2[30];
        private bool[] dust = new bool[30];
        private int[] dustID = new int[30];
        private bool[] npcHit = new bool[Main.npc.Length];
        private bool[] beenHit = new bool[256];
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.position = Main.player[Projectile.owner].Center;
            float dist = 300f;
            for (int i = 0; i < storm.Length; i++)
            {
                if (storm[i] == null || storm[i] == Vector2.Zero)
                    storm[i] = new Vector2(owner.position.X - dist - dist * Main.rand.NextFloat(),  owner.position.Y - dist - dist * Main.rand.NextFloat());
                storm[i].X += 8f;
                storm[i].Y += 8f;
                if (owner.Distance(storm[i]) <= dist) 
                {
                    dust[i] = true;
                }
                else if (storm[i].X > owner.position.X || storm[i].Y > owner.position.Y)
                {
                    dust[i] = false;
                    storm[i] = Vector2.Zero;
                }
            }
            for (int j = 0; j < dust.Length; j++)
            {
                dustID[j] = Dust.NewDust(storm[j], 1, 1, DustID.Ice);
                Main.dust[dustID[j]].position = storm[j];
                Main.dust[dustID[j]].noGravity = true;
                if (!dust[j])
                {
                    Main.dust[dustID[j]].active = false;
                }
            }
            foreach (NPC npc in Main.npc)
            {
                if (!npcHit[npc.whoAmI] && npc.active && !npc.friendly && npc.life > 0 && !npc.boss && npc.Distance(Projectile.Center) < dist)
                {
                    npcHit[npc.whoAmI] = true;
                    npc.AddBuff(BuffID.Frostburn, 150);
                    npc.StrikeNPC(npc.CalculateHitInfo(3, Projectile.position.X < npc.position.X ? 1 : -1, false, 0f));
                }
            }
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (!beenHit[i] && Main.player[i].active && i != Projectile.owner && Main.player[i].hostile && Main.player[i].team != Main.player[Projectile.owner].team && !Main.player[i].dead && Main.player[i].Distance(Projectile.position) < dist)
                {
                    beenHit[i] = true;
                    Main.player[i].AddBuff(BuffID.Frozen, 90);
                    Main.player[i].Hurt(PlayerDeathReason.ByPlayerItem(Projectile.owner, Main.player[Projectile.owner].HeldItem), 3, Main.player[i].position.X < Projectile.position.X ? -1 : 1, true);
                }
            }
        }
    }
}