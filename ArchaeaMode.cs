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
using Terraria.DataStructures;
using System.Timers;

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
        public static int ModeScaling(Stat stat, int value, float scale, int defense, DamageClass damage)
        {
            float quotient = Math.Min(defense / 999f, 0.9f);
            float mitigate = Math.Abs(quotient - 1f);
            float bonus = 1f;
            switch (damage)
            {
                case DefaultDamageClass:
                    break;
                case MagicDamageClass:
                    bonus = mitigate + 1f;
                    break;
                case RangedDamageClass:
                    break;
                case SummonDamageClass:
                    break;
                case MeleeDamageClass:
                    bonus = quotient + 1f;
                    break;
                case ThrowingDamageClass:
                    break;
            }
            
            // Ratio
            float ratio = 500f / 9999f;
            float result = value / ratio / scale;
            switch (stat)
            {
                case Stat.Life:
                    break;
                case Stat.Damage:
                    result *= bonus;
                    result *= mitigate;
                    break;
            }
            return (int)result;
        }
        private int _ModeScaling(Stat stat, int value, float scale)
        {
            /* Ratio
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
        public override void OnWorldLoad()
        {
            if (timer != null)
                timer.Dispose();
            timer = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds);
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                archaeaMode = false;
                healthScale = 1f;
                damageScale = 1f;
                dayCount = 0;
                totalTime = 0;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            healthScale = ModeChecksLifeScale();
            damageScale = ModeChecksDamageScale();
        }

        public bool archaeaMode;
        public bool progress;
        public float healthScale;
        public float damageScale;
        public float dayCount;
        public float totalTime;
        private Timer timer;

        public static Color[] unlock = new Color[9];
        public readonly int start = 0, health = 1, mana = 2, bosses = 3, bottom = 4, npcs = 5, week = 6, crafting = 7, downedMagno = 8;
        public readonly float[] scaling = new float[] { 0.05f, 0.1f, 0.05f, 0f, 0.2f, 0.1f, 0.5f, 0.05f, 0.1f };

        public float ModeChecksLifeScale()
        {
            float multiplier = 1f;
            if (!archaeaMode)
                return multiplier;
            multiplier -= 0.5f;
            for (int i = 0; i < unlock.Length; i++)
                unlock[i] = Color.Red;
            multiplier -= scaling[start];
            unlock[start] = Color.Green;
            foreach (Player player in Main.player)
            {
                if (player.active)
                {
                    if (player.statLifeMax >= ArchaeaMode.LifeCrystal(100))
                    {
                        unlock[health] = Color.Green;
                        multiplier -= scaling[health];
                    }
                    if (player.statManaMax >= 40 || player.statManaMax2 >= 40)
                    {
                        unlock[mana] = Color.Green;
                        multiplier -= scaling[mana];
                    }
                    if (player.position.Y > Main.bottomWorld * 0.75f)
                    {
                        unlock[bottom] = Color.Green;
                        multiplier -= scaling[bottom];
                    }
                    break;
                }
            }
            int count = 0;
            for (int i = 0; i < Main.townNPCCanSpawn.Length; i++)
            {
                if (Main.townNPCCanSpawn[i])
                {
                    if (count++ > 4)
                    {
                        unlock[npcs] = Color.Green;
                        multiplier -= scaling[npcs];
                        break;
                    }
                }
            }
            if (ModContent.GetInstance<ModeToggle>().dayCount > 6)
            {
                unlock[week] = Color.Green;
                multiplier -= scaling[week];
            }
            if (ModContent.GetInstance<ModeTile>().tileProgress)
            {
                unlock[crafting] = Color.Green;
                multiplier -= scaling[crafting];
            }
            if (ModContent.GetInstance<ArchaeaWorld>().downedMagno)
            {
                unlock[downedMagno] = Color.Green;
                multiplier -= scaling[downedMagno];
            }
            multiplier = (float)Math.Round(Math.Abs(multiplier - 2f), 2);
            return multiplier;
        }
        public float ModeChecksDamageScale()
        {
            float multiplier = 1f;
            if (!archaeaMode)
                return multiplier;
            multiplier -= 0.5f / 2;
            multiplier -= scaling[start];
            foreach (Player player in Main.player)
            {
                if (player != null && player.active)
                {
                    if (player.statLifeMax >= ArchaeaMode.LifeCrystal(100))
                    {
                        multiplier -= scaling[health];
                    }
                    if (player.statManaMax >= 40 || player.statManaMax2 >= 40)
                    { 
                        multiplier -= scaling[mana];
                    }
                    if (player.position.Y > Main.bottomWorld * 0.75f)
                    {
                        multiplier -= scaling[bottom];
                    }
                    break;
                }
            }
            int count = 0;
            for (int i = 0; i < Main.townNPCCanSpawn.Length; i++)
            {
                if (Main.townNPCCanSpawn[i])
                {
                    if (count++ > 4)
                    {
                        multiplier -= scaling[npcs] / 2f;
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
                multiplier -= scaling[downedMagno] / 2f;
            }
            multiplier = (float)Math.Round(Math.Abs(multiplier - 2f), 2);
            return multiplier;
        }
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
        public void SetArchaeaMode(bool flag)
        {
            if (ModContent.GetInstance<ModeToggle>().archaeaMode != flag)
            { 
                ModContent.GetInstance<ModeToggle>().archaeaMode = flag;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetHandler.Send(Packet.ArchaeaMode, 256, -1, 0, healthScale, damageScale, 0, archaeaMode, dayCount, totalTime);
            }
        }
        public void SetCordonedBiomes(bool flag)
        {
            ModContent.GetInstance<ArchaeaWorld>().cordonBounds = flag;
            //if (Main.netMode == NetmodeID.MultiplayerClient)
            //    NetHandler.Send(Packet.CordonedBiomes, 256);
        }

        /*
        public override void NetSend(BinaryWriter writer)
        {
            
            writer.Write(archaeaMode);
            writer.Write(totalTime);
            writer.Write(dayCount);
            writer.Write(damageScale);
            writer.Write(healthScale);
        }
        public override void NetReceive(BinaryReader reader)
        {
            archaeaMode = reader.ReadBoolean();
            totalTime = reader.ReadSingle();
            dayCount = reader.ReadSingle();
            damageScale = reader.ReadSingle();
            healthScale = reader.ReadSingle();
        }
        */
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
        public override bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
        {
            switch (messageType)
            {
                case MessageID.PlayerInfo:
                case MessageID.PlayerSpawn:
                case MessageID.WorldData:
                    healthScale = ModeChecksLifeScale();
                    damageScale = ModeChecksDamageScale();
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetHandler.Send(Packet.ArchaeaMode, 256, -1, 0, healthScale, damageScale, 0, archaeaMode, dayCount, totalTime);
                    break;
            }
            return base.HijackGetData(ref messageType, ref reader, playerNumber);
        }
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
                    sb.DrawString(FontAssets.MouseText.Value, "Life scale: " + healthScale, new Vector2(panel.Left + 4, panel.Top + 4), Color.White);
                    sb.DrawString(FontAssets.MouseText.Value, "Damage scale: " + damageScale, new Vector2(panel.Left + 4, panel.Top + 24), Color.White);
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
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (ModContent.GetInstance<ModeToggle>().archaeaMode)
            {
                npc.lifeMax = ArchaeaMode.ModeScaling(ArchaeaMode.Stat.Life, npc.lifeMax, ModContent.GetInstance<ModeToggle>().healthScale, npc.defense, DamageClass.Default);
                npc.life = npc.lifeMax;
                if (Main.netMode == NetmodeID.Server)
                    npc.netUpdate = true;
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            damage = ArchaeaMode.ModeScaling(ArchaeaMode.Stat.Damage, damage, ModContent.GetInstance<ModeToggle>().damageScale, npc.defense, item.DamageType);
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = ArchaeaMode.ModeScaling(ArchaeaMode.Stat.Damage, damage, ModContent.GetInstance<ModeToggle>().damageScale, npc.defense, projectile.DamageType);
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
                    if (Main.netMode == NetmodeID.Server)
                        NetHandler.Send(Packet.TileProgress, b: false);
                    break;
                }
                else if (k == 4)
                {
                    tileProgress = true;
                    if (Main.netMode == NetmodeID.Server)
                        NetHandler.Send(Packet.TileProgress, b: true);
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

