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
            ItemDrop = Mod.Find<ModItem>("magno_stonewall").Type;
            AddMapEntry(new Color(10, 10, 110));
        }
    }
}
