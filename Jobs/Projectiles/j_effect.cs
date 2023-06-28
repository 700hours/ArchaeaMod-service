using ArchaeaMod.Effects;
using ArchaeaMod.Jobs.Global;
using ArchaeaMod.NPCs;
using IL.Terraria.GameContent.NetModules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using rail;
using Steamworks;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Projectiles
{
    internal static class EffectID
    {
        public const byte
            None = 0,
            Polygon = 1;
    }
    internal class j_effect : ModProjectile
    {
        public override string Texture => "ArchaeaMod/Gores/Null";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("j_Effect");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1; 
            Projectile.alpha = 255;
            Projectile.noEnchantments = true;
            Projectile.scale = 1 / 48f;
            Projectile.tileCollide = false;
            Projectile.damage = 0;
            Projectile.knockBack = 0f;
            Projectile.ignoreWater = true;
            Projectile.lavaWet = false;
            Projectile.timeLeft = 600;
        }
        bool init = false;
        int ai => (int)Projectile.ai[0];
        int npcIndex => (int)Projectile.ai[1];
        NPC target => Main.npc[npcIndex];
        Geometric polygon;
        private void Initialize()
        {
            if (ai == EffectID.Polygon)
            {
                polygon = Geometric.NewEffect(GetVertices(), Projectile.position, new float[] { 120f * Draw.radian, 240f * Draw.radian, 360f * Draw.radian });
            }
        }
        public override bool PreAI()
        {
            if (!init)
            {
                Initialize();
                init = true;
            }
            switch (ai)
            {
                case EffectID.Polygon:
                    polygon.origin = Projectile.position.ToScreenPosition();
                    Projectile.timeLeft = 2;
                    break;
            }
            return ai > 0;
        }
        public override void AI()
        {
            polygon.UpdateRotation(target.height);
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = polygon.DrawTexture();
            SpriteBatch sb = Main.spriteBatch;
            sb.Draw(tex, polygon.origin, null, Color.Lerp(lightColor, Color.Red, (float)(polygon.Rotation / Math.PI)));
        }
        Vector2[][] GetVertices()
        {
            return new Vector2[][]
            {
                new Vector2[]
                {
                    Projectile.position + new Vector2(-30),
                    Projectile.position + new Vector2(30, 0),
                    Projectile.position + new Vector2(-30, 30)
                },
                new Vector2[]
                {
                    Projectile.position + new Vector2(30),
                    Projectile.position + new Vector2(-30, 0),
                    Projectile.position + new Vector2(30, -30)
                }
            };
        }
    }
}
