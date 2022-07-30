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
using ArchaeaMod.Projectiles;
namespace ArchaeaMod.Items
{
    [CloneByReference]
    public class c_Sword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cinnabar Sword");
            Tooltip.SetDefault("Sends ground tremors");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 48;
            Item.knockBack = 2f;
            Item.crit = 7;
            Item.value = 3500;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useTurn = false;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;
        }
        private int time;
        private int index;
        [CloneByReference]
        private Vector2[] ground;
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            index = 0;
            ground = GetGround(player, 10);
            return true;
        }
        public override void HoldItem(Player player)
        {
            if (ground == null)
                return;
            if (index < ground.Length)
            {
                if (ArchaeaItem.Elapsed(5))
                    Projectile.NewProjectileDirect(Projectile.GetSource_None(), ground[index++], Vector2.Zero, ModContent.ProjectileType<Mercury>(), Item.damage, Item.knockBack, player.whoAmI, Mercury.Ground);
            }
        }
        protected Vector2[] GetGround(Player start, int length)
        {
            List<Vector2> ground = new List<Vector2>();
            int x = (int)start.Center.X;
            int y = (int)start.position.Y + start.height;
            bool direction = start.direction == 1;
            for (int k = 0; direction ? k < length : k >= 0 - length; k -= direction ? -1 : 1)
            {
                bool add = true;
                int total = 0;
                int i = k + x / 16;
                int j = y / 16;
                while (!Main.tile[i, j].HasTile)
                {
                    j++;
                    if (total++ > 10)
                    {
                        add = false;
                        break;
                    }
                }
                if (add)
                    ground.Add(new Vector2(i * 16, j * 16));
            }
            return ground.ToArray();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ModContent.ItemType<Merged.Items.Materials.magno_bar>(), 8)
                .AddIngredient(ModContent.ItemType<Merged.Items.Materials.cinnabar_crystal>(), 6)
//            recipe.SetResult(this, 1);
                .Register();
        }
    }
}
