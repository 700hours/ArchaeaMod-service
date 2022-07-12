//using System;
//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace ArchaeaMod.NPCs
//{
//    public class _boss : ModNPC
//    {
//        public override void SetDefaults()
//        {
//            NPC.width = 128;
//            NPC.height = 128;
//            NPC.friendly = false;
//            NPC.noTileCollide = true;
//            NPC.noGravity = true;
//            NPC.aiStyle = -1;
//            NPC.boss = true;
//            NPC.damage = 10;
//            NPC.defense = 15;
//            NPC.lifeMax = 4180;
//            NPC.HitSound = SoundID.NPCHit1;
//            NPC.DeathSound = SoundID.NPCDeath1;
//            NPC.knockBackResist = 0f;
//            NPC.netUpdate = true;
//        }
//        bool sweep = false, spinAttack = false;
//        bool flames= false;
//        bool pupsSpawned = false;
//        bool magnoClone = false;
//        bool init = false;
//        int dust;
//        int pups, clone, timeLeft = 600;
//        int flamesID;
//        int ticks = 0;
//        int choose = 0;
//        int direction;
//        float TargetAngle, PlayerAngle;
//        float degrees = 0, radius = 64;
//        float Depreciate = 80, Point;
//        const float Time = 80;
//        const float radians = 0.017f;
//        Rectangle target;
//        Vector2 oldPosition, newPosition;
//        Vector2 npcCenter, playerCenter, center;
//        Vector2 Start, End;
//        public void Initialize()
//        {
//            NPC.TargetClosest(true);
//        }
//        public override void AI()
//        {
//            if(!init)
//            {
//                Initialize();
//                init = true;
//            }
//            NPC.ai[0]++;

//            Player player = Main.player[NPC.target];
//            if (NPC.target < 0 || NPC.target == 255 || player.dead || !player.active)
//            {
//                NPC.TargetClosest(true);
//            }

//            npcCenter = new Vector2(NPC.position.X + NPC.width / 2, NPC.position.Y + NPC.height / 2);
//            playerCenter = new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2);
//            PlayerAngle = (float)Math.Atan2(player.position.Y - NPC.position.Y,
//                                            player.position.X - NPC.position.X);
//            #region default pattern
//            if (NPC.ai[0] < 720)
//            {
//                if (NPC.ai[0] == 1)
//                {
//                    oldPosition = new Vector2(player.position.X + Random(), player.position.Y + Main.rand.Next(-400, 64));
//                    target = new Rectangle((int)oldPosition.X - 16, (int)oldPosition.Y - 16, 32, 32);
//                }
//                if (NPC.Hitbox.Intersects(target))
//                {
//                    oldPosition = new Vector2(player.position.X + Random(), player.position.Y + Main.rand.Next(-400, 64));
//                    target = new Rectangle((int)oldPosition.X - 16, (int)oldPosition.Y - 16, 32, 32);
//                }
//                if (Vector2.Distance(oldPosition - NPC.position, Vector2.Zero) > 280f)
//                {
//                    TargetAngle = (float)Math.Atan2(oldPosition.Y - NPC.position.Y,
//                                                    oldPosition.X - NPC.position.X);
//                    NPC.velocity.X = TargetSD(null, TargetAngle, 4f).X;
//                    NPC.velocity.Y = TargetSD(null, TargetAngle, 4f).Y;

//                    NPC.rotation = TargetAngle;
//                }
//                else if (NPC.ai[0] == 1 || NPC.ai[0]%200 == 0)
//                {
//                    NPC.velocity.X = TargetSD(null, PlayerAngle, 4f).X;
//                    NPC.velocity.Y = TargetSD(null, PlayerAngle, 4f).Y;

//                    NPC.rotation = PlayerAngle;
//                }
//                /*
//                if (NPC.position.Y > player.position.Y)
//                    NPC.velocity.Y -= 0.4f; */
//            }
//            #endregion
//            if(NPC.ai[0] >= 720)
//            {
//                if(NPC.ai[0] == 740)
//                    NPC.TargetClosest(true);

//                #region sweep
//                if (NPC.ai[0] >= 780 && NPC.ai[0] < 780 + Time)
//                {
//                    Start.X = player.position.X - 256;
//                    Start.Y = player.position.Y - 448;

//                    End.X = player.position.X + 256;
//                    End.Y = Start.Y;

//                    sweep = true;
//                }
//                if (Depreciate > 0 && sweep)
//                {
//                    Depreciate--;
//                    Point = (Time - Depreciate) / Time;
//                    NPC.position = Vector2.Lerp(Start, End, Point);

//                    NPC.rotation = radians * 90;
//                    if (Depreciate % 10 == 0)
//                    {
//                        int attack = Projectile.NewProjectile(NPC.position + new Vector2(NPC.width / 2, NPC.height / 2 + 32), Vector2.Zero, 134, 12 + Main.rand.Next(-2, 8), 2f, player.whoAmI, 0f, 0f);
//                        Projectile proj = Main.projectile[attack];
//                        proj.velocity.X += TargetSD(null, NPC.rotation, 4f).X;
//                        proj.velocity.Y += TargetSD(null, NPC.rotation, 4f).Y;
//                        proj.friendly = false;
//                        proj.hostile = true;
//                    }
//                }
//                else if (Depreciate == 0)
//                {
//                    sweep = false;
//                    Depreciate = Time;
//                }
//                int buffer = 1;
//                if (NPC.ai[0] >= 780 + Time + buffer && NPC.ai[0] < 1000)
//                {
//                    NPC.position = new Vector2(player.position.X - NPC.width / 2, player.position.Y - 256f);
//                    NPC.alpha = 200;
//                    NPC.immortal = true;

//                    player.velocity = Vector2.Zero;
//                }
//                #endregion
//                #region spin
//                if (NPC.ai[0] == 1000)
//                {
//                    NPC.TargetClosest(true);
//                    NPC.alpha = 0;
//                    NPC.immortal = false;

//                    Start.X = NPC.position.X;
//                    Start.Y = NPC.position.Y;

//                    End.X = Start.X;
//                    End.Y = player.position.Y - 384;
//                }
//                if (NPC.ai[0] > 1080)
//                {
//                    if (Depreciate > 0)
//                    {
//                        Depreciate--;
//                        Point = (Time - Depreciate) / Time;
//                        NPC.position = Vector2.Lerp(oldPosition, End, Point);

//                        TargetAngle = (float)Math.Atan2(End.Y - NPC.position.Y,
//                                                        End.X - NPC.position.X);
//                        NPC.rotation = TargetAngle;
//                    }
//                    if (Depreciate == 0) 
//                    {
//                        Depreciate = Time;
//                        spinAttack = true;
//                        direction = Main.rand.Next(0, 1);
//                        choose = 0;
//                    }
//                }
//                if (spinAttack)
//                {
//                    NPC.position = End;

//                    if (direction == 0)
//                        degrees += radians * 8;
//                    else if (direction == 1)
//                        degrees -= radians * 8;

//                    NPC.rotation = degrees;
//                    if (degrees >= radians * 360)
//                        degrees = radians;
//                    ticks++;
//                    if (ticks % 3 == 0 && degrees >= radians * 45f && degrees <= radians * 146.25f)
//                    {
//                        int attack = Projectile.NewProjectile(NPC.position + new Vector2(NPC.width / 2, NPC.height / 2 + 32), Vector2.Zero, 134, 20, 2f, player.whoAmI, 0f, 0f);
//                        Projectile proj = Main.projectile[attack];
//                        proj.velocity.X += TargetSD(null, NPC.rotation, 4f).X;
//                        proj.velocity.Y += TargetSD(null, NPC.rotation, 4f).Y;
//                        proj.friendly = false;
//                        proj.hostile = true;
//                    }
//                    if (ticks > 180)
//                    {
//                        spinAttack = false;
//                        degrees = radians;
//                        NPC.rotation = PlayerAngle;
//                        Depreciate = Time;
//                        ticks = 0;
//                        NPC.ai[0] = 0;

//                        NPC.position = new Vector2(player.position.X + Random(), player.position.Y + Random());
//                    }
//                }
//                #endregion
//            }
//            #region spirit flames
//            if (!flames && Main.rand.Next(0, 4800) == 0)
//            {
//                for (int k = 0; k < 4; k++)
//                {
//                    degrees = 90f;
//                    radius = 128f;
//                    center = player.position;
//                    float nX = center.X + (float)(radius * Math.Cos(degrees * k));
//                    float nY = center.Y + (float)(radius * Math.Sin(degrees * k));

//                    flamesID = NPC.NewNPC(NPC.GetSource_FromAI(), (int)nX, (int)nY, ModContent.NPCType<m_flame>());
//                    if(Main.netMode != 0)
//                        NetMessage.SendData(23, -1, -1, null, flamesID);

//                    Main.npc[flamesID].ai[1] = degrees * k;
//                    flames = true;
//                }
//            }
//            if (flames)
//            {
//                radius -= 0.5f;
//                NPC n = Main.npc[flamesID];
//                if (n.active = false || radius <= 1f)
//                    flames = false;
//            }
//            #endregion

//            #region magno clone sequence
//            /*  if (!pupsSpawned && Main.rand.Next(0, 6000) == 0)
//                {
//                    for (int k = 0; k < 4; k++)
//                    {
//                        pups = NPC.NewNPC((int)npcCenter.X + Main.rand.Next(-NPC.width, NPC.width), (int)npcCenter.Y, mod.NPCType("m_diggerhead"));
//                        NetMessage.SendData(23, -1, -1, null, pups, 0f, 0f, 0f, 0, 0, 0);
//                        pupsSpawned = true;
//                    }
//                }
//                if (pupsSpawned)
//                {
//                    Main.npc[pups].realLife = Main.npc[pups].whoAmI;
//                    if (!Main.npc[pups].active)
//                    {
//                        pupsSpawned = false;
//                        magnoClone = true;
//                    }
//                }
//                if (magnoClone)
//                {
//                    clone = NPC.NewNPC((int)npcCenter.X, (int)npcCenter.Y + 128, mod.NPCType("m_mimic"));
//                    Main.npc[clone].color = Color.Gold;
//                    Main.npc[clone].scale = 0.6f;
//                    timeLeft = 600;
//                    magnoClone = false;
//                }
//                if(timeLeft > 0)
//                    timeLeft--;
//                if (timeLeft == 0)
//                {
//                    Main.npc[clone].active = false;
//                    timeLeft = 600;
//                }   */
//            #endregion
//        }
//        public void SpawnDust(Vector2 vector, int width, int height, int dustType, Color color, float scale)
//        {
//            dust = Dust.NewDust(vector, width, height, dustType, 0f, 0f, 255, color, scale);
//            Main.dust[dust].noGravity = true;
//        }
//        public float Random()
//        {
//            return Main.rand.Next(-400, 400);
//        }
//        public Vector2 TargetSD(Player player, float Angle, float Radius)
//        {
//            float VelocityX = (float)(Radius * Math.Cos(Angle));
//            float VelocityY = (float)(Radius * Math.Sin(Angle));

//            return new Vector2(VelocityX, VelocityY);
//        }
//    }
//}
