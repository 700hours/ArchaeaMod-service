using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArchaeaMod.Walls
{
    public class magno_cavewall_unsafe : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(Color.DarkRed);
        }
    }
}
