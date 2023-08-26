using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Buffs;

using ArchaeaMod.Jobs.Projectiles;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Runtime.Intrinsics.X86;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Buffs
{
    internal class Summoned : ModBuff
    {
        public const int MaxTime = 180;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Just Summoned");
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            float timeLeft = npc.buffTime[buffIndex];
            if (npc.oldPosition != Vector2.Zero)
            { 
                npc.velocity = Vector2.Zero;
                npc.scale = (timeLeft / MaxTime - 1f) * -1;
                npc.color = Color.Lerp(Color.Black, Color.White, npc.scale);
            }
        }
    }
}
