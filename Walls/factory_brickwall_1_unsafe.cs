using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Walls
{
    public class factory_brickwall_1_unsafe : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            ItemDrop = ModContent.ItemType<Items.Walls.factory_brickwall_1>();
            AddMapEntry(new Color(80, 10, 10));
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.1f;
            g = 0.0f;
            b = 0.0f;
            if (Main.tile[i, j].HasTile)
            {
                r = 0f;
            }
        }
        public override void KillWall(int i, int j, ref bool fail)
        {
            Player plr = Main.player.OrderBy(t => t.Distance(new Vector2(i * 16, j * 16))).First();
            if (plr.HeldItem.hammer >= 100)
            {
                fail = false;
            }
            fail = true;
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
