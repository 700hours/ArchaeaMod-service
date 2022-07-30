using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Merged.Walls
{
    public class magno_stone : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            TileID.Sets.HousingWalls[Type] = true;
            ItemDrop = ModContent.ItemType<Merged.Items.Walls.magno_stonewall>();
            AddMapEntry(new Color(10, 10, 110));
        }
    }
}
