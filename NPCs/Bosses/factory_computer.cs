using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ArchaeaMod.Items;
using ArchaeaMod.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs.Bosses
{
    internal class factory_computer : ModNPC
    {
        public override string Texture => "ArchaeaMod/Gores/arrow";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Factory Computer");
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public override void SetDefaults()
        {
            NPC.width = 128;
            NPC.height = 128;
            NPC.aiStyle = -1;
            NPC.behindTiles = true;
            NPC.boss = true;
            NPC.damage = 30;
            NPC.defense = 12;
            NPC.knockBackResist = 1;
            NPC.lavaImmune = true;
            NPC.lifeMax = 10000;
            NPC.npcSlots = 10f;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.value = 50000;     
        }
        AIStyle ai
        {
            get { return (AIStyle)(int)NPC.ai[0]; }
            set { NPC.ai[0] = (int)value; }
        }
        int ticks
        {
            get { return (int)NPC.ai[1]; }
            set { NPC.ai[1] = value; }
        }
        float rotation
        {
            get { return NPC.ai[2]; }
            set { NPC.ai[2] = value; }
        }
        float divisor => NPC.life < NPC.lifeMax / 2 ? 1.5f : 1f;
        const float Speed = 3f;
        const float Radius = 800f;
        Player target => Main.player[NPC.target];
        
        public override void AI()
        {
            ai = AIStyle.ElectricArc;
            NPC.velocity.X = 0f;
            if (ai > 0)
            {
                ticks++;
            }
            switch (ai)
            {
                case AIStyle.JustSpawned:
                    NPC.direction = NPC.position.X / 16 < Main.maxTilesX / 2 ? 1 : -1;
                    if (ArchaeaItem.Elapsed(ref NPC.ai[3], 600))
                    {
                        NPC.ai[3] = 0f;
                        ai = AIStyle.Start;
                        NPC.netUpdate = true;
                    }
                    break;
                case AIStyle.Start:
                    if (ticks % 20 == 0)
                    {
                        rotation += 22.5f / 2;
                        rotation = MathHelper.WrapAngle(rotation);
                        int proj = Projectile.NewProjectile(Projectile.GetSource_None(), NPC.Center, ArchaeaNPC.AngleBased(rotation, Speed), ProjectileID.RocketI, 50, 3f, Main.myPlayer);
                        Main.projectile[proj].timeLeft = 180;
                    }
                    if (ticks > 180)
                    {
                        ai = AIStyle.ElectricArc;
                        ticks = 0;
                        NPC.netUpdate = true;
                    } 
                    break;
                case AIStyle.ElectricArc:
                    for (int i = 0; i < 360; i += 45)
                    { 
                        Vector2 start = NPC.Center;
                        Vector2 end = start + new Vector2(0, Radius);
                        ArchaeaItem.Bolt(ref start, end, i + (rotation += Draw.radian * 5f), 20, 8, -100F, 0.5f);
                    }
                    if (ticks > 300)
                    {
                        ai = AIStyle.BurningSteam;
                        rotation = 0f;
                        ticks = 0;
                        NPC.ai[3] = 0f;
                        NPC.netUpdate = true;
                        rotation = NPC.Center.AngleTo(target.position);
                    }
                    break;
                case AIStyle.BurningSteam:
                    if (ArchaeaItem.Elapsed(time(40)))
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0, 0, 0, default, 3);
                    }
                    rotation -= Draw.radian * 9f;
                    Projectile.NewProjectile(Projectile.GetSource_None(), NPC.Center, ArchaeaNPC.AngleBased(rotation, Speed * 2f), ModContent.ProjectileType<BurningSteam>(), 30, 0.5f, Main.myPlayer);
                    if (ticks > 480)
                    {
                        ai = AIStyle.LavaGlobs;
                        ticks = 0;
                        NPC.netUpdate = true;
                    }
                    break;
                case AIStyle.LavaGlobs:
                    if (ArchaeaItem.Elapsed(time(60)))
                    {
                        Projectile.NewProjectile(Projectile.GetSource_None(), NPC.Center, ArchaeaNPC.AngleBased(ArchaeaNPC.RandAngle(), Speed), ModContent.ProjectileType<OilLeak>(), 20, 0.5f, Main.myPlayer);
                    }
                    if (ticks > 300)
                    {
                        if (ArchaeaItem.Elapsed(20))
                        { 
                            int i = (int)(NPC.position.X + NPC.width) / 2 / 16;
                            int j = (int)(NPC.position.Y + NPC.height - 8) / 16;
                            Main.tile[i, j].LiquidAmount = 0;
                        }
                    }
                    if (ticks > 600)
                    {
                        ai = (AIStyle)Main.rand.Next(new[] {2,3,4});
                        ticks = 0;
                        NPC.netUpdate = true;
                    }
                    break;
            }
        }
        private int time(int time)
        {
            return (int)(time / divisor);
        }
    }
    internal class BurningSteam : ModProjectile
    {
        public override string Texture => "ArchaeaMod/Gores/Null";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Burning steam");
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.scale = 1 / 128f;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.damage = 35;
            Projectile.timeLeft = 20;
        }
        bool init = false;
        public override void AI()
        {
            if (!init)
            {
                Projectile.velocity.Y -= 5f;
                init = true;
            }
            if (Projectile.position.X <= Projectile.oldPosition.X || Projectile.position.X > Projectile.oldPosition.X || Projectile.position.Y <= Projectile.oldPosition.Y || Projectile.position.Y > Projectile.oldPosition.Y)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity += new Vector2(0.917f);
            int dust = Dust.NewDust(Projectile.position, 1, 1, DustID.Water, 0, 0, Scale: 1.5f);
            Main.dust[dust].noGravity = true;
            if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                for (int i = 0; i < 5; i++)
                { 
                    int proj = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position, ArchaeaNPC.AngleToSpeed(ArchaeaNPC.RandAngle(), 5f), ModContent.ProjectileType<dust_diffusion>(), 24, 0f, Main.myPlayer, DustID.Water);
                    Main.projectile[proj].localAI[0] = 10f;
                }
                Lighting.AddLight(Projectile.position, TorchID.Blue);
                Projectile.active = false;
            }
        }
    }
    internal class OilLeak : ModProjectile
    {
        public override string Texture => "ArchaeaMod/Gores/Null";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Oil");
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.alpha = 100;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.damage = 35;
            Projectile.scale = 1 / 128f;
            Projectile.timeLeft = 20;
        }
        bool init = false;
        public override void AI()
        {
            if (!init)
            {
                Projectile.velocity.Y -= 5f;
                init = true;
            }
            if (Projectile.position.X <= Projectile.oldPosition.X || Projectile.position.X > Projectile.oldPosition.X || Projectile.position.Y <= Projectile.oldPosition.Y || Projectile.position.Y > Projectile.oldPosition.Y)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity += new Vector2(0.917f);
            Dust.NewDust(Projectile.position, 1, 1, DustID.SilverFlame, 0, 0, Scale: 2f);
            if (Projectile.wet || Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                WorldGen.PlaceLiquid((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 - 1, LiquidID.Water, 255);
                Projectile.active = false;
            }
            Lighting.AddLight(Projectile.position, TorchID.White);
        }
    }
    internal enum AIStyle : int
    {
        JustSpawned = 0,
        Start = 1,
        ElectricArc = 2,
        BurningSteam = 3,
        LavaGlobs = 4
    }
}
