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
            AnimationFrameHeight = 74;
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
            MinPick = 15;
            HitSound = SoundID.Dig;
            DustType = 1;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Elevator");
            AddMapEntry(Color.BurlyWood, name);
        }
        bool init = false;
        NPC elevator;
        public override void NearbyEffects(int i, int j, bool closer)
        {
            int x = i * 16;
            int y = j * 16;
            if (!init)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_None(), new Vector2(x + 24, y + 80 - 64), Vector2.Zero, ModContent.ProjectileType<Projectiles.Elevator>(), 0, 0);
                init = true;
            }
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (Main.tile[i,j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 2*18)
            { 
                if (Main.rand.NextBool(6))
                {
                    Vector2 position = new Vector2(i * 16, j * 16);
                    int num123 = Dust.NewDust(position, 16, 16, 31, 0f, 0f, 80, default(Color), 1.4f);
                    Dust expr_5B99_cp_0 = Main.dust[num123];
                    expr_5B99_cp_0.position.X = expr_5B99_cp_0.position.X - 4f;
                    Main.dust[num123].noGravity = true;
                    Main.dust[num123].velocity *= 0.2f;
                    Main.dust[num123].velocity.Y = (float)(-(float)Main.rand.Next(7, 13)) * 0.15f;
                }
            }
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            ModContent.GetInstance<ArchaeaWorld>().elevatorCount++;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(Item.GetSource_None(), i * 16, j * 16, 64, 64, ModContent.ItemType<Jobs.Items.Elevator>());
            ModContent.GetInstance<ArchaeaWorld>().elevatorCount--;
        }
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            int interval = 6;
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
