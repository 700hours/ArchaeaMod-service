using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ArchaeaMod.Backgrounds
{
    public class bg_style : ModUndergroundBackgroundStyle
    {
        public static int Style;
        public override void SetStaticDefaults()
        {
            Style = this.Slot;
        }
        public override void FillTextureArray(int[] textureSlots)
        {
            textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot("ArchaeaMod/Backgrounds/bg_magno");
            textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot("ArchaeaMod/Backgrounds/bg_magno_surface");
            textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot("ArchaeaMod/Backgrounds/bg_magno_connector");
            textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot("ArchaeaMod/Backgrounds/bg_magno");
        }
    }   
} 