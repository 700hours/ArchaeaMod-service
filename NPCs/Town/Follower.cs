using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchaeaMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace ArchaeaMod.NPCs.Town
{
    internal class NPCAI : GlobalNPC
    {
        public override void GetChat(NPC npc, ref string chat)
        {
            if (npc.TypeName == "Mechanic")
            {
                SpawnMechanicMinion(npc, npc.GetSource_FromAI());
            }
            else if (npc.townNPC)
            {
                SpawnFollowMenu(npc, npc.GetSource_FromAI());
            }
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.TypeName == "Mechanic")
            {
                SpawnMechanicMinion(npc, source);
            }
            else if (npc.townNPC)
            {
                SpawnFollowMenu(npc, source);
            }
        }
        public static void SpawnFollowMenu(NPC npc, IEntitySource source)
        {
            if (Main.npc.FirstOrDefault(t => t.active && t.type == ModContent.NPCType<FollowerMenu>() && t.ai[1] == npc.type) == default)
            {
                //  Town menu minion
                NPC.NewNPC(source, (int)npc.position.X + npc.width / 2 - 8, (int)npc.position.Y - 32, ModContent.NPCType<FollowerMenu>(), 0, npc.whoAmI, npc.type);
            }
        }
        public static void SpawnMechanicMinion(NPC npc, IEntitySource source)
        {
            if (Main.npc.FirstOrDefault(t => t.active && t.type == ModNPCID.MechanicMinion) == default)
            {
                //  Faux Mechanic 
                //  Ran in the Faux minion chat dialog
                //  Projectile.NewProjectile(source, (int)npc.position.X, (int)npc.position.Y, 0f, 0f, ModContent.ProjectileType<Mechanic>(), 20, 2f);
                //  Faux minion
                int index = NPC.NewNPC(source, (int)npc.position.X, (int)npc.position.Y, ModNPCID.MechanicMinion);
                NPC n = Main.npc[index];
                //  Real minion
                int proj = Projectile.NewProjectile(source, npc.position, Vector2.Zero, ModContent.ProjectileType<Merged.Projectiles.magno_minion>(), 26, 1f, Main.myPlayer, npc.whoAmI);
                Main.projectile[proj].localAI[0] = 26;
                //  Set minion owner
                n.ai[0] = proj;
            }
        }
    }
    internal class FollowerMenu : ModNPC
    {
        public override string Texture => "ArchaeaMod/Gores/arrow";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Follower Menu");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.scale = 24 / 128f;
            NPC.rotation = (float)(90 * (decimal)Draw.radian);
            NPC.width = 128;
            NPC.height = 128;
            NPC.immortal = true;
            NPC.dontTakeDamage = true;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.lifeMax = 100;
            NPC.defense = 10;
            NPC.knockBackResist = 1f;
            NPC.damage = 10;
            NPC.value = 1000;
            NPC.lavaImmune = true;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.alpha = 0;
            NPC.friendly = true;
        }
        private int owner
        {
            get { return (int)NPC.ai[0]; }
            set { NPC.ai[0] = value; }
        }
        private int type
        {
            get { return (int)NPC.ai[1]; }
            set { NPC.ai[1] = value; }
        }
        NPC Owner => Main.npc[owner];
        Projectile leader => Main.projectile[projID];
        int projID;
        public override bool CanChat() => true;
        public bool flag = false;
        public override void DrawEffects(ref Color drawColor)
        {
            drawColor = Color.LightGray;
        }
        public override void ModifyHoverBoundingBox(ref Rectangle boundingBox)
        {
            boundingBox = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
        }
        public override void AI()
        {
            if (!Main.npc[owner].active)
            {
                NPC.active = false;
            }
            else
            {                                            
                NPC.position = new Vector2(Owner.position.X + Owner.width / 2 - NPC.width / 2, Owner.Hitbox.Top - NPC.height * 2);
            }
            if (NPC.position.X <= NPC.oldPosition.X || NPC.position.X > NPC.oldPosition.X || NPC.position.Y <= NPC.oldPosition.Y || NPC.position.Y > NPC.oldPosition.Y)
            {
                NPC.netUpdate = true;
            }
        }
        public override string GetChat()
        {
            return flag ?
                $"Would you like to have the {Main.npc[owner].TypeName} follow you? She's really cool, isn't she?" :
                $"Would you like to have the {Main.npc[owner].TypeName} stop following you? She weird, isn't she?";
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (flag)
            {
                button = "Stop following";
            }
            else
            {
                button = "Follow";
            } 
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                flag = !flag;
                if (!flag && leader != default)
                {
                    leader.active = false;
                    return;
                }
                projID = Projectile.NewProjectile(Projectile.GetSource_TownSpawn(), (int)Owner.position.X, (int)Owner.position.Y, 0f, 0f, ModContent.ProjectileType<Follower>(), 20, 2f, Main.myPlayer, owner, Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<Follower>()]);
                Main.projectile[projID].localAI[0] = type;
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return !target.townNPC;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
        public override bool CheckActive()
        {
            return Main.npc[owner].active;
        }
    }
    internal class Follower : ModProjectile
    {
        public override string Texture => "ArchaeaMod/Gores/Null";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Follower");
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 32;
            Projectile.height = 42;
            Projectile.damage = 0;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }
        int ownerIndex
        {
            get { return (int)Projectile.ai[0]; }
            set { Projectile.ai[0] = value;}
        }
        int followerID
        {
            get { return (int)Projectile.ai[1]; }
            set { Projectile.ai[1] = value; }
        }
        int ownerType
        {
            get { return (int)Projectile.localAI[0]; }
            set { Projectile.localAI[0] = value; }
        }
        bool init = false;
        bool beginMove = false;
        float rand = 0f;
        int ticks = 0;
        int ticks2 = 0;
        NPC owner => Main.npc[ownerIndex];
        IList<Vector2> oldVelocity = new List<Vector2>();
        IList<Vector2> oldVelocity2 = new List<Vector2>();
        public override bool? CanCutTiles() => false;
        private bool PlayerNotControlMove(Player player)
        {
            return player != null && !player.controlUp && !player.controlRight && !player.controlDown && !player.controlLeft && !player.controlJump;
        }
        private bool PlayerMoving(Player player)
        {
            return player.velocity.X != 0f && player.velocity.Y != 0f;
        }
        private bool FollowerMoving(Projectile follower)
        {
            return follower.position.X <= follower.oldPosition.X || follower.position.X > follower.oldPosition.X || follower.position.Y <= follower.oldPosition.Y || follower.position.Y > follower.oldPosition.Y;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.townNPC || target.friendly || target.CountsAsACritter)
                return;
            if (ArchaeaItem.Elapsed(ref ticks2, 20))
            {
                target.StrikeNPC(Main.hardMode ? 40 : 20, 2f, target.Center.X < Projectile.Center.X ? -1 : 1, Main.rand.NextBool(), false, Main.netMode != 0);
                ticks2 = 0;
            }
        }
        public override bool PreAI()
        {
            if (!init)
            {
                rand = Main.rand.Next(-50, 50) + 1;
                init = true;
            }
            owner.velocity = Projectile.velocity;
            owner.position = Projectile.position;
            if (Projectile.position.X <= Projectile.oldPosition.X || Projectile.position.X > Projectile.oldPosition.X || Projectile.position.Y <= Projectile.oldPosition.Y || Projectile.position.Y > Projectile.oldPosition.Y)
            {
                Projectile.netUpdate = true;
            }
            return true;
        }
        public override void AI()
        {
            if (owner.active)
            {
                Projectile.timeLeft = 10;
                Projectile.height = owner.height;
            }
            Player player = Main.LocalPlayer;
            var follower = Main.projectile.Where(t => t.active && t.owner == player.whoAmI && t.type == Type && t.localAI[0] != ownerType).ToArray();
            if (followerID == 0)
            {
                Follow(player);
            }
            else
            {
                Follow(player, follower[followerID - 1]);
            }
        }
        public void Follow(Player player)
        {
            if (!PlayerNotControlMove(player) || PlayerMoving(player))
            {
                oldVelocity.Add(player.position + new Vector2(rand, Math.Abs(player.height - owner.height)));
                if (!beginMove)
                {
                    if (ArchaeaItem.Elapsed(ref ticks, 60))
                    {
                        ticks = 0;
                        beginMove = true;
                    }
                    else
                    {
                        Projectile.position += ArchaeaNPC.AngleToSpeed(Projectile.AngleTo(oldVelocity[0]), player.moveSpeed);
                    }
                }
            }
            if (oldVelocity.Count > 0)
            {
                if (beginMove)
                {
                    owner.direction = player.Center.X < owner.Center.X ? -1 : 1;
                    Projectile.velocity = player.velocity;
                    Projectile.position = oldVelocity[0];
                    oldVelocity.RemoveAt(0);
                }
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                beginMove = false;
            }
            if (Projectile.velocity.Y < 0f)
            {
                int d = Dust.NewDust(Projectile.position + new Vector2(0, Projectile.height - 2), 1, 1, DustID.Torch, Scale: 2f);
                Main.dust[d].noLight = false;
                Main.dust[d].noGravity = true;
                int d2 = Dust.NewDust(Projectile.position + new Vector2(Projectile.width - 16, Projectile.height - 2), 1, 1, DustID.Torch, Scale: 2f);
                Main.dust[d2].noLight = false;
                Main.dust[d2].noGravity = true;
                if (ArchaeaItem.Elapsed(ref ticks2, 5))
                {
                    SoundEngine.PlaySound(SoundID.Item13, Projectile.Center);
                    ticks2 = 0;
                }
            }
        }
        public void Follow(Player player, Projectile npc)
        {
            if (FollowerMoving(npc))
            {
                oldVelocity2.Add(npc.position + new Vector2(rand, Math.Abs(npc.height - owner.height)));
                if (!beginMove)
                {
                    if (ArchaeaItem.Elapsed(ref ticks, 60))
                    {
                        ticks = 0;
                        beginMove = true;
                    }
                    else
                    {
                        Projectile.position += ArchaeaNPC.AngleToSpeed(Projectile.AngleTo(oldVelocity2[0]), player.moveSpeed);
                    }
                }
            }
            if (oldVelocity2.Count > 0)
            {
                if (beginMove)
                {
                    owner.direction = npc.Center.X < owner.Center.X ? -1 : 1;
                    Projectile.velocity = npc.velocity;
                    Projectile.position = oldVelocity2[0];
                    oldVelocity2.RemoveAt(0);
                }
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                beginMove = false;
            }
            if (Projectile.velocity.Y < 0f)
            {
                int d = Dust.NewDust(Projectile.position + new Vector2(0, Projectile.height - 2), 1, 1, DustID.Torch, Scale: 2f);
                Main.dust[d].noLight = false;
                Main.dust[d].noGravity = true;
                int d2 = Dust.NewDust(Projectile.position + new Vector2(Projectile.width - 16, Projectile.height - 2), 1, 1, DustID.Torch, Scale: 2f);
                Main.dust[d2].noLight = false;
                Main.dust[d2].noGravity = true;
                if (ArchaeaItem.Elapsed(ref ticks2, 5))
                {
                    SoundEngine.PlaySound(SoundID.Item13, Projectile.Center);
                    ticks2 = 0;
                }
            }
        }
    }
}
