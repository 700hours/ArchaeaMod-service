using Terraria;
using Terraria.ModLoader;

namespace ArchaeaMod.Backgrounds
{
    public class bg_style : ModUndergroundBackgroundStyle
    {
        public static int Style;
        public override void Load()
        {
            Style = this.Slot;
        }
        public override void FillTextureArray(int[] textureSlots)
        {
            try
            { 
                textureSlots[0] = ModContent.GetModBackgroundSlot("Backgrounds/bg_magno");
                textureSlots[1] = ModContent.GetModBackgroundSlot("Backgrounds/bg_magno_surface");
                textureSlots[2] = ModContent.GetModBackgroundSlot("Backgrounds/bg_magno_connector");
                textureSlots[3] = ModContent.GetModBackgroundSlot("Backgrounds/bg_magno");
            }
            catch (System.Exception e)
            {
                return;
            }
        }
    }   
} 