using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArchaeaMod.Merged.Tiles
{
	public class c_ore : ModTile
	{
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override void SetStaticDefaults()
		{
            Main.tileSpelunker[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
            Main.tileSolid[Type] = true;
            //  connects with dirt
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            DustType = 1;
			ItemDrop = ModContent.ItemType<Merged.Items.Tiles.cinnabar_ore>();
            //  UI map tile color
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Cinnabar Ore");
            AddMapEntry(new Color(201, 152, 115), name);
            MineResist = 1.8f;
            MinPick = 35;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            type = ModContent.DustType<Dusts.cinnabar_dust>();
            return true;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
