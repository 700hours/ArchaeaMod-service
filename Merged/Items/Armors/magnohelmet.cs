using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Merged.Items.Materials;
namespace ArchaeaMod.Merged.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class magnohelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Helmet");
            Tooltip.SetDefault("Increases maximum minion "
                +   "\ncount by 1");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = 100;
            Item.rare = 2;
            Item.defense = 3;
        }
        public override void AddRecipes()
        {
            // -- //
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<magno_bar>(), 10)
                .AddIngredient(ModContent.ItemType<magno_fragment>(), 8)
                .AddTile(TileID.Anvils)
//            recipe.SetResult(this, 1);
                .Register();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == Mod.Find<ModItem>("magnoplate").Type && legs.type == Mod.Find<ModItem>("magnogreaves").Type;
        }
        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
        }
        bool flag;
        int projMinion;
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Summons a Magno minion"
                +   "\nto your aid when"
                +   "\nin dire need";
            if (!flag && player.statLife <= player.statLifeMax / 2)
            {
                if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("magno_minion").Type] < player.maxMinions && player.numMinions < player.maxMinions)
                {
                    player.AddBuff(Mod.Find<ModBuff>("magno_summon").Type, 18000, false);
                    SoundEngine.PlaySound(SoundID.Item20, player.Center);
                    projMinion = Projectile.NewProjectile(Projectile.GetSource_None(), player.position, Vector2.Zero, Mod.Find<ModProjectile>("magno_minion").Type, 5, 3f, player.whoAmI, 0f, 0f);
                    player.maxMinions += 1;
                }
                flag = true;
            }
            if (flag && player.statLife > player.statLifeMax / 2)
            {
                if (Main.projectile[projMinion].type == Mod.Find<ModProjectile>("magno_minion").Type)
                    Main.projectile[projMinion].active = false;
                flag = false;
            }
        }
    }
}
