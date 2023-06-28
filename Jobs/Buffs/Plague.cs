using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Buffs;
using ArchaeaMod.Jobs.Global;
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
    internal class Plague : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plague");
        }
        int ticks = 0;
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (ArchaeaItem.Elapsed(ref ticks, 30))
            {
                npc.StrikeNPC(5, 0f, 0, fromNet: Main.netMode != 0);
                SoundEngine.PlaySound(SoundID.NPCHit1, npc.Center); 
                int index = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GreenBlood, Main.rand.NextFloat(-2f, 2f), 2f, 0, default, 2f);
                Main.dust[index].noGravity = false;
                ticks = 0;
            }
        }
    }
}
