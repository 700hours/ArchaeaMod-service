using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace ArchaeaMod.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class RustbaneHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rustbane Visor");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false;
            // drawHair = false;
            // drawAltHair = false;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.rare = 3;
            Item.defense = 9;
            Item.value = 2500;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return
            head.type == Item.type &&
            body.type == ModContent.ItemType<RustbanePlate>() &&
            legs.type == ModContent.ItemType<RustbaneLegs>();
        }
        public override void UpdateArmorSet(Player player)
        {
            if (Main.dedServ) return;
            //  Rustbane armor set bonus
            player.setBonus = "Emanates rusty dust storm";
            if (Main.time > 0 && (int)Main.time % 10 == 0)
            {
                float radius = Main.rand.Next(100, 200);
                double angle = Math.PI * 2d * Main.rand.NextFloat();
                double cos = player.Center.X + radius * Math.Cos(angle);
                double sine = player.Center.Y + radius * Math.Sin(angle);
                ArchaeaPlayer.RadialDustDiffusion(player.Center, cos, sine, radius, 10, DustID.Pearlsand, 37, false, player.whoAmI);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.MythrilAnvil)
                .AddIngredient(ModContent.ItemType<Items.Materials.r_plate>(), 10)
                .Register();
        }
    }
}
