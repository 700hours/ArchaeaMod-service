using System;
using System.Transactions;
using System.Windows.Markup;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs.Bosses
{
    public class _boss02 : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = 128;
            NPC.height = 128;
            NPC.friendly = false;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.boss = true;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 4180;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.netUpdate = true;
        }
        int ticks
        {
            get { return (int)NPC.ai[0]; }
            set { NPC.ai[0] = value; }
        }
        AI oldAI;
        AI ai 
        {
            get { return (AI)(byte)NPC.ai[1]; }
            set { NPC.ai[1] = (byte)value; }
        }
        Player target => Main.player[NPC.target];
        bool[,] oldCage;
        //  this boss unlocks biomes
        //  if not defeated, the biome remains locked
        //  changes patterns after each defeat
        public override bool PreAI()
        {
            ticks++;
            if (ai == 0)
            {
                //spawn effects
                ai = (AI)1;
            }
            return ai != 0;
        }
        public override void AI()
        {
            if (target.dead)
            {
                if (ticks % 100 == 0)
                { 
                    NPC.TargetClosest(); 
                }
                return;
            }
            if (ticks > 600)
            {
                NPC.TargetClosest();
                ticks = 0;
                ai++;
                if ((byte)ai > 4)
                {
                    ai = (AI)1;
                }
                if (Main.rand.NextBool(10))
                {
                    ai = (AI)(byte)Main.rand.Next(new[] {1, 2, 4 });
                }
            }
            switch (ai)
            {
                case Bosses.AI.Cage:
                    //  reset immediately before continuing
                    SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                    ai = Bosses.AI.None;
                    {   // init
                        this.ClearOldCage();
                        int size = SmartSize();
                        oldCage = new bool[size, size];
                    }
                    int lenX = oldCage.GetLength(0);
                    int lenY = oldCage.GetLength(1);
                    OldCage.coord = new Point[lenX, lenY];
                    for (int m = 0; m < lenX; m++)
                    for (int n = 0; n < lenY; n++)
                    {
                        if (m == 0 || m == lenX - 1 || n == 0 || n == lenY - 1)
                        { 
                            int i = (int)target.position.X / 16 - lenX / 2 + m;
                            int j = (int)target.position.Y / 16 - lenY / 2 + n;
                            OldCage.coord[m, n] = new Point(i, j);
                            {   //  Setting tiles
                                Tile tile = Main.tile[Math.Max(0, i), Math.Max(0, j)];
                                if (!tile.HasTile && tile.BlockType == TileID.Dirt)
                                { 
                                    oldCage[m, n] = true;
                                    tile.TileType = TileID.AdamantiteBeam;
                                    tile.HasTile = true;
                                    WorldGen.SectionTileFrame(i - 1, j - 1, i + 1, j + 1);
                                    if (Main.netMode != NetmodeID.SinglePlayer)
                                    {   //  DEBUG: proper message ID?
                                        NetMessage.SendData(MessageID.TileFrameSection, number: i, number2: j);
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }
        void ClearOldCage()
        {
            if (oldCage == null) return;
            int lenX = oldCage.GetLength(0);
            int lenY = oldCage.GetLength(1);
            for (int m = 0; m < lenX; m++)
            for (int n = 0; n < lenY; n++)
            {
                if (m == 0 || m == lenX - 1 || n == 0 || n == lenY - 1)
                {
                    if (OldCage.coord.GetLength(0) < m || OldCage.coord.GetLength(1) < n)
                    { 
                        return;
                    }
                    var p = OldCage.coord[m, n];
                    if (p.X <= 0 || p.X >= Main.maxTilesX || p.Y <= 0 || p.Y >= Main.maxTilesY)
                    {
                        return;
                    }
                    if (!oldCage[m, n])
                    {
                        WorldGen.KillTile(p.X, p.Y, false, false, true);
                    }
                }
            }
            oldCage = null;
        }
        int SmartSize(float @base = 15f)
        {
            return (int)(@base / (target.statLifeMax2 / 500f));
        }
    }
    public static class OldCage
    {
        public static Point[,] coord;
    }
    public enum AI : byte
    {
        None = 1,
        Cage = 3,
        Laser = 4,
        Physics = 2,
        JustSpawned = 0
    }
}
