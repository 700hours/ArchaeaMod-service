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

namespace ArchaeaMod.NPCs.Bosses
{
    internal class factory_01 : ModNPC
    {
        public override string Texture => "ArchaeaMod/Gores/arrow";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Factory Computer");
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
        int screenY = Main.screenHeight;
        int ai = -1;
        int oldLife;
        bool init = false;
        Color color = Color.Red;
        Rectangle hitbox => new Rectangle(0, screenY, Main.screenWidth, Main.screenHeight);
        public override void AI()
        {
            if (!init)
            {
                oldLife = NPC.lifeMax;
                ai = 10;
                init = true;
            }
            if (ai == 10)
            {
                screenY -= 16;
                ai = 0;
            }
            if (ai % 2 == 0)
            {
                if (ArchaeaItem.Elapsed(300))
                {
                    NPC.TargetClosest(false);
                    SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                    int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<t_effect>(), 10, 0f, Main.myPlayer, 10, NPC.target);
                    Main.projectile[proj].localAI[0] = ModContent.BuffType<Weaken>();
                    Main.projectile[proj].localAI[1] = DustID.PinkTorch;
                    ai++;
                }
            }
            else
            {
                if (ArchaeaItem.Elapsed(300))
                {
                    ArchaeaItem.DustCircle(NPC.Center, ModContent.DustType<Merged.Dusts.cinnabar_dust>());
                    SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                    for (int i = 0; i < 5; i++)
                    { 
                        int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<t_effect>(), 20, 0f, Main.myPlayer, 20, NPC.target);
                        Main.projectile[proj].localAI[0] = ModContent.BuffType<Buffs.stun>();
                        Main.projectile[proj].localAI[1] = DustID.AncientLight;
                    }
                    ai++;
                }
            }
            foreach (Player plr in Main.player)
            {
                if (new Rectangle(hitbox.X + (int)Main.screenPosition.X, hitbox.Y + (int)Main.screenPosition.Y, hitbox.Width, hitbox.Height).Contains(plr.Center.ToPoint()))
                {
                    plr.AddBuff(ModContent.BuffType<Buffs.mercury>(), 180);
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(MessageID.AddPlayerBuff, plr.whoAmI, -1, null, ModContent.BuffType<Buffs.mercury>());
                    }
                }
            }
            if (oldLife > NPC.life)
            {
                screenY += oldLife - NPC.life;
                oldLife = NPC.life;
            }
            if (ArchaeaItem.Elapsed(10)) screenY -= (int)(2 / ((float)NPC.life / NPC.lifeMax));
            if (screenY < 0) screenY = 0;
            if (screenY > Main.screenHeight) screenY = Main.screenHeight;
        }
        public override void PostDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            sb.Draw(TextureAssets.MagicPixel.Value, new Vector2(0, screenY - 10), new Rectangle(0, 0, Main.screenWidth, 10), color * 0.5f);
            sb.Draw(TextureAssets.MagicPixel.Value, new Vector2(0, screenY), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color * 0.25f);
        }
    }
}
