using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Merged.Items.Materials;
namespace ArchaeaMod.Merged.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class cinnabarmask : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cinnabar Mask");
            /* Tooltip.SetDefault("10% increased magic"
                +   "\ndamage"); */
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = 100;
            Item.rare = 3;
            Item.defense = 4;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<magno_bar>(), 10)
                .AddIngredient(ModContent.ItemType<cinnabar_crystal>(), 8)
                .AddTile(TileID.Anvils)
//            recipe.SetResult(this, 1);
                .Register();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<cinnabarplate>() && legs.type == ModContent.ItemType<cinnabargreaves>();
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) /= 0.9f;
        }
        bool spawnOK = false;
        int ticks = 0;
        int buffer = 256;
        int x, y;
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Generates lethal" 
                +   "\nspores";
            if (ticks++ % 60 == 0)
            {
                int newProj = Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.cinnabar_spore>(), 14, 0f, player.whoAmI, x, y);
            }
        }
        public bool TileCheck(int i, int j)
        {
        //  bool Dirt = Main.tile[i, j].type == TileID.Dirt;
            bool Active = Main.tile[i, j].HasTile == true;
            bool Solid = Main.tileSolid[Main.tile[i, j].TileType] == true;
            if (Solid && Active) return true;
            else return false;
        }
    }
}
