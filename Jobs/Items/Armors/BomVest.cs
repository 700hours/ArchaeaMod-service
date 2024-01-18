using ArchaeaMod.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Jobs.Items.Armors
{ 
    [AutoloadEquip(EquipType.Body)]
    public class BomVest : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
		    Item.height = 20;
		    Item.value = 800;
		    Item.noMelee = true;
		    Item.rare = 4;
            Item.defense = 9;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Type)
				.AddIngredient(ModContent.ItemType<ArchaeaMod.Items.Materials.r_plate>(), 12)
                .AddIngredient(ModContent.ItemType<ArchaeaMod.Merged.Items.Materials.magno_core>(), 5)
				.AddTile(TileID.Anvils)
				.Register();
            Recipe.Create(Type)
                .AddIngredient(ModContent.ItemType<ArchaeaMod.Items.Materials.r_plate>(), 12)
                .AddIngredient(ModContent.ItemType<ArchaeaMod.Merged.Items.Materials.magno_core>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public static void SetBonus(Player P)
	    {
		    if (ArchaeaItem.NotEquipped(P, ModContent.ItemType<BomVest>()))
			    return;
		    Rectangle pBox = new Rectangle((int)P.position.X-8, (int)P.position.Y-8, P.width+8, P.height+8);
		    foreach (NPC N in Main.npc)
		    {
			    if (!N.active) continue;
			    if (N.life <= 0) continue;
			    if (N.friendly) continue;
			    if (N.dontTakeDamage) continue;
			    if (N.boss) continue;
			    Rectangle nBox = new Rectangle((int)N.position.X, (int)N.position.Y, N.width, N.height);
			    if (pBox.Intersects(nBox))
			    {
				    N.StrikeNPC(N.CalculateHitInfo(10, P.direction, false, (float)Math.Round((double)P.velocity.X, 1), DamageClass.Melee));
			    }
		    }
	    }
    }
}
