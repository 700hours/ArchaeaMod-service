using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchaeaMod.Items
{
    public class GhostlyChains : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ghostly Chains");
            // Tooltip.SetDefault("Chain gang");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 20000;
            Item.mana = 18;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.channel = false;
            Item.noMelee = true;
            Item.shootSpeed = 5f;
            Item.knockBack = 1f;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.ghostly_chains>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.LightPurple;
            Item.DamageType = DamageClass.Summon;
            //Item.UseSound = SoundID.?     Throw sound
        }
        int ticks = 0;
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.ghostly_chains>()] == 0;
        }
        public override bool? UseItem(Player player)
        {
            if (ticks++ > 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(Item.GetSource_ItemUse(Item), player.Center + new Vector2(ArchaeaItem.StartThrowX(player), 0), NPCs.ArchaeaNPC.AngleToSpeed(player.Center.AngleTo(Main.MouseWorld), Item.shootSpeed), ModContent.ProjectileType<Projectiles.ghostly_chains>(), 0, 0f, player.whoAmI);
                }
                ticks = 0;
                return true;
            }
            return false;
        }
        public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Gores/GhostlyChains").Value;
            sb.Draw(tex, position, frame, Color.SkyBlue * 0.67f, 0f, origin, 1f, SpriteEffects.None, 0f);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Gores/GhostlyChains").Value;
            sb.Draw(tex, Item.position - Main.screenPosition, null, Color.SkyBlue * 0.67f, 0f, new Vector2(tex.Width / 2, tex.Height / 2), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
