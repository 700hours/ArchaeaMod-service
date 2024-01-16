using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.TakerylProject.Items
{
	public class SpacialAnomaly : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Spacial Anomaly");
			//Tooltip.SetDefault("Bends special swords to your hand"
			//		+	"\nHold the 'Middle Mouse Button' to charge"
			//		+	"\nMust have a 'unique' sword selected on your hotbar");
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 1;
			Item.value = 50000;
			Item.rare = 5;
			Item.scale = 1f;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			Recipe.Create(this.Type)
			// recipe 1
				  .AddIngredient(46)	// recipe for night's bane
				  .AddIngredient(121)
				  .AddIngredient(155)
				  .AddIngredient(190)
				  .AddTile(26)
				  .Register();
            // recipe 2
            Recipe.Create(this.Type)
				  .AddIngredient(675)	// night's edge
				  .AddTile(TileID.CrystalBall)
				  .Register();
		}
		bool light = false;
		int lightDust, lightDust2;
		int coolDown = 0;
			
		int spinCharge = 0, spinDuration = 600;
		bool /*canSpin = false, */spinning = false, charging = false;
		int radius = 72, Radius; 
		float degrees = 0.017f, degrees2 = 3.077f, degrees3 = 0;
		int dmgTicks;
		int soundTicks = 0;
		int ticks = 0;
		
		int blasts;
		float charge = 450f;
		bool active = false;
		
		const float defaultCharge = 450f;
		
		Vector2 dustPos;
		Vector2 center;
		Texture2D swordTexture;
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			var modPlayer = player.GetModPlayer<ProjectPlayer>();
			if (!modPlayer.Spacial)
				return;

			ticks++;
			
			#region Missile Attack
			if(ticks >= 90 && Main.mouseRight && !Main.playerInventory)
			{
				ticks = 0;
				active = !active;
			}
			if(charge <= 0)
			{
				modPlayer.missileCoolDown = 900;
				charge = defaultCharge;
			}
			if(!active)
				charge += 0.5f;
			if(active && charge > 0f && modPlayer.missileCoolDown == 0)
			{
				charge--;
				
				Vector2 updateCenter = player.position + new Vector2(0, player.height/2);
				Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y );
				
				if(ticks%60 == 0)
				{
					// rocket ID: 134
					blasts = Projectile.NewProjectile(Item.GetSource_FromThis(), updateCenter, Vector2.Zero, 134, 32 + Main.rand.Next(-16, 8), 4f, player.whoAmI, 0f, 0f);
					SoundEngine.PlaySound(SoundID.Item14, player.position);
				}
				radius = 4;
				
				float Angle = (float)Math.Atan2(Main.screenPosition.Y + Main.mouseY - updateCenter.Y,
												Main.screenPosition.X + Main.mouseX - updateCenter.X);
				float MouseAngle = (float)Math.Atan2(mousev.Y - Main.projectile[blasts].position.Y, 
													mousev.X - Main.projectile[blasts].position.X);
			
				Main.projectile[blasts].velocity.X += Distance(Main.projectile[blasts], MouseAngle, radius).X;
				Main.projectile[blasts].velocity.Y += Distance(Main.projectile[blasts], MouseAngle, radius).Y;
			}
			#endregion
			
			#region Light
			int type = 0;
			if(coolDown == 0 && Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.Q) < 0)
			{
				modPlayer.lightState = !modPlayer.lightState;
				
				light = !light;
				coolDown = 60;
			}
			if(light)
			{
				degrees += 0.017f * 6; // 1 degree in radians multiplied by desired degree
				
				radius = 90;
				
				if(modPlayer.angel)
					type = 20;
				else if(modPlayer.demon)
					type = 35;
				else type = 6;
				
				center = player.position + new Vector2(player.width/2, player.height/2);
				// dust 1
				lightDust = Dust.NewDust(player.Center, 8, 8, type, 0f, 0f, 0, Color.White, 1f);
				Main.dust[lightDust].noGravity = true;
				Main.dust[lightDust].position.X = center.X + (float)(radius*Math.Cos(degrees));
				Main.dust[lightDust].position.Y = center.Y + (float)(radius*Math.Sin(degrees));
				// dust 2
				lightDust2 = Dust.NewDust(player.Center, 8, 8, type, 0f, 0f, 0, Color.White, 1f);
				Main.dust[lightDust2].noGravity = true;
				Main.dust[lightDust2].position.X = center.X + (float)(radius*Math.Cos(degrees + (0.017f*180)));
				Main.dust[lightDust2].position.Y = center.Y + (float)(radius*Math.Sin(degrees + (0.017f*180)));
			}
			if(coolDown > 0)
				coolDown--;
			#endregion
		}
		
		public Vector2 Distance(Projectile projectile, float Angle, float Radius)
		{
			float VelocityX = (float)(Radius*Math.Cos(Angle));
			float VelocityY = (float)(Radius*Math.Sin(Angle));
			
			if(projectile.velocity.X > 6f) 
				projectile.velocity.X -= 2f;
			if(projectile.velocity.Y > 6f) 
				projectile.velocity.Y -= 2f;
			
			return new Vector2(VelocityX, VelocityY);
		}
	}
}