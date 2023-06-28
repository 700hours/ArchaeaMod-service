using Humanizer;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using Newtonsoft.Json;
using System.Runtime.Intrinsics.X86;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Items
{
	public class ChurchBlueprint : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Church Blueprint");
            Tooltip.SetDefault("Use to begin construction on a church.");
        }
        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer)
            {
                var modPlayer = player.GetModPlayer<ArchaeaPlayer>();
                if (modPlayer.blueprint == Rectangle.Empty)
                {
                    int width = 12 * 16;
                    int height = 8 * 16;
                    modPlayer.blueprint = new Rectangle(Main.screenWidth / 2 - width / 2, Main.screenHeight / 2 - height / 2, width, height);
                }
                else return false;
                return true;
            }
            return false;
        }

        public static void SyncTile(int i, int j, int whoAmI)
        {
            if (Main.netMode == 2)
            {
                NetMessage.SendTileSquare(whoAmI, i, j);
            }
        }
    }
}