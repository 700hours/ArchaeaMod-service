using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Walls
{
    public class factory_brickwall_1 : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            TileID.Sets.HousingWalls[Type] = true;
            ItemDrop = ModContent.ItemType<Items.Walls.factory_brickwall_1>();
            AddMapEntry(new Color(80, 10, 10));
        }
    }
}
