using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using ArchaeaMod.Items;
using ArchaeaMod.NPCs;

namespace ArchaeaMod.Jobs.Tiles
{
    public class Elevator : ModTile
    {
        public override void SetStaticDefaults()
        {
            AnimationFrameHeight = 64;
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileFrameCounter[Type] = 4;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 1, 0);
            TileObjectData.newTile.Origin = new Point16(0, 3);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(1, 3);
            TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0);
            TileObjectData.addAlternate(0);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(2, 3);
            TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0);
            TileObjectData.addAlternate(1);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(3, 3);
            TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0);
            TileObjectData.addAlternate(2);
            TileObjectData.addTile(Type);
            ItemDrop = ModContent.ItemType<Jobs.Items.Elevator>();
            MinPick = 15;
            HitSound = SoundID.Dig;
            DustType = 1;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Elevator");
            AddMapEntry(Color.BurlyWood, name);
        }
        NPC elevator;
        public override void NearbyEffects(int i, int j, bool closer)
        {
            int x = i * 16;
            int y = j * 16;
            if (elevator == default(NPC) || !elevator.active)
            {
                elevator = NPC.NewNPCDirect(NPC.GetSource_None(), new Vector2(x + 24, y + 80 - 64), ModContent.NPCType<NPCs.Elevator>(), 0, y + 80 - 64);
            }
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (ArchaeaItem.Elapsed(80))
            {
                float scale = Main.rand.NextFloat() + 1f;
                var v2 = new Vector2((i + 1)* 16 + 16, (j + 3) * 16);
                var dust = Dust.NewDust(v2, 2, 3, DustID.Smoke, ArchaeaNPC.RandAngle(), 3 * scale, 0, default, 3 * scale);
                Main.dust[dust].noLight = false;
                Main.dust[dust].noGravity = true;
            }
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            ModContent.GetInstance<ArchaeaWorld>().elevatorCount++;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<ArchaeaWorld>().elevatorCount--;
        }
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            int interval = 3;
            int maxFrames = 3;
            if (frameCounter++ % interval == 0)
            {
                if (frame < maxFrames)
                {
                    frame++;
                }
                else frame = 0;
            }
        }
    }
}
