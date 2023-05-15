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
        private decimal start
        {
            get { return (decimal)ai[0]; }
        }
        private decimal radius = 64m;
        private decimal orbit;
        private const decimal radian = 0.017m;
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
            orbit = Math.Round(orbit + (radian * Math.Min((decimal)player.statLifeMax / Math.Max(player.statLife, 1m) + 2m, 6m)), 2);
            if (orbit >= Math.Round((decimal)Math.PI * 2m, 2))
                orbit = 0m;
            decimal cos = (decimal)player.Center.X + (radius * ((decimal)player.statLife / player.statLifeMax2)) * (decimal)Math.Cos((double)(start + orbit));
            decimal sine = (decimal)player.Center.Y + (radius * ((decimal)player.statLife / player.statLifeMax2)) * (decimal)Math.Sin((double)(start + orbit));
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