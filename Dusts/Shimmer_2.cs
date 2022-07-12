using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Dusts
{
    public class Shimmer_2 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale = 1.2f;
            dust.velocity /= 4f;
            dust.color = Color.White;
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X;
            dust.scale -= 0.05f;
            Lighting.AddLight((int)dust.position.X / 16, (int)dust.position.Y / 16, 0.460f, 0.160f, 0.160f);
            if (dust.scale <= 0.30f)
            {
                dust.active = false;
            }

            return true;
        }
    }
}
