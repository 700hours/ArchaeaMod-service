using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchaeaMod.Projectiles
{
    public class PossessedBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Possessed Bullet");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 2;
            Projectile.damage = 6;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.stepSpeed = 6f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
        }
        SpriteBatch sb => Main.spriteBatch;
        Vector2 impact = Vector2.Zero;
        int npc = 0;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            impact = ArchaeaMain.Impact(Projectile, target.Hitbox);
            npc = target.whoAmI;
            int index = NPC.NewNPC(NPC.GetSource_None(), (int)impact.X, (int)impact.Y, ModContent.NPCType<NPCs.PossessedBullet>(), 0, Projectile.damage, Projectile.owner, Target: npc);
            if (Main.netMode == 1)
            {
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, index);
            }
        }
        public override void AI()
        {
            if (Projectile.velocity.X < 0f && Projectile.oldVelocity.X >= 0f || Projectile.velocity.X > 0f && Projectile.oldVelocity.X <= 0f || Projectile.velocity.Y < 0f && Projectile.oldVelocity.Y >= 0f || Projectile.velocity.Y > 0f && Projectile.oldVelocity.Y <= 0f)
                Projectile.netUpdate = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Gores/MagnoBullet").Value;
            sb.Draw(tex, Projectile.position - Main.screenPosition, null, Color.SkyBlue * 0.9f, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1.1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
