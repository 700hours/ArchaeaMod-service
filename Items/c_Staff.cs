using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.NPCs;
using ArchaeaMod.Projectiles;
namespace ArchaeaMod.Items
{
    public class c_Staff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cinnabar Staff");
            Tooltip.SetDefault("Emits mercury dust");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 40;
            Item.knockBack = 2f;
            Item.crit = 7;
            Item.mana = 10;
            Item.value = 3500;
            Item.rare = 2;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Magic;
        }
        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (ArchaeaItem.Elapsed(10))
                Projectile.NewProjectileDirect(Projectile.GetSource_None(), hitbox.Center(), NPCs.ArchaeaNPC.AngleToSpeed(player.AngleTo(Main.MouseWorld), VelocityWeight(player, 4f)), ModContent.ProjectileType<Pixel>(), Item.damage, Item.knockBack, player.whoAmI, Pixel.Mercury, Pixel.Sword);
        }
        public static float VelocityWeight(Player player, float strength, float multiplier = 0.01f)
        {
            return player.Distance(Main.MouseWorld) * multiplier * strength;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ModContent.ItemType<Merged.Items.Materials.magno_bar>(), 8)
                .AddIngredient(ModContent.ItemType<Merged.Items.Materials.cinnabar_crystal>(), 8)
//            recipe.SetResult(this, 1);
                .Register();
        }
    }
}
