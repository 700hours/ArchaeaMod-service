using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class PossessedBullet : ModNPC
    {
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possessed Bullet");
        }
        public override void SetDefaults()
        {
            NPC.width = 1;
            NPC.height = 1;
            NPC.hide = true;
            NPC.friendly = true;
            NPC.aiStyle = -1;
            NPC.lifeMax = 650;
            NPC.defense = 10;
            NPC.damage = 0;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }
        public override void AI()
        {
            NPC target = Main.npc[NPC.target];
            if (target.active && target.life > 0)
            {
                NPC.position += target.velocity;
                Vector2 v2 = NPC.position + new Vector2(target.width * (target.direction == 1 ? 1 : 0), 0);
                int dust = Dust.NewDust(v2, 3, 3, DustID.Ash, 0, 0, 0, Color.DarkGray, 1.2f);
                Main.dust[dust].noGravity = true;
                if ((int)Main.time % 60 == 0)
                {
                    target.StrikeNPC(20, 0f, 0, false, true);
                    Dust.NewDust(v2, 3, 3, DustID.Smoke, 0, -3f, 0, default, 2.5f);
                }
            }
            else NPC.active = false;
        }
    }
}
