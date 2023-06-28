using ArchaeaMod.Items;
using ArchaeaMod.Jobs.Global;
using ArchaeaMod.Jobs.Projectiles;
using ArchaeaMod.NPCs;
using IL.Terraria.GameContent.NetModules;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using rail;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static IL.Terraria.WorldBuilding.Searches;
using static System.Formats.Asn1.AsnWriter;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ArchaeaMod.Jobs.Items
{
    internal class Manipulate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Manipulate");
            Tooltip.SetDefault("Make enemies do figure 8's, or something.\n" +
                "Drains mana quickly.\n" +
                "Requires line of sight.");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.useStyle = 1;
            Item.useAnimation = 20;
            Item.useTime = 0;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.scale = 1;
            Item.value = 0;
            Item.rare = 2;
        }
        NPC target = default(NPC);
        Projectile effect = default(Projectile);
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                Vector2 mousev = Main.MouseWorld;
                Rectangle mouse = new Rectangle((int)(mousev.X - 16f), (int)(mousev.Y - 16f), 32, 32);

                if (player.statMana <= 0)
                { 
                    target = default(NPC);
                    return false;
                }
                if (Main.rand.NextBool(60))
                {
                    int index = Dust.NewDust(player.Center, 1, 1, DustID.AncientLight, ArchaeaNPC.RandAngle() * 4f, ArchaeaNPC.RandAngle() * 4f, 0, default, 2f);
                    Main.dust[index].noGravity = true;
                }
                if (ArchaeaItem.Elapsed(10))
                {
                    int index = Dust.NewDust(player.position + new Vector2(player.width / 2, player.height - 1), 1, 1, DustID.AncientLight, ArchaeaNPC.RandAngle() * 3f, 0f, 0, default, 1f);
                    Main.dust[index].noGravity = true;
                }
                if (target == default(NPC))
                { 
			        NPC[] npc = Main.npc;
			        for(int m = 0; m < npc.Length; m++)
			        {
				        NPC nPC = npc[m];
				        Rectangle npcBox = nPC.Hitbox;
				        if (mouse.Intersects(npcBox) && !nPC.boss && player.statMana > 0 && Main.mouseLeft && Collision.CanHitLine(nPC.Center, nPC.width, nPC.height, player.Center, player.width, player.height))
				        {
                            target = nPC;
                            effect = Projectile.NewProjectileDirect(Projectile.GetSource_None(), target.Center, Vector2.Zero, ModContent.ProjectileType<j_effect>(), 0, 0f, Main.myPlayer, EffectID.Polygon, target.whoAmI);
                            break;
				        }
			        }
                }
                else
                {
                    if (Main.mouseLeft && Collision.CanHitLine(target.Center, target.width, target.height, player.Center, player.width, player.height))
                    {
                        target.position = new Vector2(mousev.X - (float)target.width / 2, mousev.Y - (float)target.height / 2);
                        player.statMana--;
                        player.manaRegenDelay = (int)player.maxRegenDelay;
                        if (ArchaeaNPC.IsNotOldPosition(target))
                        {
                            float angle = target.oldPos[2].AngleTo(mousev);
                            float distance = target.oldPos[2].Distance(target.position);
                            target.rotation = angle;
                            if (distance > 20f)
                            {
                                if (Collision.SolidCollision(target.position, target.width, target.height))
                                {
                                    target.StrikeNPC((int)distance / 2, 0f, 0, fromNet: Main.netMode == 1);
                                }
                            }
                            target.netUpdate = true;
                        }
                    }
                    else
                    {
                        effect.active = false;
                        target = default(NPC);
                    }
                }
            }
            return null;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Book)
                .AddIngredient(ItemID.Sunflower, 5)
                .AddIngredient(ItemID.Deathweed, 5)
                .AddTile(ItemID.Bookcase)
                .Register();
            
        }
    }
}