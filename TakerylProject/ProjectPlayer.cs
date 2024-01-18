using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using ReLogic.Graphics;

using ArchaeaMod.TakerylProject.Projectiles;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Audio;

namespace ArchaeaMod.TakerylProject
{
	public class ProjectPlayer : ModPlayer
	{
        #region sword spin
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
        #endregion
        public bool canSpin = false;
		public bool drawBlink = false;
		public bool drawLeft, drawRight;
		public bool angel, demon, spacePirate;
		public bool 
			SK_Ranged,
			SK_Melee,
			SK_Phase;
		private bool leapt;
		public bool lightState = false;
		public bool disabled = true, debug = true;
		public int 
			blinkCoolDown = 0, spinCoolDown = 0, 
			strikeCoolDown = 0, missileCoolDown = 0,
			SK_coolDown, SK_maxCoolDown;
		private int oldHP, direction, num;
		public int Active_Skill;
		public const int
			AS_None = -1,
            AS_Spin = 0,
            AS_Leap = 1,
			AS_Throw = 2,
			AS_Convert = 10,
			AS_PhaseShift = 11,
			AS_RefreshAura = 12,
			AS_ShockNova = 13,
			AS_WindBuff = 14,
			AS_LightningBolt = 15,
			AS_Poison = 20,
			AS_ToxicNova = 21, 
			AS_Berzerk = 22,
			AS_IceStorm = 23,
			AS_MeteorRain = 24,
			AS_Swamp = 25;
		public readonly string[] Name = new string[]
		{
			"Spin",
			"Leap",
            "Throw",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "Convert",
			"Phase Shift",
			"Refresh Aura",
			"Shock Nova",
			"Wind Buff",
			"Lightning Bolt",
            "",
            "",
            "",
            "",
            "Poison",
			"Toxic Nova",
			"Berserk",
			"Ice Storm",
			"Meteor Rain",
			"Swamp"
        };
		private float weight, distance;
		private Vector2 start, target;
		private Item swordSpin;
		
	/*	public override TagCompound Save()
		{
			return new TagCompound {
				{"allegiance", angel} ,
				{"_allegiance", demon}
			};
		}

		public override void Load(TagCompound tag)
		{
			angel = tag.GetBool("allegiance");
			demon = tag.GetBool("_allegiance");
		}	*/
		private bool init = true;
		public override void PreUpdate()
		{
			switch (Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().jobChoice)
			{
				case JobID.ALL_BusinessMan:
					Active_Skill = AS_LightningBolt;
					break;
				case JobID.ALL_Entrepreneur:
					Active_Skill = AS_RefreshAura;
					break;
				case JobID.ALL_Merchant:
					Active_Skill = AS_Convert;
					break;
				case JobID.MAGE_Botanist:
					Active_Skill = AS_Swamp;
					break;
				case JobID.MAGE_Witch:
					Active_Skill = AS_ToxicNova;
					break;
				case JobID.MAGE_Wizard:
					Active_Skill = AS_MeteorRain;
					break;
				case JobID.MELEE_Smith:
					Active_Skill = AS_Spin;
					break;
				case JobID.MELEE_Warrior:
					Active_Skill = AS_Leap;
					break;
				case JobID.MELEE_WhiteKnight:
					Active_Skill = AS_Throw;
					break;
				case JobID.RANGED_Bowsman:
					Active_Skill = AS_WindBuff;
					break;
				case JobID.RANGED_CorperateUsurper:
					Active_Skill = AS_ShockNova;
					break;
				case JobID.RANGED_Outlaw:
					Active_Skill = AS_Poison;
					break;
				case JobID.SUMMONER_Alchemist:
					Active_Skill = AS_IceStorm;
					break;
				case JobID.SUMMONER_Scientist:
					Active_Skill = AS_Berzerk;
					break;
				case JobID.SUMMONER_Surveyor:
					Active_Skill = AS_PhaseShift;
					break;
			}
            #region sword spin skill
            if (canSpin)
            {
				//	blinkCharge = 0;
                if (swordSpin.type == 46 || swordSpin.type == 121 || swordSpin.type == 155 || swordSpin.type == 190 || swordSpin.type == 273 || swordSpin.type == 368 || swordSpin.type == 484 || swordSpin.type == 485 || swordSpin.type == 486 || swordSpin.type == 675 || swordSpin.type == 723 || swordSpin.type == 989 || swordSpin.type == 1166 || swordSpin.type == 1185 || swordSpin.type == 1192 || swordSpin.type == 1199 || swordSpin.type == ItemID.CopperShortsword)
                {
                    spinning = true;

                    radius = 72;
                    degrees3 += 0.06f / 2f;

                    center = Player.position + new Vector2(Player.width / 2, Player.height / 2);

                    float PosX = center.X + (float)(radius * Math.Cos(degrees3));
                    float PosY = center.Y + (float)(radius * Math.Sin(degrees3));

                    Vector2 swordPos = Player.bodyPosition + new Vector2(PosX, PosY);

                    Rectangle swordHitBox = new Rectangle((int)swordPos.X, (int)swordPos.Y, swordSpin.width, swordSpin.height);
                    NPC[] npc = Main.npc;
                    for (int k = 0; k < npc.Length - 1; k++)
                    {
                        NPC n = npc[k];
                        Vector2 npcv = new Vector2(n.position.X, n.position.Y);
                        Rectangle npcBox = new Rectangle((int)npcv.X, (int)npcv.Y, n.width, n.height);
                        if (n.active && !n.friendly && !n.dontTakeDamage && dmgTicks == 0 && swordPos.Distance(n.Center) <= swordHitBox.Width)
                        {
                            n.StrikeNPC(n.CalculateHitInfo((int)(swordSpin.damage * 1.1f), Player.Center.X < n.Center.X ? 1 : -1, false, swordSpin.knockBack * 3, DamageClass.Melee, true));
                            dmgTicks = 10;
                        }
                    }
                    Projectile[] projectile = Main.projectile;
                    for (int l = 0; l < projectile.Length - 1; l++)
                    {
                        Projectile n = projectile[l];
                        Vector2 projv = new Vector2(n.position.X, n.position.Y);
                        Rectangle projBox = new Rectangle((int)projv.X, (int)projv.Y, n.width, n.height);
                        if (n.active && swordPos.Distance(n.Center) <= swordHitBox.Width)
                        {
                            n.timeLeft = 0;
                        }
                    }
                    for (int l = 0; l < Main.player.Length - 1; l++)
                    {
                        Player n = Main.player[l];
                        Vector2 projv = new Vector2(n.position.X, n.position.Y);
                        Rectangle projBox = new Rectangle((int)projv.X, (int)projv.Y, n.width, n.height);
                        if (n.active && !n.dead && n.hostile && n.InOpposingTeam(Player) && swordPos.Distance(n.Center) <= swordHitBox.Width)
                        {
                            n.DropSelectedItem();
                        }
                    }

                    //	TODO: spin sound effect
                    //soundTicks++;
                    //if(soundTicks%16 == 0)
                    //	SoundEngine.PlaySound(SoundID., Player.bodyPosition + new Vector2(PosX, PosY));
                }
                else
                {
                    spinning = false;
                }
                if (spinDuration > 0)
                    spinDuration--;
                if (spinDuration == 0)
                {
                    degrees3 = 0f;
                    canSpin = false;
                    spinning = false;
                }
            }
            if (dmgTicks > 0)
                dmgTicks--;
            #endregion
            if (ArchaeaMain.jobSkill.JustPressed)
			{
				if (Active_Skill == AS_Throw)
				{
					for (int i = 0; i < SwordID.swordTypes.Length; i++)
					{	
						if (Player.HeldItem.type == SwordID.swordTypes[i])
						{	
							float speed = 8f;
							Projectile.NewProjectile(Projectile.GetSource_None(), Player.position + new Vector2(16, 0), new Vector2(speed * (float)Math.Cos(Player.AngleTo(Main.MouseWorld)), speed * (float)Math.Sin(Player.AngleTo(Main.MouseWorld))), ModContent.ProjectileType<Projectiles.ThrownSword>(), 30, 5f, Player.whoAmI, ThrownSword.AI_Embed4All, Player.inventory[Player.selectedItem].type);
							Player.inventory[Player.selectedItem].type = ItemID.None;
							break;
						}
					}
				}
				else if (Active_Skill == AS_Spin && SK_coolDown <= 0)
				{
                    #region Sword Spin
                    swordSpin = Player.inventory[Player.selectedItem];
                    if (!canSpin && !spinning)
                    {
                        foreach (int i in SwordID.swordTypes)
                        {
                            if (swordSpin.type == i)
                            {
                                spinCharge = 60;
                                if (spinCharge >= 60)
                                {
                                    canSpin = true;
                                    spinCharge = 0;
                                    spinDuration = 600;
                                    spinCoolDown = 1200;  
									SetCoolDown(900);
                                }
                                if (spinCharge == 30)
                                    SoundEngine.PlaySound(SoundID.Item2, Player.position);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (spinCharge > 0)
                            spinCharge--;
                    }
                    #endregion
                }
                else if (Active_Skill == AS_Leap && SK_coolDown <= 0)
				{
					if (!leapt)
					{
                        if (Player.direction == 1)
                            target = new Vector2(Player.position.X + distance, Player.position.Y);
                        else
                            target = new Vector2(Player.position.X - distance, Player.position.Y);
                        leapt = true;

                        int k = (int)Player.position.X / 16;
                        int l = (int)Player.position.Y / 16;
                        if ((Main.tile[k - 1, l + 1].HasTile && Main.tileSolid[Main.tile[k - 1, l + 1].TileType] && Player.direction == -1) || (Main.tile[k + 2, l + 1].HasTile && Main.tileSolid[Main.tile[k + 2, l + 1].TileType] && Player.direction == 1))
                        {
                            weight = 0f;
                            leapt = false;
                            return;
                        }

                        SK_coolDown = 180;
                        SK_maxCoolDown = 180;
						Player.lifeRegenTime = 150;
						oldHP = Player.statLife;
						direction = Player.direction;
						weight = 0f;
						distance = 16f * 10f + 16f;
						start = Player.position - new Vector2((distance - 16f) / 16f * 8f * (direction * -1), 0f);
						start.Y += 8f;
						if (direction > 0)
						{
							start.X += 16f;
							start.Y += 8f;
						}
					}
				}
				if (Active_Skill > AS_Throw)
				{
					//if (angel)
					AlphaSkillSet();
					//else if (demon)
					DireSkillSet();
				}
			}
			if (leapt)
			{
                float cos = (float)(distance / 2 * Math.Cos(weight * Math.PI));
				float sine = (float)(distance * 1.25f * Math.Sin(weight * Math.PI));
				if (weight < 1f)
				{ 
					weight += 0.02f;
					//	Make it progression based so longer leaps
					Player.position += new Vector2(cos, sine * direction) * (direction * -1) + Vector2.Lerp(start, target, weight) - new Vector2(Main.screenPosition.X + Main.screenWidth / 2, Main.screenPosition.Y + Main.screenHeight / 2);
				}
				else leapt = false;
			}
			if (SK_Phase)
			{
				Player.position = Vector2.Lerp(start, phaseTo, weight);
				if (weight < 1f)
					weight += 0.1f;
				else 
				{
					phaseTo = Vector2.Zero;
					SK_Phase = false;
				}
			}

			if (angel || demon)
				Player.noFallDmg = true;
			#region actions
				if (missileCoolDown > 0)
					missileCoolDown--;
				if (spinCoolDown > 0)
					spinCoolDown--;
				if (blinkCoolDown > 0)
					blinkCoolDown--;
				if (strikeCoolDown > 0)
					strikeCoolDown--;
				if (SK_coolDown > 0)
					SK_coolDown--;
			#endregion
		}
		public override void PostUpdate()
		{
			int sx = (int)Player.position.X, sy = (int)Player.position.Y;
			int	ex = sx + Player.width - 8, ey = sy + Player.height - 8;
			if (weight >= 1f || oldHP != Player.statLife)
				leapt = false;
            if (leapt)
			{
				if (weight >= 0.08f)
				{ 
					for (int i = 0; i < 4; i++)
					{
						for (int j = 0; j < 5; j++)
						{
							int x = (int)(Player.position.X - 16 + i * 16) / 16;
							int y = (int)(Player.position.Y - 16 + j * 16) / 16;
							if (Main.tile[x, y].HasTile && Main.tileSolid[Main.tile[x, y].TileType])
							{
								leapt = false;
							}
						}
					}
				}
			}

			return;
			#region debug
				if (KeyPress(Keys.Right))
				{
					//Player.inventory[6].SetDefaults(mod.ItemType<Items.Alpha>());
					//Player.inventory[7].SetDefaults(mod.ItemType<Items.Dire>());
					//Player.inventory[8].SetDefaults(mod.ItemType<Items.BlinkDagger>());
					//Player.inventory[9].SetDefaults(mod.ItemType<Items.SpacialAnomaly>());
				}
			#endregion
		}
		
		int animate = 0, frameHeight = 52;
		Texture2D blinkTexture;
		Texture2D GemTexture;
		Texture2D WingsTex;
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            int radius = 72;
			if (/*thrown || */canSpin)
				degrees3 += 0.06f; // 0.017f = 1 degree displayed in radians
			else degrees3 = 0;
			
		//	UI properties
			boxOrigin = new Vector2(32, 160);
			boxSize = new Vector2(40, 40);
			boxCenter = new Vector2(96, 224);
			colorSpecial = default(Color);
			newColor = default(Color);
			newColor2 = default(Color);
			newColor3 = default(Color);
			newColor4 = default(Color);
			textSize = "Right Mouse";
			
		// Light
			string textSpecial = "Light (Q)";
			if (lightState)
				colorSpecial = Color.LightBlue;
			else if (!lightState)
				colorSpecial = Color.Gray;

			//Main.spriteBatch.DrawString(FontAssets.MouseText.Value, textSpecial, boxCenter + new Vector2(32, 64), colorSpecial, 0f, new Vector2(8,8) + FontAssets.MouseText.Value.MeasureString(textSize), 1f, SpriteEffects.None, 0f);
		
		//	CurrentPoint = (Time - Depreciating) / Time;
			
			SpriteEffects effects = SpriteEffects.None;
			Color color = Player.GetImmuneAlpha(Lighting.GetColor((int)((double)Player.position.X + (double)Player.width * 0.5) / 16, (int)(((double)Player.position.Y + (double)Player.height * 0.25) / 16.0), Color.White), 0f);
			Vector2 Position = Player.position;
			Vector2 origin = new Vector2((float)Player.legFrame.Width * 0.5f, (float)Player.legFrame.Height * 0.5f);
			Vector2 bodyPosition = new Vector2((float)((int)(Player.position.X - Main.screenPosition.X - (float)(Player.bodyFrame.Width / 2) + (float)(Player.width / 2))), (float)((int)(Player.position.Y - Main.screenPosition.Y + (float)Player.height - (float)Player.bodyFrame.Height + 4f)));
			Vector2 wingsPosition = new Vector2((float)((int)(Position.X - Main.screenPosition.X + (float)(Player.width / 2) - (float)(9 * Player.direction)) + 0 * Player.direction), (float)((int)(Position.Y - Main.screenPosition.Y + (float)(Player.height / 2) + 2f * Player.gravDir + (float)24 * Player.gravDir)));

      		float MoveX = origin.X + (float)(radius*Math.Cos(degrees3));
			float MoveY = origin.Y + (float)(radius*Math.Sin(degrees3));
			
			Item item = Player.inventory[Player.selectedItem];
			for (int i = 0; i < SwordID.swordTypes.Length; i++) 
			{
				if (item.type == SwordID.swordTypes[i])
				{				// TODO: check if this works
					swordTexture = TextureAssets.Item[item.type].Value;
					break;
				}
			}
			if (canSpin)
			{
				Main.spriteBatch.Draw(swordTexture, 
					bodyPosition + Player.bodyPosition + new Vector2(MoveX, MoveY), 
					null, Color.White, (degrees3*3)*(-1) + 0.48f, origin, 1f, effects, 0f);
			}
			if (!Main.playerInventory)
				DrawSkillUI(SK_coolDown, newColor, 90, "Active skill: " + Name[Active_Skill]);
			if (Active_Skill == AS_Berzerk)
			{
				if (ArchaeaMain.jobSkill.JustPressed)
					num++;
				if (num > 2)
					num = 0;
				if (num == 1)
				{
					float dist = Player.Distance(Main.MouseWorld);
					for (float rad = 0f; rad < Math.PI * 2f; rad += radians(dist))
					{
						float cos = (float)(Player.Center.X + dist * Math.Cos(rad)) - Main.screenPosition.X;
						float sine = (float)(Player.Center.Y + dist * Math.Sin(rad)) - Main.screenPosition.Y;
						Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)cos, (int)sine, 1, 1), Color.Red);
					}
					for (int i = 0; i < Main.player.Length; i++)
					{
						if (Main.player[i].active && !Main.player[i].dead && !Main.player[i].hostile && !Main.player[i].InOpposingTeam(Player))
						{
							if (Main.player[i].Distance(Player.Center) <= dist)
							{ 
								gainBuff[i] = true;
							}
							else gainBuff[i] = false;
                        }
					}
					for (int i = 0; i < Main.npc.Length; i++)
					{
						if (Main.npc[i].friendly)
						{
							if (Main.npc[i].Distance(Player.Center) <= dist)
							{ 
								npcGainBuff[i] = true;
							}
							else npcGainBuff[i] = false;
                        }
                    }
				}
				if (num == 2 && CanCastSkill(20, 600))
				{
					for (int j = 0; j < gainBuff.Length; j++)
					{
						if (gainBuff[j])
						{
							gainBuff[j] = false;
							Main.player[j].AddBuff(BuffID.Wrath, 1800);
							Main.player[j].AddBuff(BuffID.Archery, 1800);
							Main.player[j].AddBuff(BuffID.Ironskin, 1800);
						}
					}
                    for (int j = 0; j < npcGainBuff.Length; j++)
                    {
                        if (npcGainBuff[j])
                        {
							npcGainBuff[j] = false;
                            Main.npc[j].AddBuff(BuffID.Wrath, 1800);
                            Main.npc[j].AddBuff(BuffID.Archery, 1800);
                            Main.npc[j].AddBuff(BuffID.Ironskin, 1800);
                        }
                    }
					num = 0;
                }
			}
			#region debug
			return;
			if (!disabled)
			{
				try
				{
					if (KeyPress(Keys.Up) && WingID.Selected > 0)
						WingID.Selected--;
					if (KeyPress(Keys.Down))
						WingID.Selected++;
					if (KeyPress(Keys.F1) && !disabled)
					{
						int[] wings = new int[] { ItemID.AngelWings, ItemID.DemonWings };
						for (int i = 0; i < 2; i++)
							Item.NewItem(Item.GetSource_None(), Player.Center, Vector2.Zero, wings[i]);
					}
					if (KeyPress(Keys.F2))
						Main.NewText(WingID.Selected);
					Player.wings = WingID.Selected;
				}
				catch 
				{
					return;
				}
			}
			#endregion
		}
		public static bool KeyPress(Keys key)
		{
			return Main.oldKeyState.IsKeyUp(key) && Main.keyState.IsKeyDown(key);
		}
		public static bool KeyHold(Keys key)
		{
			return Main.keyState.IsKeyDown(key);
		}
		private bool skillActive;
		private Vector2 phaseTo = Vector2.Zero;
		internal void AlphaSkillSet()
		{
			if (KeyPress(Keys.E))
				skillActive = !skillActive;
			switch (Active_Skill)
			{
				case AS_Convert:
					if (CanCastSkill(10, 300))
					{
						Projectile.NewProjectile(Projectile.GetSource_None(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<a_Convert>(), 0, 0f, Player.whoAmI);
					}
					break;
				case AS_PhaseShift:
					if (CanCastSkill(20, 450))
					{
						while (phaseTo == Vector2.Zero)
						{
							Vector2 position = Player.position;
							int x = (int)Main.rand.NextFloat(position.X - 400f, position.X + 400f) / 16;
							int y = (int)Main.rand.NextFloat(position.Y - 300f, position.Y + 300f) / 16;
							int tile = 1;
							if (Collision.SolidTiles(x - tile, x + tile * 2, y, y + tile * 2))
								continue;
							weight = 0f;
							start = Player.position;
							phaseTo = new Vector2(x * 16, y * 16);
							SK_Phase = true;
						}
					}
					break;
				case AS_RefreshAura:
					if (CanCastSkill(20, 600))
					{
						Projectile.NewProjectile(Projectile.GetSource_None(), Player.Center, Vector2.Zero, ModContent.ProjectileType<a_Aura>(), 0, 0f, Player.whoAmI);
					}
					break;
				case AS_WindBuff:
					if (CanCastSkill(15, 180))
					{
						float cos = 4f * (float)Math.Cos(Player.AngleTo(Main.MouseWorld));
						float sine = 4f * (float)Math.Sin(Player.AngleTo(Main.MouseWorld));
						Projectile.NewProjectile(Projectile.GetSource_None(), Player.Center, new Vector2(cos, sine), ModContent.ProjectileType<a_Wind>(), 0, 0f, Player.whoAmI);
					}
					break;
				case AS_ShockNova:
					if (CanCastSkill(20, 300))
					{
						Projectile.NewProjectile(Projectile.GetSource_None(), Player.Center, Vector2.Zero, ModContent.ProjectileType<a_Shock>(), 0, 0f, Player.whoAmI);
					}
					break;
				case AS_LightningBolt:
					if (CanCastSkill(20, 360))
					{
						for (int i = 0; i < 3; i++)
							Projectile.NewProjectile(Projectile.GetSource_None(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<a_Lightning>(), 33, 0f, Player.whoAmI);
					}
					break;
				default:
					break;
			}
		}
		internal void DireSkillSet()
		{
			if (KeyPress(Keys.E))
				skillActive = !skillActive;
			switch (Active_Skill)
			{
				case AS_Poison:
					if (CanCastSkill(10, 180))
					{
						Projectile.NewProjectile(Projectile.GetSource_None(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<d_Poison>(), 0, 0f, Player.whoAmI);
					}
					break;
				case AS_ToxicNova:
					if (CanCastSkill(15, 300))
					{
						Projectile.NewProjectile(Projectile.GetSource_None(), Player.Center, Vector2.Zero, ModContent.ProjectileType<d_Toxic>(), 0, 0f, Player.whoAmI);
					}
					break;
				case AS_IceStorm:
					if (CanCastSkill(20, 1200)) // 35?
					{
						Projectile.NewProjectile(Projectile.GetSource_None(), Player.Center, Vector2.Zero, ModContent.ProjectileType<d_IceStorm>(), 0, 0f, Player.whoAmI);
					}
					break;
				case AS_MeteorRain:
					if (CanCastSkill(20, 1200)) // 40
					{
						Projectile.NewProjectile(Projectile.GetSource_None(), Player.Center, Vector2.Zero, ModContent.ProjectileType<d_Meteor>(), 0, 0f, Player.whoAmI);
					}
					break;
				case AS_Swamp:
					if (CanCastSkill(20, 1200)) //30
					{
						Projectile.NewProjectile(Projectile.GetSource_None(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<d_Bog>(), 0, 0f, Player.whoAmI);
					}
					break;
				default:
					break;
			}
		}
		internal void SetCoolDown(int duration, int buff = 10)
		{
			SK_coolDown = duration;
            SK_maxCoolDown = duration;
		}
		internal bool CanCastSkill(int manaCost, int coolDown)
		{
			bool success = SK_coolDown == 0 && Player.statMana >= manaCost;
			if (success)
			{
				Player.manaRegenDelay = 150;
				Player.CheckMana(manaCost, true);
				SetCoolDown(coolDown);
			}
			return success;
		}

		private bool[] gainBuff = new bool[256];
        private bool[] npcGainBuff = new bool[Main.npc.Length];
        private Vector2 boxOrigin;
		private Vector2 boxSize;
		private Vector2 boxCenter;
		private Color colorSpecial, newColor, newColor2, newColor3, newColor4;
		private string textSize;

		internal void DrawSkillUI(int coolDown, Color newColor, int offset = 76, string text = "")
		{
			SpriteBatch spriteBatch = Main.spriteBatch;
			
			SpriteEffects effects = SpriteEffects.None;
			
			int leftOrient = (int)boxCenter.X - 76;
			int verticalOrient = (int)boxCenter.Y;
			int max = SK_maxCoolDown;

			if (coolDown > 0)
				newColor = Color.Red;
			else if (coolDown == 0)
				newColor = Color.LightGreen;

            Utils.DrawBorderString(spriteBatch, text, boxCenter + new Vector2(-76, (offset - 16) * -1), newColor);
            	
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(leftOrient, verticalOrient - offset, 100, 4), Color.Black);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(leftOrient, verticalOrient - offset, (int)(Math.Abs((float)coolDown / Math.Max(max, 1) - 1f) * 100f), 4), Color.Lerp(Color.Green, Color.Red, (float)coolDown/max));
		}

		public const float radian = 0.017f;
        public float radians(float distance)
        {
            return radian * (45f / distance);
        }
	}
}