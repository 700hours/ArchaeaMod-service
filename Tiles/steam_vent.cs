using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchaeaMod.Items;
using ArchaeaMod.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArchaeaMod.Tiles
{
    public class steam_vent : ModTile
    {
        int ticks = 0;
        int x = 0;
        int y = 0;
        NPC oldNpc;
        NPC npc;
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
            TileObjectData.newTile.AnchorValidTiles = new int[] { ArchaeaWorld.magnoStone };
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Steam vent");
            AddMapEntry(new Color(210, 110, 110), name);
            DustType = ModContent.DustType<Merged.Dusts.magno_dust>(); 
            MineResist = 1.2f;
            MinPick = 45;
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (ticks == 0)
            {
                foreach (NPC n in Main.npc)
                {
                    if (!n.active) continue;
                    if (n.noGravity) continue;
                    if (n.noTileCollide) continue;
                    if (n.CountsAsACritter) continue;
                    for (int l = 0; l < n.width / 16; l++)
                    {
                        x = (int)n.position.X / 16;
                        y = (int)(n.position.Y + n.height - 8) / 16;
                        if (Factory.GetSafely(x + l, y).TileType == Type)
                        {
                            SoundEngine.PlaySound(SoundID.Item34, new Vector2(x, y));
                            oldNpc = npc;
                            npc = n;
                            ticks++;
                            goto EFFECT;
                        }
                    }
                }
                Player player = Main.LocalPlayer;
                x = (int)player.position.X / 16;
                y = (int)(player.position.Y + player.height - 8) / 16;
                for (int l = 0; l < player.width / 16; l++)
                {
                    if (Factory.GetSafely(x + l, y).TileType == Type)
                    {
                        SoundEngine.PlaySound(SoundID.Item34, new Vector2(x, y));
                        ticks++;
                        goto EFFECT;
                    }
                }
            }
            else goto EFFECT;
            return;
            EFFECT:
            {
                if (ticks++ > 120 - 1)
                {
                    ticks = 0;
                    return;
                }
                if (ArchaeaItem.Elapsed(ticks, 120))
                {
                    SoundEngine.PlaySound(SoundID.Item34, new Vector2(x, y));
                }
                if (ArchaeaItem.Elapsed(5))
                {
                    GeyserEffect(i, j);
                }
            }
        }
        public override void RandomUpdate(int i, int j)
        {
            GeyserEffect(i, j);
        }
        private void GeyserEffect(int i, int j)
        {
            int x = i * 16;
            int y = j * 16;
            Vector2 v2 = new Vector2(x, y);
            Dust.NewDust(v2, 8, 8, DustID.SteampunkSteam, Main.rand.NextFloat(-1, 1), -8f, 0, default, 3f);
            Dust.NewDust(v2, 4, 4, DustID.Water, Main.rand.NextFloat(-1, 1), -8f, 0, default, 1.5f);
        }
    }
}