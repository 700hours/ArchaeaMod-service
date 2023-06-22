using System;
using System.Transactions;
using System.Windows.Markup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent;
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
        bool[] zone => target.GetModPlayer<ArchaeaPlayer>().zones;
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
                        }
                    }
                    break;
            }
        }
        public override void PostAI()
        {
            
        }
        public override void PostDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            int lenX = OldCage.coord.GetLength(0);
            int lenY = OldCage.coord.GetLength(1);
            for (int m = 0; m < lenX; m++)
            {
                for (int n = 0; n < lenY; n++)
                {
                    if (m == 0 || m == lenX - 1 || n == 0 || n == lenY - 1)
                    {
                        int x = OldCage.coord[m, n].X * 16;
                        int y = OldCage.coord[m, n].Y * 16;
                        //sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(x, y, 16, 16), )
                    }
                } 
            }
        }
        Color getZoneColor()
        {
            for (int i = 0; i < zone.Length; i++)
            {
                
            }   
            return Color.White;
        }

        void ClearOldCage()
        {
            if (OldCage.coord == null) return;
            int lenX = OldCage.coord.GetLength(0);
            int lenY = OldCage.coord.GetLength(1);
            for (int m = 0; m < lenX; m++)
            for (int n = 0; n < lenY; n++)
            {
                if (m == 0 || m == lenX - 1 || n == 0 || n == lenY - 1)
                {
                    OldCage.coord[m, n] = Point.Zero;
                }
            }
            OldCage.coord = null;
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
