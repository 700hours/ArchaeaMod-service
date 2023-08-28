using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

using ArchaeaMod.NPCs;

namespace ArchaeaMod.Merged.Tiles
{
	public class m_ore : ModTile
	{
		public override void SetStaticDefaults()
		{
            Main.tileSpelunker[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            Main.tileMerge[Type][ArchaeaWorld.magnoStone] = true;
            DustType = 1;
			ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Merged.Items.Tiles.magno_ore>();
            //  UI map tile color
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Rubidium");
            AddMapEntry(new Color(201, 152, 115), name);
           // soundStyle = 0;
            HitSound = SoundID.Tink;
            MineResist = 1.5f;
            MinPick = 35;
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            type = ModContent.DustType<Dusts.magno_dust>();
            return true;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override bool Drop(int i, int j)/* tModPorter Note: Removed. Use CanDrop to decide if an item should drop. Use GetItemDrops to decide which item drops. Item drops based on placeStyle are handled automatically now, so this method might be able to be removed altogether. */
        {
            float chance = Main.rand.NextFloat();
            if (Main.netMode == 2)
                NetHandler.Send(Packet.TileExplode, -1, -1, i, chance, 0.3f, j);
            if (chance >= 0.30f)
                return true;
            if (Main.netMode == 0)
                TileExplode(i, j);
            return true;
        }
        public static void TileExplode(int i, int j)
        {
            int x = i * 16 + 8;
            int y = j * 16 + 8;
            float range = 3f * 16;
            Vector2 center = new Vector2(x, y);
            Player[] proximity = Main.player.Where(t => t.Distance(center) < range).ToArray();
            for (float k = 0; k < Math.PI * 2f; k++)
            {
                for (int l = 0; l < range; l++)
                {
                    Vector2 velocity = ArchaeaNPC.AngleToSpeed(k, 3f);
                    int rand = Main.rand.Next(20);
                    if (rand == 0)
                        Dust.NewDustDirect(ArchaeaNPC.AngleBased(center, k, range), 1, 1, DustID.Smoke, velocity.X, velocity.Y, 0, default(Color), 2f);
                    if (rand == 10)
                        Dust.NewDustDirect(ArchaeaNPC.AngleBased(center, k, l), 4, 4, 6, 0f, 0f, 0, default(Color), 2f);
                }
            }
            foreach (Player player in proximity)
            {
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " struck dead in a mining accident"), 10, player.position.X / 16 < i ? -1 : 1);
                if (Main.netMode == 2)
                    NetMessage.SendData(MessageID.PlayerHurtV2, player.whoAmI, -1, null);
            }
        }
    }
}
