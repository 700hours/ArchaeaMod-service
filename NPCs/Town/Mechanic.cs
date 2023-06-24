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
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.TypeName == "Mechanic")
            { 
                SpawnMechanicMinion(npc, source);
            }
        }
        public static void SpawnMechanicMinion(NPC npc, IEntitySource source)
        {
            if (Main.npc.FirstOrDefault(t => t.active && t.type == ModNPCID.Mechanic) == default)
            { 
                //  Faux Mechanic 
                NPC.NewNPC(source, (int)npc.position.X, (int)npc.position.Y, ModNPCID.Mechanic);
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
    internal class MechanicMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanic's Minion");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 26;
            NPC.height = 26;
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
            NPC.alpha = 255;
            NPC.friendly = true;
        }
        private int owner
        {
            get { return (int)NPC.ai[0]; }
            set { NPC.ai[0] = value; }
        }
        public Projectile Owner => Main.projectile[owner];
        public override string Texture => "ArchaeaMod/Gores/Blank";
        public bool flag 
        { 
            get { return ModContent.GetInstance<Mechanic>().following; }
            set { ModContent.GetInstance<Mechanic>().following = value; } 
        }
        public bool justTalked = false;
        public override bool CanChat() => true;

        public override void AI()
        {
            NPC.position = Owner.position;
            if (NPC.position.X <= NPC.oldPosition.X || NPC.position.X > NPC.oldPosition.X || NPC.position.Y <= NPC.oldPosition.Y || NPC.position.Y > NPC.oldPosition.Y)
            {
                NPC.netUpdate = true;
            }
        }
        public override string GetChat()
        {
            if (!justTalked)
            {
                justTalked = true;
                return "The cultists tossed her down in the dungeon as she was just about to open the factory door, and will ask you to take her to the factory.";
            }
            return flag ? 
                "Would you like to have the mechanic follow you? She's really cool, isn't she?" : 
                "Would you like to have the mechanic stop following you? She weird, isn't she?";
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = flag ? "Stop following" : "Follow";
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                flag = !flag;
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
            return Owner.active;
        }
    }
    internal class Mechanic : ModNPC
    {
        public bool following = false;
        public override string Texture => "ArchaeaMod/Gores/Blank";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanic");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 28;
            NPC.height = 42;
            NPC.immortal = true;
            NPC.dontTakeDamage = true;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.lifeMax = 100;
            NPC.defense = 10;
            NPC.knockBackResist = 1f;
            NPC.damage = 0;
            NPC.value = 1000;
            NPC.lavaImmune = true;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.alpha = 255;
            NPC.friendly = true;
            NPC.noGravity = false;
        }
        bool beginMove = false;
        int ticks = 0;
        int ticks2 = 0;
        NPC owner => Main.npc.FirstOrDefault(t => t.TypeName == "Mechanic");
        IList<Vector2> oldVelocity = new List<Vector2>();
        private bool PlayerNotControlMove(Player player)
        {
            return !player.controlUp && !player.controlRight && !player.controlDown && !player.controlLeft && !player.controlJump;
        }
        private bool PlayerMoving(Player player)
        {
            return player.velocity.X != 0f && player.velocity.Y != 0f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.townNPC || target.friendly || target.CountsAsACritter)
                return;
            if (ArchaeaItem.Elapsed(ref ticks2, 20))
            { 
                target.StrikeNPC(Main.hardMode ? 40 : 20, 2f, target.Center.X < NPC.Center.X ? -1 : 1, Main.rand.NextBool(), false, Main.netMode != 0);
                ticks2 = 0;
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
        public override bool PreAI()
        {
            owner.velocity = NPC.velocity;
            owner.position = NPC.position;
            if (NPC.position.X <= NPC.oldPosition.X || NPC.position.X > NPC.oldPosition.X || NPC.position.Y <= NPC.oldPosition.Y || NPC.position.Y > NPC.oldPosition.Y)
            {
                NPC.netUpdate = true;
            }
            return true;
        }
        public override void AI()
        {
//          if (!following) return;
            Player player = Main.LocalPlayer;
            NPC.direction = NPC.velocity.X < 0f ? -1 : 1;
            if (!PlayerNotControlMove(player) || PlayerMoving(player))
            {
                oldVelocity.Add(player.position);
                if (!beginMove && ArchaeaItem.Elapsed(ref ticks, 60))
                {
                    ticks = 0;
                    beginMove = true;
                }
            }
            if (oldVelocity.Count > 0)
            { 
                if (beginMove)
                {
                    NPC.velocity = player.velocity;
                    NPC.position = oldVelocity[0];
                    oldVelocity.RemoveAt(0);
                }
            }
            else 
            {
                NPC.velocity = Vector2.Zero;
                beginMove = false;
            }
            if (NPC.velocity.Y < 0f)
            {
                int d  = Dust.NewDust(NPC.position + new Vector2(0, NPC.height - 2), 1, 1, DustID.Torch, Scale: 1.5f);
                Main.dust[d].noLight = false;
                Main.dust[d].noGravity = true;
                int d2 = Dust.NewDust(NPC.position + new Vector2(NPC.width - 16, NPC.height - 2), 1, 1, DustID.Torch, Scale: 1.5f);
                Main.dust[d2].noLight = false;
                Main.dust[d2].noGravity = true;
                if (ArchaeaItem.Elapsed(ref ticks2, 5))
                {
                    SoundEngine.PlaySound(SoundID.Item13, NPC.Center);
                    ticks2 = 0;
                }
            }
        }
        public override bool CheckActive()
        {
            return owner.active;
        }
    }
}
