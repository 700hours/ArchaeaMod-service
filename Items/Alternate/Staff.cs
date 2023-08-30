using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ArchaeaMod.Projectiles;
using Terraria.Audio;
using ArchaeaMod.NPCs;

namespace ArchaeaMod.Items.Alternate
{
    public class Staff : ModItem
    {
        public override string Texture => "ArchaeaMod/Items/c_Staff";
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Charred Staff"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip0", "Casts fire wave"));
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 40;
            Item.noMelee = true;
            Item.mana = 10;
            Item.value = 5000;
            Item.rare = ItemRarityID.Green;
            Item.useTime = 60;
            Item.useAnimation = 5;
            Item.useStyle = 5;
            Item.autoReuse = true;
            Item.channel = true;
            Item.DamageType = DamageClass.Magic;
        }

        private int time;
        private int second = 60;
        private int elapsed
        {
            get { return (int)(second * 1.5f); }
        }
        private int maxTime
        {
            get { return elapsed * 5; }
        }
        private int manaCost
        {
            get { return second / Item.useTime; }
        }
        private bool update = true;
        private int index;
        private int type = -1;
        public const int
            Reset = -1,
            Start = 0,
            Boost = 1,
            Hover = 2,
            Launch = 3;
        private float alpha;
        [CloneByReference]
        private Dust[] dust = new Dust[5];
        [CloneByReference]
        public Target[] targets;
        public override Vector2? HoldoutOrigin() => new Vector2(18, 8);
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            if (player.statMana <= 0)
                return false;
            if (update)
            {
                targets = Target.GetTargets(player, 135f).Where(t => t != null).ToArray();
                update = false;
            }
            return true;
        }
        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (ArchaeaItem.Elapsed(30) && player.controlUseItem)
            {
                player.CheckMana(manaCost, true);
                player.manaRegenDelay = 120;
            }
            if (time++ % elapsed == 0 && time != 0)
            {
                update = true;
                index++;
            }
            for (int i = 0; i < index; i++)
            {
                if (i < 5)
                {
                    dust[i] = Dust.NewDustDirect(player.Center - new Vector2(25f, 32f) + new Vector2(i * 12f, 0f), 1, 1, 6, 0f, 0f, 0, default(Color), 2f);
                    dust[i].noGravity = true;
                }
            }
            if (targets == null)
                return;
            if (index == 5)
            {
                foreach (Target target in targets.Where(t => t != null))
                    target.AttackEffect(Target.ShockWave);
                BlastWave(player);
                index = 0;
            }
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            ArchaeaItem.ActiveChannelStyle(player);
        }
        public void BlastWave(Player player)
        {
            for (float r = 0f; r < Math.PI * 2f; r += 0.017f * (45f / 15f))
            {
                Vector2 velocity = NPCs.ArchaeaNPC.AngleToSpeed(r, 15f);
                Projectile pixel = Projectile.NewProjectileDirect(Projectile.GetSource_None(), player.Center, velocity, ModContent.ProjectileType<Pixel>(), 0, 0f, player.whoAmI, Pixel.Fire, Pixel.Default);
                pixel.tileCollide = false;
                pixel.timeLeft = 15;
            }
            var npc = Main.npc.Where(t => t.active && !t.townNPC && !t.CountsAsACritter && t.Distance(player.Center) < 300f);
            foreach (NPC n in npc)
            { 
                int direction = n.Center.X < player.Center.X ? -1 : 1;
                n.StrikeNPC(40, 4f, direction, false, false, Main.netMode != 0);
                n.velocity.Y += 5f;
                n.velocity.X += 5f * direction;
            }
            SoundEngine.PlaySound(SoundID.Item14, player.Center);
        }
        protected void ResetItem()
        {
            dust = new Dust[5];
            time = 0;
            alpha = 0f;
        }
        public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/c_Staff").Value;
            sb.Draw(tex, position, frame, Color.Firebrick * 0.67f, 0f, origin, 1f, SpriteEffects.None, 0f);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/c_Staff").Value;
            sb.Draw(tex, Item.position - Main.screenPosition + new Vector2(0, 32), null, Color.Firebrick * 0.67f, 0f, new Vector2(tex.Width / 2, tex.Height / 2), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
    
}
