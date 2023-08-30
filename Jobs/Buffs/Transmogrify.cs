using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Buffs;

using ArchaeaMod.Jobs.Projectiles;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Transactions;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Buffs
{
    internal class Transmogrify : ModBuff
    {
        public const int MaxTime = 180;
        float oldScale = 0;
        NPC nPC = default;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Transmogrified");
        }
    //  turn players into an NPC and then back when the buff expires
        public override void Update(NPC npc, ref int buffIndex)
        {
            float timeLeft = npc.buffTime[buffIndex];
            if (timeLeft == MaxTime - 1)
            {
                oldScale = npc.scale;
                int tries = 0;
                int count = Main.npc.Count(t => t.active && !t.friendly && !t.boss && !t.townNPC && !t.CountsAsACritter && !ModNPCID.Follower(t.type));
                do
                {
                    bool any = Main.npc.Any(t => t.active && !t.friendly && !t.boss && !t.townNPC && !t.CountsAsACritter && !ModNPCID.Follower(t.type));
                    if (!any)
                    {
                        return;
                    }
                    nPC = Main.npc[Main.rand.Next(Main.npc.Length)];
                    if (nPC.active && !nPC.friendly && !nPC.boss && !nPC.townNPC && !nPC.CountsAsACritter && !ModNPCID.Follower(nPC.type))
                    {
                        return;
                    }
                } while (++tries < count);
                if (tries == count)
                { 
                    npc.DelBuff(buffIndex--);
                    return;
                }
            }
            if (npc.oldPosition != Vector2.Zero)
            {
                npc.velocity = Vector2.Zero;
                npc.scale = timeLeft / MaxTime;
                npc.color = Color.Lerp(Color.Black, Color.White, npc.scale);
            }
            if (timeLeft == 1)
            {
                npc.scale = oldScale;
                npc.Transform(nPC.type);
            }
        }
    }
}
