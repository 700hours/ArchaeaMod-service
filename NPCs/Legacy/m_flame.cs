using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class m_flame : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Flame");
        }
        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 48;
            NPC.friendly = false;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.damage = 15;
            NPC.defense = 0;
            NPC.lifeMax = 20;
        //  NPC.HitSound = SoundID.NPCHit1;
        //  NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
        }

        bool init = false;
        public void Initialize()
        {
            degrees = NPC.ai[1];
        }
        float radius = 180;
        float degrees = 0.017f;
        Vector2 center;
        const float radians = 0.017f;
        public override void AI()
        {
            if (!init)
            {
                Initialize();
                init = true;
            }
            NPC.color = Color.White;

            NPC.TargetClosest(true);

            Player player = Main.player[NPC.target];

            degrees += radians * 3.2f;
            radius -= 0.5f;

            center = player.position;
            NPC.position.X = center.X + (float)(radius * Math.Cos(degrees));
            NPC.position.Y = center.Y + (float)(radius * Math.Sin(degrees));

            if (radius < 1f)
                NPC.active = false;
            
            for (int k = 0; k < 2; k++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, 170, 0f, 0f, 100, default(Color), 1.2f);
                Main.dust[d].noGravity = true;
            }
        }
    }
}
