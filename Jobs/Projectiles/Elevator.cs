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
using System.Collections.Generic;
using rail;

namespace ArchaeaMod.Jobs.Projectiles
{
    public class Elevator : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Elevator");
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 80;
            Projectile.timeLeft = 2000;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.hide = true;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
        public bool docked = true;
        public bool onSolidGround => Main.tile[(int)(Projectile.position.X + 8) / 16, (int)(Projectile.position.Y + Projectile.height + 24) / 16].HasTile && Main.tileSolid[Main.tile[(int)(Projectile.position.X + 24) / 16, (int)(Projectile.position.Y + Projectile.height + 24) / 16].TileType];
        public readonly float MaxLen = 16 * 300;
        public float HomeY;
        public readonly int chainLen = 12;
        public static Rectangle prjB, plrB;
        public static bool switched = false, connected;
        public static int switchTimer = 0;
        public bool AtHomeY()
        {
            return new Vector2(0, Projectile.Center.Y).Distance(new Vector2(0, HomeY)) <= 16 || Projectile.Center.Y <= HomeY;
        }
        public bool AtMaxLength()
        {
            return new Vector2(0, Projectile.Center.Y).Distance(new Vector2(0, HomeY)) >= MaxLen;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float light = Lighting.Brightness((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16);
            Color color = Color.White;
            lightColor.R = (byte)(color.R * Math.Min(light, 1f));
            lightColor.G = (byte)(color.G * Math.Min(light, 1f));
            lightColor.B = (byte)(color.B * Math.Min(light, 1f));
            float x = Projectile.position.X + new Vector2(Projectile.width / 2 + chainLen / 2).X;
            ArchaeaNPC.DrawChain(TextureAssets.Chain.Value, Main.spriteBatch, new Vector2(x, Projectile.position.Y + chainLen / 2), new Vector2(x, HomeY + 40), chainLen);
            return true;
        }
        bool init = false;
        public override void AI()
        {
            if (!init)
            {
                HomeY = Projectile.position.Y;
                Projectile.position.X += 18;
                Projectile.position.Y += 80 - 64;
                init = true;
            }
            Projectile.timeLeft = 2;
            Player player = Main.player[Main.myPlayer];
            if (ArchaeaNPC.IsNotOldPosition(Projectile))
            {
                Projectile.netUpdate = true;
            }
            if (!AtMaxLength())
            {
                if (!docked && !onSolidGround)
                {
                    Projectile.position.Y += 8f / 60f;
                }
            }
            if (connected && player.controlJump)
            {
                player.velocity.Y = -Player.jumpSpeed * 2f;
                connected = false;
            }
            if (switched) switchTimer--;
            if (switchTimer <= 0)
            {
                switchTimer = 30;
                switched = false;
            }
            prjB = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            plrB = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
            if (plrB.Intersects(prjB) && ArchaeaMain.elevatorButton.JustPressed && !connected && !switched)
            {
                connected = true;
                switched = true;
            }
            if (plrB.Intersects(prjB) && ArchaeaMain.elevatorButton.JustPressed && connected && !switched)
            {
                connected = false;
                switched = true;
            }
            player.GetModPlayer<ArchaeaPlayer>().elevatorConnected = connected;
            if (connected)
            {
                if (switched) Projectile.velocity.Y = 3f;
                if (player.controlDown && !AtMaxLength() && !onSolidGround) Projectile.velocity.Y = 3f;
                if (player.controlUp) Projectile.velocity.Y = -3f;
                if (!player.controlDown && Projectile.velocity.Y < 0) Projectile.velocity.Y -= 0.5f;
                if (!player.controlUp && Projectile.velocity.Y > 0) Projectile.velocity.Y += 0.5f;

                player.position = new Vector2(Projectile.position.X + 9f, Projectile.position.Y + 28);
                player.fallStart = (int)player.position.Y;
                int n = ModContent.TileType<Tiles.Elevator>();
                for (int i = (int)Projectile.position.X / 16; i < (int)(Projectile.position.X + Projectile.width) / 16; i++)
                    for (int j = (int)(Projectile.position.Y + Projectile.height) / 16; j < (int)(Projectile.position.Y + Projectile.height + 8f) / 16; j++)
                    {
                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType == n && !player.controlDown)
                        {
                            Projectile.velocity *= 0f;
                            return;
                        }
                        if (Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType] && Main.tile[i, j].TileType != 19 && !player.controlUp)
                        {
                            Projectile.velocity *= 0f;
                            return;
                        }
                    }
                if (AtMaxLength())
                {
                    Projectile.velocity *= 0f;
                }
                if (AtHomeY())
                {
                    if (player.controlUp || Projectile.velocity.Y < 0)
                    {
                        Projectile.position.Y = HomeY;
                        return;
                    }
                }
            }
            if (!connected) Projectile.velocity.Y = 0;
            if (Projectile.velocity.Y > 2f) Projectile.velocity.Y = 3f;
            if (Projectile.velocity.Y < -2f) Projectile.velocity.Y = -3f;
        }
    }
}