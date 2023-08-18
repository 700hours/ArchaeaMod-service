using ArchaeaMod.NPCs;
using IL.Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Buffs
{
    internal class Phase : ModBuff
    {
        public const int MaxTime = 300;
        Vector2 oldPosition = Vector2.Zero;
        Vector2 floor = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void Update(Player player, ref int buffIndex)
        {
            int buffTime = player.buffTime[buffIndex];
            int tries = 0;
            int maxTries = 10;
            if (buffTime == MaxTime)
            {
                oldPosition = player.position;
                do
                { 
                    floor = ArchaeaNPC.AllSolidFloorsV2(player, 1000);
                    if (floor == Vector2.Zero) continue;
                    for (int i = 0; i < player.width / 16; i++)
                    for (int j = 0; j < player.height / 16; j++)
                    if (Main.tile[(int)floor.X / 16 + i, (int)floor.Y / 16 - player.height / 16 + j].HasTile && Main.tileSolid[Main.tile[(int)floor.X / 16 + i, (int)floor.Y / 16 - player.height / 16 + j].TileType])
                    {
                        continue;
                    }
                } while (++tries < maxTries);
                if (tries == maxTries)
                {
                    player.DelBuff(buffIndex--);
                    return;
                }
                floor.Y -= player.height;
            }
            float weight = ((float)buffTime / MaxTime - 1f) * -1;
            if (weight == 0 || weight == 1)
            {
                int a = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 20, 0f, 0f, 100, default(Color), 2.5f);
                Main.dust[a].noGravity = true;
            }
            if (weight > 0)
            {
                player.lavaImmune = player.lavaWet; 
                player.position = Vector2.Lerp(oldPosition, floor, weight);
            }
        }
    }
}