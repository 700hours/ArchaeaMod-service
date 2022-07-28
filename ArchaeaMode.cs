using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using ArchaeaMod.ModUI;

namespace ArchaeaMod.Mode
{
    public sealed class ArchaeaMode
    {
        public enum Stat
        {
            Life,
            Damage
        }
        private static float LifeRegen(int statMax)
        {
            return statMax / 9999f + 1f;
        }
        private static float ManaRegen(int statMax)
        {
            return statMax / 999f + 1f;
        }
        public static int HealPotion(int amount)
        {
            float quotient = 400f / 9999f;
            int result = (int)(amount / quotient);
            return result;
        }
        public static int ManaPotion(int amount)
        {
            float quotient = 200f / 999f;
            int result = (int)(amount / quotient);
            return result;
        }
        public static int LifeCrystal(int add = 20)
        {
            float quotient = 400f / 9999f;
            //int vanilla = 20;
            int result = (int)(add / quotient);
            result += result % 2;
            return result;
        }
        public static int ManaCrystal(int add = 20)
        {
            float quotient = 200f / 999f;
            //int vanilla = 20;
            int result = (int)(add / quotient);
            result += result % 2;
            return result;
        }
        public static int ModeScaling(Stat stat, int value, float scale)
        {
            /* 
             * Ratio
            float ratio = h / w;
            float result = h / ratio;
            */
            float result = value / scale;
            switch (stat)
            {
                case Stat.Life:
                    result *= 1.2f;
                    break;
                case Stat.Damage:
                    result *= 1.1f;
                    break;
            }
            return (int)result;
        }
    }
    public class ModeToggle : ModSystem
    {
        public bool archaeaMode;
        public bool progress;
        public float healthScale;
        public float damageScale;
        public override void SaveWorldData(TagCompound tag)/* Edit tag parameter rather than returning new TagCompound */
        {
            tag.Add("ArchaeaMode", archaeaMode);
            tag.Add("HealthScale", healthScale);
            tag.Add("DamageScale", damageScale);
            tag.Add("DayCount", dayCount);
            tag.Add("TotalTime", totalTime);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            archaeaMode = tag.GetBool("ArchaeaMode");
            healthScale = tag.GetFloat("HealthScale");
            damageScale = tag.GetFloat("DamageScale");
            dayCount = tag.GetFloat("DayCount");
            totalTime = tag.GetFloat("TotalTime");
        }
        public override void NetSend(BinaryWriter writer)
        {
            //writer.Write(archaeaMode);
            //writer.Write(totalTime);
            //writer.Write(dayCount);
        }
        public override void NetReceive(BinaryReader reader)
        {
            //archaeaMode = reader.ReadBoolean();
            //totalTime = reader.ReadSingle();
            //dayCount = reader.ReadSingle();
        }
        private bool init;
        private Button objectiveButton;
        public override void PostUpdateEverything()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            { 
                totalTime += (float)Main.frameRate / 60f;
                dayCount = totalTime / (float)Main.dayLength;
            }
            if (Main.dedServ)
                return;
            if (!init)
            {
                objectiveButton = new Button("Mode Status", new Rectangle(20, 284, 10 * 11, 24));
                init = true;
            }
            if (objectiveButton.LeftClick() && Main.playerInventory)
                progress = !progress;
        }
        public float dayCount;
        public float totalTime;
        public override void PostDrawTiles()
        {
            SpriteBatch sb = Main.spriteBatch;
            if (Main.playerInventory)
            {
                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                if (progress)
                {
                    Rectangle panel = new Rectangle(306 - 160, 255, 180, 100);
                    sb.Draw(TextureAssets.MagicPixel.Value, panel, Color.DodgerBlue * 0.33f);
                    sb.DrawString(FontAssets.MouseText.Value, "Life scale: " + Math.Abs(ModeNPC.ModeChecksLifeScale() - 2f), new Vector2(panel.Left + 4, panel.Top + 4), Color.White);
                    sb.DrawString(FontAssets.MouseText.Value, "Damage scale: " + Math.Abs(ModeNPC.ModeChecksDamageScale() - 2f), new Vector2(panel.Left + 4, panel.Top + 24), Color.White);
                    sb.DrawString(FontAssets.MouseText.Value, "Day: " + Math.Round(dayCount + 1, 0), new Vector2(panel.Left + 4, panel.Top + 44), Color.White);
                    sb.DrawString(FontAssets.MouseText.Value, "World time: " + Math.Round(totalTime / 60d / 60d, 1), new Vector2(panel.Left + 4, panel.Top + 64), Color.White);
                }
                if (archaeaMode)
                    objectiveButton.Draw();
                sb.End();
            }
        }
    }
    public class ModeNPC : GlobalNPC
    {
        public override bool InstancePerEntity 
        {
            get { return true; }
        }
        private bool init;
        
        private static readonly int 
            start = 0, health = 1, mana = 2, bosses = 3, bottom = 4, npcs = 5, week= 6, crafting = 7, downedMagno = 8;
        private static readonly float[] scaling = new float[] { 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f };
        public override void AI(NPC n)
        {
            if (!init)
            {
                if (ModContent.GetInstance<ModeToggle>().archaeaMode)
                {
                    n.lifeMax = ArchaeaMode.ModeScaling(ArchaeaMode.Stat.Life, n.lifeMax, ModeChecksLifeScale());
                    n.damage = ArchaeaMode.ModeScaling(ArchaeaMode.Stat.Damage, n.damage, ModeChecksDamageScale());
                    n.life = n.lifeMax;
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.SyncNPC, number: n.whoAmI);
                }
                init = true;
            }
        }
        public static float ModeChecksLifeScale()
        {
            float multiplier = 1f;
            if (!ModContent.GetInstance<ModeToggle>().archaeaMode)
                return multiplier;
            multiplier -= scaling[start];
            foreach (Player player in Main.player)
            {
                if (player != null)
                {
                    if (player.statLifeMax >= ArchaeaMode.LifeCrystal(100))
                    {
                        multiplier -= scaling[health];
                        break;
                    }
                    if (player.statManaMax >= ArchaeaMode.ManaCrystal(80))
                    {
                        multiplier -= scaling[mana];
                        break;
                    }
                    if (player.position.Y > Main.bottomWorld * 0.75f)
                    {
                        multiplier -= scaling[bottom];
                        break;
                    }
                }
            }
            int count = 0;
            for (int i = 0; i < Main.townNPCCanSpawn.Length; i++)
            {
                if (Main.townNPCCanSpawn[i])
                {
                    if (count++ > 4)
                    {
                        multiplier -= scaling[npcs];
                        break;
                    }
                }
            }
            if (ModContent.GetInstance<ModeToggle>().dayCount > 6)
            {
                multiplier -= scaling[week];
            }
            if (ModContent.GetInstance<ModeTile>().tileProgress)
            {
                multiplier -= scaling[crafting];
            }
            if (ModContent.GetInstance<ArchaeaWorld>().downedMagno)
            {
                multiplier -= scaling[downedMagno];
            }
            ModContent.GetInstance<ModeToggle>().healthScale = Math.Max(multiplier, 0.4f);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetHandler.Send(Packet.ModeScaling, 256);
            return Math.Max(multiplier, 0.4f);
        }
        public static float ModeChecksDamageScale()
        {
            float multiplier = 1f;
            if (!ModContent.GetInstance<ModeToggle>().archaeaMode)
                return multiplier;
            multiplier -= scaling[start];
            foreach (Player player in Main.player)
            {
                if (player != null && player.active)
                {
                    if (player.statLifeMax >= ArchaeaMode.LifeCrystal(100))
                    {
                        multiplier -= scaling[health];
                        break;
                    }
                    if (player.statManaMax >= ArchaeaMode.ManaCrystal(80))
                    {
                        multiplier -= scaling[mana];
                        break;
                    }
                    if (player.position.Y > Main.bottomWorld * 0.75f)
                    {
                        multiplier -= scaling[bottom];
                        break;
                    }
                }
            }
            int count = 0;
            for (int i = 0; i < Main.townNPCCanSpawn.Length; i++)
            {
                if (Main.townNPCCanSpawn[i])
                {
                    if (count++ > 4)
                    {
                        multiplier -= scaling[npcs];
                        break;
                    }
                }
            }
            if (ModContent.GetInstance<ModeToggle>().dayCount > 6)
            {
                multiplier -= scaling[week];
            }
            if (ModContent.GetInstance<ModeTile>().tileProgress)
            {
                multiplier -= scaling[crafting];
            }
            if (ModContent.GetInstance<ArchaeaWorld>().downedMagno)
            {
                multiplier -= scaling[downedMagno];
            }
            ModContent.GetInstance<ModeToggle>().damageScale = Math.Max(multiplier, 0.4f);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetHandler.Send(Packet.ModeScaling, 256);
            return Math.Max(multiplier, 0.4f);
        }
    }
    public class ModeItem : GlobalItem
    {
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (ModContent.GetInstance<ModeToggle>().archaeaMode)
            {
                damage = new StatModifier(1f, Math.Abs(ModContent.GetInstance<ModeToggle>().damageScale - 2f));
            }
        }
    }        
    public class ModeProjectile : GlobalProjectile
    {
        public override void ModifyDamageScaling(Projectile projectile, ref float damageScale)
        {
            if (ModContent.GetInstance<ModeToggle>().archaeaMode)
            {
                damageScale *= Math.Abs(ModContent.GetInstance<ModeToggle>().damageScale - 2f);
            }
        }
    }
    public class ModeTile : GlobalTile
    {
        public bool tileProgress;
        private int[,] playerCrafting = new int[5,3];
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            int[] crafting = new int[] 
            { 
                TileID.Anvils, 
                TileID.MythrilAnvil, 
                TileID.Furnaces,
                TileID.LihzahrdFurnace,
                TileID.Benches
            };
            for (int l = 0; l < crafting.Length; l++)
            {
                if (item.createTile == crafting[l])
                {
                    type = crafting[l];
                    break;
                }
            }
            if (type == 0) 
            {
                return;
            }
            int index = 0;
            for (int k = 0; k < 5; k++)
            {
                if (playerCrafting[k,0] == 0)
                {
                    index = k;
                    tileProgress = false;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetHandler.Send(Packet.TileProgress, 256, b: false);
                    break;
                }
                else if (k == 4)
                {
                    tileProgress = true;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetHandler.Send(Packet.TileProgress, 256, b: true);
                    return;
                }
            }
            foreach (int m in playerCrafting)
            {
                if (m == type)
                {
                    return;
                }
            }
            playerCrafting[index,0] = type;
            playerCrafting[index,1] = i;
            playerCrafting[index,2] = j;
        }
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            for (int k = 0; k < playerCrafting.GetLength(0); k++)
            {
                if (type == playerCrafting[k, 0])
                {
                    for (int m = -2; m <= 2; m++)
                    for (int n = -2; n <= 2; n++)
                    {
                        if (playerCrafting[k, 1] - m == i && playerCrafting[k, 2] - n == j)
                        {
                            playerCrafting[k, 0] = 0;
                            break;
                        }
                    }
                }
            }
        }
    }
}

