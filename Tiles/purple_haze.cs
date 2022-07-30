using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Tiles
{
    public class purple_haze : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = false;
            ItemDrop = ModContent.ItemType<Items.Tiles.purple_haze>();
            AddMapEntry(Color.MediumPurple);
            MinPick = 95;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = Color.Purple.R / 5;
            g = Color.Purple.G / 5;
            b = Color.Purple.B / 5;
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (Main.rand.Next(60) == 0)
            {
                var dust = Dust.NewDust(new Vector2(i * 16 + 8, j * 16 + 8), 1, 1, DustID.PurpleTorch);
                Main.dust[dust].noLight = false;
                Main.dust[dust].noGravity = true;
            }
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return ArchaeaPlayer.AccIsEquipped(Main.LocalPlayer, ModContent.ItemType<Items.dream_catcher>());
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (!closer) return;
            if (!ModContent.GetInstance<ArchaeaWorld>().downedNecrosis)
            {
                for (int n = 0; n < Main.player.Length; n++)
                {
                    if (Main.player[n] == null) 
                        continue;
                    if (ArchaeaPlayer.AccIsEquipped(Main.player[n], ModContent.ItemType<Items.dream_catcher>()))
                        continue;
                    if (!Main.player[n].active) 
                        continue;
                    if (Main.player[n].dead) 
                        continue;
                    if (Main.player[n].Distance(new Vector2(i * 16, j * 16)) < 80f)
                    {
                        Main.player[n].AddBuff(BuffID.Slow, 180);
                        Main.player[n].AddBuff(BuffID.Poisoned, 300);
                    }
                }
            }
        }
    }
}
