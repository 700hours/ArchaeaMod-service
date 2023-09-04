using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ArchaeaMod.Entities;
using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Buffs;
using ArchaeaMod.Jobs.Projectiles;
using ArchaeaMod.NPCs.Legacy;
using ArchaeaMod.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using XPT.Core.Audio.MP3Sharp.Decoding;

namespace ArchaeaMod.NPCs.Bosses
{
    internal class factory_02 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Factory Computer");
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public override void SetDefaults()
        {
            NPC.width = 64;
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
        bool init = false;
        int[] parts = new int[4];
        public override bool PreAI()
        {
            if (!init)
            {
                int x = (int)NPC.position.X;
                int y = (int)NPC.position.Y;
                for (int i = 0; i < 4; i++)
                {
                    parts[i] = NPC.NewNPC(NPC.GetSource_FromAI(), x, y, ModContent.NPCType<factory_02_arm_01>(), 0, i, NPC.whoAmI);
                }
                init = true;
            }
            return init;
        }
        public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            int x = (int)(NPC.position.X - Main.screenPosition.X);
            int y = (int)(NPC.position.Y - Main.screenPosition.Y);
            sb.Draw(Mod.Assets.Request<Texture2D>("Gores/arrow").Value, new Rectangle(x, y, NPC.width, NPC.height), Color.White);
            return false;
        }
    }
    internal class factory_02_arm_01 : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 24;
            NPC.aiStyle = -1;
            NPC.behindTiles = true;
            NPC.damage = 30;
            NPC.defense = 12;
            NPC.knockBackResist = 1;
            NPC.lavaImmune = true;
            NPC.lifeMax = 10000;
            NPC.npcSlots = 10f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = 50000;
        }
        bool init = false;
        int ai = 0;
        int part = 0;
        int ticks = -150;
        bool idle = false;
        public int id => (int)NPC.ai[0];
        public NPC parent => Main.npc[(int)NPC.ai[1]];
        public NPC child;
        public override bool PreAI()
        {
            switch (id + 1)
            {
                case 1:
                    NPC.position = parent.position + new Vector2(-64, 32);
                    NPC.direction = -1;
                    break;
                case 2:
                    NPC.position = parent.position + new Vector2(64, 32);
                    NPC.direction = 1;
                    break;
                case 3:
                    NPC.position = parent.position + new Vector2(-64, 64);
                    NPC.direction = -1;
                    break;
                case 4:
                    NPC.position = parent.position + new Vector2(64, 64);
                    NPC.direction = 1;
                    break;
            }
            if (!init)
            {
                int x = (int)NPC.position.X;
                int y = (int)NPC.position.Y;
                part = NPC.NewNPC(NPC.GetSource_FromAI(), x, y, ModContent.NPCType<factory_02_arm_02>(), 0, NPC.whoAmI, 0, parent.whoAmI, id);
                (child = Main.npc[part]).direction = NPC.direction;
                child.ai[1] = 1;
                init = true;
            }
            return init;
        }
        public override void AI()
        {
            if (ticks++ == 150)
            {
                child.ai[1] *= -1;
                if (++ai > 4)
                {
                    ai = 0;
                }
                ticks = -150;
            }
            switch (ai)
            {
                default:
                case 0:
                    break;
                case 1:
                case 3:
                    NPC.rotation = MathHelper.ToRadians(Vector2.Lerp(new Vector2(105f, 0f), new Vector2(150f, 0f), Math.Abs(ticks) / 150f).X);
                    break;
                case 2:
                case 4:
                    NPC.rotation = MathHelper.ToRadians(Vector2.Lerp(new Vector2(105f + 180f, 0f), new Vector2(150f + 130f, 0f), Math.Abs(ticks) / 300f).X);
                    break;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }
        public override void PostDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            Vector2 origin = Vector2.Zero;
            switch (id + 1)
            {
                case 1:
                case 3:
                    origin = new Vector2(NPC.width, NPC.height / 2);
                    break;
                case 2:
                case 4:
                    origin = new Vector2(0, NPC.height / 2);
                    break;
            }
            sb.Draw(Mod.Assets.Request<Texture2D>("Gores/arrow").Value, NPC.position - Main.screenPosition, null, Color.White, NPC.rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
    internal class factory_02_arm_02 : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 64;
            NPC.aiStyle = -1;
            NPC.behindTiles = true;
            NPC.damage = 30;
            NPC.defense = 12;
            NPC.knockBackResist = 1;
            NPC.lavaImmune = true;
            NPC.lifeMax = 10000;
            NPC.npcSlots = 10f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = 50000;
        }
        bool init = false;
        int determinate => (int)NPC.ai[1];
        int id => (int)NPC.ai[3];
        float chaseSpeed = 0f;
        float x;
        Vector2 pOrigin => new Vector2(x, parent.position.Y + parent.height / 2);
        Vector2 origin  => new Vector2(NPC.position.X + (NPC.direction == -1 ? NPC.width : 0f), NPC.position.Y + NPC.height / 2);
        public NPC parent => Main.npc[(int)NPC.ai[0]];
        public NPC source => Main.npc[(int)NPC.ai[2]];
        public override bool PreAI()
        {
            if (!init)
            {
                switch (id + 1)
                {
                    case 1:
                    case 3:
                        NPC.position = parent.position + new Vector2(-64, 0);
                        break;
                    case 2:
                    case 4:
                        NPC.position = parent.position + new Vector2(64, 0);
                        break;
                }
                x = parent.position.X;
                if (NPC.direction == -1)
                {
                    x -= NPC.width;
                }
                init = true;
            }
            return init;
        }
        public override void AI()
        {
            NPC.rotation = origin.AngleTo(pOrigin);
            if (origin.Distance(pOrigin) > 3f)
            {
                chaseSpeed = 3f * determinate;
                float cos = (float)(chaseSpeed * Math.Cos(NPC.rotation));
                float sine = (float)(chaseSpeed * Math.Sin(NPC.rotation));
                NPC.velocity = new Vector2(cos, sine);
            }
            else
            {
                NPC.velocity = Vector2.Zero;
                chaseSpeed = 0f;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }
        public override void PostDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            Vector2 corner = Vector2.Zero;
            switch (id + 1)
            {
                case 1:
                    corner = new Vector2(-400, -400);
                    break;
                case 2:
                    corner = new Vector2(400, -400);
                    break;
                case 3:
                    corner = new Vector2(400, 400);
                    break;
                case 4:
                    corner = new Vector2(-400, 400);
                    break;
            }
            Vector2 origin = Vector2.Zero;
            switch (id + 1)
            {
                case 1:
                case 3:
                    origin = new Vector2(NPC.width, NPC.height / 2);
                    break;
                case 2:
                case 4:
                    origin = new Vector2(0, NPC.height / 2);
                    break;
            }
            sb.Draw(Mod.Assets.Request<Texture2D>("Gores/arrow").Value, NPC.position - Main.screenPosition, null, Color.White, NPC.rotation, origin, 1f, SpriteEffects.None, 0f);
            ArchaeaNPC.DrawChain(Mod.Assets.Request<Texture2D>("Gores/chain").Value, sb, new Vector2(NPC.position.X + (NPC.direction == 1 ? NPC.width : 0f), NPC.position.Y + NPC.height / 2), source.Center + corner);
        }
    }
}
