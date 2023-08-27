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
        public const int MaxTime = 60;
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
            int maxTries = 100;
            if (buffTime == MaxTime - 1)
            {
                oldPosition = player.position;
                do
                { 
                    floor = ArchaeaNPC.AllSolidFloorsV2(player, 1000);
                    if (floor == Vector2.Zero) continue;
                    for (int i = 0; i < 2; i++)
                    for (int j = 0; j < 4; j++)
                    if (Main.tile[(int)floor.X / 16 + i, (int)floor.Y / 16 - j].HasTile && Main.tileSolid[Main.tile[(int)floor.X / 16 + i, (int)floor.Y / 16 - j].TileType])
                    {
                        continue;
                    }
                    break;
                } while (++tries < maxTries);
                if (tries == maxTries)
                {
                    player.statMana += player.statManaMax / 3;
                    player.DelBuff(buffIndex--);
                    return;
                }
                floor.Y -= player.height;
            }
            float weight = ((float)buffTime / MaxTime - 1f) * -1;
            if (buffTime == 1 || buffTime == MaxTime - 1)
            {
                int a = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 20, 0f, 0f, 100, default(Color), 1.5f);
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