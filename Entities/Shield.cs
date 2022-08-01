using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ArchaeaMod.NPCs;

namespace ArchaeaMod.Entities
{
    public class MagnoShield : ArchaeaEntity
    {
        private float start
        {
            get { return ai[0]; }
        }
        private float radius = 64f;
        private float orbit;
        private const float radian = 0.017f;
        public override void SetDefaults()
        {
            width = 18;
            height = 40;
            type = ArchaeaEntity.ID.Shield;
        }
        public override void Update()
        {
            netUpdate = true;
            Player player = Main.player[owner];
            rotation = ArchaeaNPC.AngleTo(player.Center, Center);
            orbit = (float)Math.Round(orbit + (radian * Math.Min(player.statLifeMax / Math.Max(player.statLife, 1) + 2f, 6f)), 2);
            if (orbit >= Math.Round(Math.PI * 2f, 2))
                orbit = 0f;
            double cos = player.Center.X + (float)(radius * ((float)player.statLife / player.statLifeMax2)) * Math.Cos(start + orbit);
            double sine = player.Center.Y + (float)(radius * ((float)player.statLife / player.statLifeMax2)) * Math.Sin(start + orbit);
            Center = new Vector2((float)cos, (float)sine);
            if (!player.active || Items.ArchaeaItem.NotEquipped(player, ModContent.ItemType<Items.m_shield>()))
                Kill(true);
        }
        public override void Kill(bool effect)
        {
            if (effect && active)
                ArchaeaNPC.DustSpread(Center, 2, 2, ModContent.DustType<Merged.Dusts.magno_dust>(), 8, 1.2f, Color.White, false, 2f);
            active = false;
        }
    }
}