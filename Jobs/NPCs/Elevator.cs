using System;

using ArchaeaMod.NPCs.Bosses;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.IO;
using Terraria.GameContent;

namespace ArchaeaMod.Jobs.NPCs
{
    public class Elevator : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Elevator");
        }
        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 80;
            NPC.timeLeft = 2000;
            NPC.friendly = true;
            NPC.lavaImmune = true;
            NPC.immortal = true;
            NPC.HitSound = SoundID.Dig;
            NPC.hide = true;
        }
        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCsOverPlayers.Add(index);
            Main.instance.DrawCacheNPCsBehindNonSolidTiles.Add(index);
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public bool docked = true;
        public bool onSolidGround => Main.tile[(int)(NPC.position.X + 8) / 16, (int)(NPC.position.Y + NPC.height + 8) / 16].HasTile && Main.tileSolid[Main.tile[(int)(NPC.position.X + 8) / 16, (int)(NPC.position.Y + NPC.height + 8) / 16].TileType];
        public readonly float MaxLen = 16 * 300;
        public float HomeY => NPC.ai[0];
        public readonly int chainLen = 12;
        public static Rectangle prjB, plrB;
        public static bool switched = false, connected;
        public static int switchTimer = 0;
        public bool AtHomeY()
        {
            return new Vector2(0, NPC.Center.Y).Distance(new Vector2(0, HomeY)) <= 16 || NPC.Center.Y <= HomeY;
        }
        public bool AtMaxLength()
        {
            return new Vector2(0, NPC.Center.Y).Distance(new Vector2(0, HomeY)) >= MaxLen;
        }
        public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            float x = NPC.position.X + new Vector2(NPC.width / 2 + chainLen / 2).X;
            ArchaeaNPC.DrawChain(TextureAssets.Chain.Value, sb, new Vector2(x, NPC.position.Y + chainLen / 2) - screenPos, new Vector2(x, HomeY) - screenPos, chainLen);
            return true;
        }
        public override void AI()
        {
            Player player = Main.player[Main.myPlayer];
            if (ArchaeaNPC.IsNotOldPosition(NPC))
            {
                NPC.netUpdate = true;
            }
            if (!AtMaxLength())
            {
                if (!docked && !onSolidGround)
                {
                    NPC.position.Y += 8f / 60f;
                }
            }
            if (player.controlJump)
            {
                connected = false;
            }
            if (switched) switchTimer--;
            if (switchTimer <= 0)
            {
                switchTimer = 30;
                switched = false;
            }
            if (NPC.timeLeft < 100) NPC.timeLeft = 2000;
            prjB = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
            plrB = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
            if (plrB.Intersects(prjB) && Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.X) < 0 && !connected && !switched)
            {
                connected = true;
                switched = true;
            }
            if (plrB.Intersects(prjB) && Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.X) < 0 && connected && !switched)
            {
                connected = false;
                switched = true;
            }
            if (connected)
            {
                player.position = new Vector2(NPC.position.X + 6f, NPC.position.Y - 16f);
                player.fallStart = (int)player.position.Y;
                int n = ModContent.TileType<Tiles.Elevator>();
                for (int i = (int)NPC.position.X / 16; i < (int)(NPC.position.X + NPC.width) / 16; i++)
                    for (int j = (int)(NPC.position.Y + NPC.height) / 16; j < (int)(NPC.position.Y + NPC.height + 8f) / 16; j++)
                    {
                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType == n && !player.controlDown)
                        {
                            NPC.velocity *= 0f;
                            return;
                        }
                        if (Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType] && Main.tile[i, j].TileType != 19 && !player.controlUp)
                        {
                            NPC.velocity *= 0f;
                            return;
                        }
                    }
                if (AtMaxLength())
                {
                    NPC.velocity *= 0f;
                }
                if (AtHomeY())
                {
                    if (player.controlUp || NPC.velocity.Y < 0)
                    { 
                        NPC.position.Y = HomeY;
                        return;
                    }
                }
                if (player.controlDown && !AtMaxLength()) NPC.velocity.Y += 0.2f;
                if (player.controlUp) NPC.velocity.Y -= 0.2f;
                if (!player.controlDown && NPC.velocity.Y < 0) NPC.velocity.Y -= 0.5f;
                if (!player.controlUp && NPC.velocity.Y > 0) NPC.velocity.Y += 0.5f;
            }
            if (!connected) NPC.velocity.Y = 0;
            if (NPC.velocity.Y > 5f) NPC.velocity.Y = 5f;
            if (NPC.velocity.Y < -5f) NPC.velocity.Y = -5f;
        }
        public override bool PreKill()
        {
            return false;
        }
        public override bool CheckActive()
        {
            return false;
        }
    }
}