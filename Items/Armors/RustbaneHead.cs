using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchaeaMod.Merged.Projectiles;
using ArchaeaMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace ArchaeaMod.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class RustbaneHead : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ItemName", "Rustbane Visor"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip0", "ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip1", "// drawHair = false"));
            tooltips.Add(new TooltipLine(Mod, "Tooltip2", "// drawAltHair = false"));
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
            player.setBonus = "\"Bomb?!\"";
            if (Main.npc.Where(t => t.Center.Distance(player.Center) <= 300f && t.active && !t.friendly).Count() > 0)
            {
                if (Main.time > 0 && (int)Main.time % Main.rand.Next(5, 30) == 0)
                {
                    float radius = Main.rand.Next(player.height, 250);
                    double angle = Math.PI * 2d * Main.rand.NextFloat();
                    double cos = player.Center.X + radius * Math.Cos(angle);
                    double sine = player.Center.Y + radius * Math.Sin(angle);
                    var v2 = new Vector2((float)cos, (float)sine);
                                   //  TODO, find all values that are Flat and return zero
                    int damage = (int)(90f + player.GetDamage(DamageClass.Summon).Flat);    
                    int Proj2 = Projectile.NewProjectile(Projectile.GetSource_None(), v2, Vector2.Zero, ModContent.ProjectileType<magno_minionexplosion>(), 0, damage, player.whoAmI, 1f, 0f);
                    var t = Main.npc.Where(t => t.active && !t.friendly && t.Center.Distance(player.Center) <= radius);
                    if (Main.rand.NextFloat() < 0.2f && t.Count() > 0)
                    {
                        var _npc = t.ToArray()[Main.rand.Next(t.Count())];
                        v2 = _npc.Center - new Vector2(Main.projectile[Proj2].width / 2, Main.projectile[Proj2].height / 2);
                    }
                    else
                    {
                        v2 -= new Vector2(Main.projectile[Proj2].width / 2, Main.projectile[Proj2].height / 2);
                    }
                    Main.projectile[Proj2].position = v2;
                    SoundEngine.PlaySound(SoundID.Item14, Main.projectile[Proj2].position);
                    Main.projectile[Proj2].netUpdate = true;
                    int target = Main.projectile[Proj2].FindTargetWithLineOfSight();
                    if (target > -1)
                    {
                        NPC npc = Main.npc[target];
                        if (npc.Distance(Main.projectile[Proj2].Center) < Main.projectile[Proj2].width)
                        { 
                            ArchaeaNPC.StrikeNPC(npc, damage, 4f, Main.projectile[Proj2].Center.X < npc.Center.X ? 1 : -1, false);
                            int type = Projectile.NewProjectile(Projectile.GetSource_None(), npc.Center, Vector2.Zero, ModContent.ProjectileType<Merged.Projectiles.magno_minion>(), 5, 0f, player.whoAmI, 0f, npc.whoAmI);
                            Main.projectile[type].localAI[1] = -100f;
                        }
                    }
                }
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
