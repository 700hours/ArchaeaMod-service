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
namespace ArchaeaMod.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class ShockPlate : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shock Plate");
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Ranged weapons create strong bolts of lightning";
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 18;
            Item.defense = 15;
            Item.value = 5000;
            Item.rare = ItemRarityID.Orange;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return
            head.type == ModContent.ItemType<ShockMask>() &&
            body.type == Item.type && 
            legs.type == ModContent.ItemType<ShockLegs>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.Autohammer)
                .AddIngredient(ModContent.ItemType<Items.Materials.r_plate>(), 15)
//            recipe.SetResult(this, 1);
                .Register();
        }
    }
    public class Bolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bolt");
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.damage = 10;
            Projectile.knockBack = 0f;
            Projectile.alpha = 240;
            Projectile.timeLeft = 40;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
        }
        private int ai = -1;
        private float spawnY = 800f;
        private float speedY
        {
            get { return spawnY / 10f; }
        }
        private NPC target
        {
            get { return Main.npc[(int)Projectile.ai[0]]; }
        }
        public override bool PreAI()
        {
            switch (ai)
            {
                case -1:
                    Projectile.Center = target.Center - new Vector2(0f, spawnY);
                    Projectile.netUpdate = true;
                    goto case 0;
                case 0:
                    ai = 0;
                    break;
            }
            return true;
        }
        public override void AI()
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 20;
            if (Projectile.position.Y < target.position.Y + target.height - Projectile.height)
                Projectile.position.Y += speedY;
        }
    }
}
