using ArchaeaMod.Jobs.Global;
using ArchaeaMod.NPCs;
using IL.Terraria.GameContent.NetModules;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using rail;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static IL.Terraria.WorldBuilding.Searches;
using static System.Formats.Asn1.AsnWriter;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ArchaeaMod.Jobs.Items
{
    internal class Scroll_firestorm : ModItem
	{
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            { 
                var modPlayer = player.GetModPlayer<ArchaeaPlayer>();
                if (!modPlayer.fireStorm)
                { 
                    modPlayer.fireStorm = true;
                    SoundEngine.PlaySound(SoundID.Item8, player.Center);
                    if (Main.netMode == 1)
                    {
                        NetHandler.Send(Packet.CastFireStorm, i: player.whoAmI, b: true);
                    }
                    return true;
                }
                else return false;
            }
            return null;
		}
	}
}