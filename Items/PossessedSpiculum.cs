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

namespace ArchaeaMod.Items
{
    public class PossessedSpiculum : ModItem
    {
        public override string Texture => "ArchaeaMod/Gores/MagnoSpear_2";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possessed Spiculum");
            Tooltip.SetDefault("Embodied throwing arm");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 15000;
            Item.damage = 50;
            Item.shoot = ModContent.ProjectileType<Projectiles.Spiculum>();
            Item.shootSpeed = 5f;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.reuseDelay = 30;
            Item.autoReuse = false;
            Item.useTime = 90;
            Item.useAnimation = 35;
            Item.channel = false;
            Item.rare = ItemRarityID.LightPurple;
            Item.DamageType = DamageClass.Melee;
            //Item.UseSound = SoundID.?  Throw sound
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Spiculum>()] == 0;
        }
        public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Gores/MagnoSpear_2").Value;
            sb.Draw(tex, position, frame, Color.SkyBlue * 0.67f, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Gores/MagnoSpear_2").Value;
            sb.Draw(tex, Item.position, null, Color.SkyBlue * 0.67f, 0f, new Vector2(tex.Width / 2, tex.Height / 2), scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
