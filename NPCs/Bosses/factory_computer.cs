using System;
using System.Collections.Generic;
using System.Linq;
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
            NPC.knockBackResist = 1f;
            NPC.lavaImmune = true;
            NPC.lifeMax = 10000;
            NPC.npcSlots = 10f;
            NPC.noGravity = true;
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
        const float Radius = 300f;
        
        public override void AI()
        {
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
                        Projectile.NewProjectile(Projectile.GetSource_None(), NPC.Center, ArchaeaNPC.AngleBased(rotation, Speed), ProjectileID.RocketI, 50, 3f, NPC.whoAmI);
                    }
                    if (ticks > 180)
                    {
                        ai = AIStyle.ElectricArc;
                        ticks = 0;
                        NPC.netUpdate = true;
                    } 
                    break;
                case AIStyle.ElectricArc:
                    for (float i = 0; i < 1f; i += 0.1f)
                    { 
                        rotation = (float)Math.PI * 2f * i;
                        Vector2 start = NPC.Center;
                        Vector2 end = start + ArchaeaNPC.AngleBased(rotation, Radius);
                        ArchaeaItem.Bolt(ref start, end, rotation, 50, 5, 5, -100F);
                    }
                    if (ticks > 300)
                    {
                        ai = AIStyle.BurningSteam;
                        ticks = 0;
                        NPC.netUpdate = true;
                    }
                    break;
                case AIStyle.BurningSteam:
                    if (ArchaeaItem.Elapsed(time(60)))
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SteampunkSteam);
                    }
                    if (ArchaeaItem.Elapsed(ref NPC.ai[3], time(180)))
                    {
                        NPC.ai[3] = 0f;
                        if (ticks % time(60) == 0)
                        {
                            rotation -= 22.5f;
                            Projectile.NewProjectile(Projectile.GetSource_None(), NPC.Center, ArchaeaNPC.AngleBased(rotation, Speed), ModContent.ProjectileType<BurningSteam>(), 30, 0.5f, Main.myPlayer);
                        }
                    }
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
                            int i = (int)NPC.position.X + NPC.width / 2 / 16;
                            int j = (int)NPC.position.Y + NPC.height - 8 / 16;
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
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.damage = 35;
            Projectile.timeLeft = 20;
        }
        public override void AI()
        {
            if (Projectile.position.X <= Projectile.oldPosition.X || Projectile.position.X > Projectile.oldPosition.X || Projectile.position.Y <= Projectile.oldPosition.Y || Projectile.position.Y > Projectile.oldPosition.Y)
            {
                Projectile.netUpdate = true;
            }
            Dust.NewDust(Projectile.position, 1, 1, DustID.Water, 0, 0, Scale: 2f);
            if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                for (int i = 0; i < 5; i++)
                { 
                    int proj = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<dust_diffusion>(), 24, 0f, Main.myPlayer, DustID.Water);
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
            Projectile.timeLeft = 20;
        }
        public override void AI()
        {
            if (Projectile.position.X <= Projectile.oldPosition.X || Projectile.position.X > Projectile.oldPosition.X || Projectile.position.Y <= Projectile.oldPosition.Y || Projectile.position.Y > Projectile.oldPosition.Y)
            {
                Projectile.netUpdate = true;
            }
            Dust.NewDust(Projectile.position, 1, 1, DustID.SilverFlame, 0, 0, Scale: 2f);
            if (Projectile.wet || Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                WorldGen.PlaceLiquid((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, LiquidID.Water, 50);
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
