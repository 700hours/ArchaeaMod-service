using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.TakerylProject.Projectiles
{
	public class a_Lightning : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Lightning Bolt");
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

        private bool init;
        private int next = 1, casts;
        private float weight;
        private Vector2[] connect = new Vector2[8];
        private Vector2 dest, start;
        private bool[] npcHit = new bool[Main.npc.Length];
        private bool[] beenHit = new bool[256];
        internal bool GeneratePath()
        {
            if (!init)
            {
                float height = 800f;
                dest = Projectile.position;
                Projectile.position.Y -= height;
                Projectile.position.X += Main.rand.NextFloat(-100, 100);
                for (int i = 0; i < connect.Length - 1; i++)
                {
                    connect[i] = new Vector2(dest.X + Main.rand.NextFloat(-100, 100), Projectile.position.Y + (height / connect.Length) * i);
                }
                connect[connect.Length - 1] = dest;
                init = true;
            }
            return init;
        }
        public override bool PreAI()
        {
            return GeneratePath();
        }
        public override void AI()
        {
            if (next >= connect.Length)
                return;
            if (Projectile.position.Y < connect[next].Y)
            {
                for (float i = 0; i < 1f; i += 0.025f)
                {
                    var d = Dust.NewDust(Projectile.Center, 1, 1, DustID.WitherLightning);
                    Main.dust[d].noGravity = true;
                    Projectile.position = Vector2.Lerp(connect[next - 1], connect[next], i);
                }
            }
            else if (next < connect.Length)
                next++;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];
                if (!npcHit[i] && npc.active && npc.life > 0 && !npc.friendly)
                {
                    if (Projectile.Center.Distance(npc.Center) < 80f)
                    {
                        npc.StrikeNPC(npc.CalculateHitInfo(Projectile.damage, Projectile.position.X < npc.Center.X ? 1 : -1, true));
                        npc.AddBuff(ModContent.BuffType<Buffs.stun>(), 210);
                        npcHit[i] = true;
                    }
                }
            }
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player plr = Main.player[i];
                if (!beenHit[i] && plr.active && !plr.dead && plr.InOpposingTeam(Main.player[Projectile.owner]) && plr.hostile)
                {
                    if (Projectile.Center.Distance(plr.Center) < 80f)
                    {
                        plr.Hurt(PlayerDeathReason.ByProjectile(plr.whoAmI, Projectile.whoAmI), Projectile.damage, Projectile.position.X < plr.Center.X ? 1 : -1, true);
                        plr.AddBuff(ModContent.BuffType<Buffs.stun>(), 150);
                        beenHit[i] = true;
                    }
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Webbed, 150);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {   
            target.AddBuff(ModContent.BuffType<Buffs.stun>(), 210);
        }
    }
}