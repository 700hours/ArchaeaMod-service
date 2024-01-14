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

using ArchaeaMod.Interface.UI;
using Terraria.DataStructures;
using System.Timers;
using ArchaeaMod.Progression;
using ArchaeaMod.NPCs.Town;
using Terraria.Audio;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ArchaeaMod.Mode
{
    public sealed class ArchaeaMode : ModSystem
    {
        public enum Stat
        {
            None,
            Life,
            Damage
        }
        public enum StatWho
        {
            None,
            Player,
            NPC
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
            float a = amount / 400f;
            int result = (int)a * 4000; // not done yet
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
        public static int ModeScaling(StatWho who, Stat stat, int value, float scale, int defense, DamageClass damage)
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

            // Original ratio
            // float ratio = 500f / 9999f;
            // float result = value / ratio * scale;
            float ratio = 500f / 999f;
            float result = value / ratio;
            switch (who)
            {
                case StatWho.None:
                    break;
                //  Player.PreHurt
                case StatWho.Player:
                    result *= scale;
                    break;
                //  NPC.OnSpawn
                case StatWho.NPC:
                    result *= scale;
                    break;
            }
            switch (stat)
            {
                case Stat.None:
                    break;
                case Stat.Life:
                    break;
                // NPC.ModifyHitByItem & NPC.ModifyHitByProjectile
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
        public static Color[] unlock = new Color[9];
        public static readonly int start = 0, health = 1, mana = 2, bosses = 3, bottom = 4, npcs = 5, week = 6, crafting = 7, downedMagno = 8;
        public static readonly float[] scaling = new float[] { 0.05f, 0.1f, 0.05f, 0f, 0.2f, 0.1f, 0.5f, 0.05f, 0.1f };
        public bool
            Start,
            Health,
            Mana,
            Bosses,
            Bottom,
            NPCs,
            Week,
            Crafting,
            DownedMagno;
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(Start);
            writer.Write(Health);
            writer.Write(Mana);
            writer.Write(Bosses);
            writer.Write(Bottom);
            writer.Write(NPCs);
            writer.Write(Week);
            writer.Write(Crafting);
            writer.Write(DownedMagno);
        }
        public override void NetReceive(BinaryReader reader)
        {
            Start       = reader.ReadBoolean();
            Health      = reader.ReadBoolean();
            Mana        = reader.ReadBoolean();
            Bosses      = reader.ReadBoolean();
            Bottom      = reader.ReadBoolean();
            NPCs        = reader.ReadBoolean();
            Week        = reader.ReadBoolean();
            Crafting    = reader.ReadBoolean();
            DownedMagno = reader.ReadBoolean();
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("Start", Start);
            tag.Add("Health", Health);
            tag.Add("Mana", Mana);
            tag.Add("Bosses", Bosses);
            tag.Add("Bottom", Bottom);
            tag.Add("NPCs", NPCs);
            tag.Add("Week", Week);
            tag.Add("Crafting", Crafting);
            tag.Add("DownedMagno", DownedMagno);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            Start = tag.GetBool("Start");
            Health = tag.GetBool("Health");
            Mana = tag.GetBool("Mana");
            Bosses = tag.GetBool("Bosses");
            NPCs = tag.GetBool("NPCs");
            Week = tag.GetBool("Week");
            Crafting = tag.GetBool("Crafting");
            DownedMagno = tag.GetBool("DownedMagno");
        }
        private int Convert(bool flag)
        {
            return flag ? 1 : 0;
        }
        public static bool ConvertToBool(int num)
        {
            return num == 1 ? true : false;
        }
        public override bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
        {
            return base.HijackGetData(ref messageType, ref reader, playerNumber);
            switch (messageType)
            {
                case MessageID.SyncTalkNPC:
                    //  Doesn't occur in singleplayer
                    break;
                case MessageID.PlayerInfo:
                case MessageID.PlayerSpawn:
                case MessageID.WorldData:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        //NetHandler.Send(Packet.ModeProgress, 256, -1, Convert(Start), Convert(Health), Convert(Mana), Convert(Bottom), NPCs, Convert(Week), Convert(Crafting), Convert(DownedMagno));
                    }
                    break;
            }
        }
        public float ModeChecksLifeScale(bool mode)
        {
            float multiplier = 1f;
            if (!mode)
                return multiplier;
            multiplier -= 0.5f;
            Start = true;
            for (int i = 0; i < unlock.Length; i++)
                unlock[i] = ModeUI.innactiveColor;
            multiplier -= scaling[start];
            unlock[start] = ModeUI.activeColor;
            if (!Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().objectiveStat[start])
            {
                Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().remainingStat++;
                Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().overallMaxStat++;
                Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().objectiveStat[start] = true;
            }
            Start = true;
            foreach (Player player in Main.player)
            {
                if (player.active)
                {
                    if (Health || player.statLifeMax >= ArchaeaMode.LifeCrystal(20))
                    {
                        unlock[health] = ModeUI.activeColor;
                        if (!player.GetModPlayer<ArchaeaPlayer>().objectiveStat[health])
                        {
                            player.GetModPlayer<ArchaeaPlayer>().remainingStat++;
                            Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().overallMaxStat++;
                            player.GetModPlayer<ArchaeaPlayer>().objectiveStat[health] = true;
                        }
                        multiplier -= scaling[health];
                        Health = true;
                    }
                    if (Mana || player.statManaMax >= 40 || player.statManaMax2 >= 40)
                    {
                        unlock[mana] = ModeUI.activeColor;
                        if (!player.GetModPlayer<ArchaeaPlayer>().objectiveStat[mana])
                        {
                            player.GetModPlayer<ArchaeaPlayer>().remainingStat++;
                            Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().overallMaxStat++;
                            player.GetModPlayer<ArchaeaPlayer>().objectiveStat[mana] = true;
                        }
                        multiplier -= scaling[mana];
                        Mana = true;
                    }
                    if (Bottom || player.position.Y >= Main.maxTilesY * 16 - player.height)
                    {
                        unlock[bottom] = ModeUI.activeColor;
                        if (!player.GetModPlayer<ArchaeaPlayer>().objectiveStat[bottom])
                        {
                            player.GetModPlayer<ArchaeaPlayer>().remainingStat++;
                            Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().overallMaxStat++;
                            player.GetModPlayer<ArchaeaPlayer>().objectiveStat[bottom] = true;
                        }
                        multiplier -= scaling[bottom];
                        Bottom = true;
                    }
                    break;
                }
            }
            int count = 0;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (NPCs || Main.npc[i].townNPC && count++ > 4)
                {
                    unlock[npcs] = ModeUI.activeColor;
                    if (!Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().objectiveStat[bottom])
                    {
                        Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().remainingStat++;
                        Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().overallMaxStat++;
                        Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().objectiveStat[bottom] = true;
                    }
                    multiplier -= scaling[npcs];
                    NPCs = true;
                    break;
                }
            }
            if (Bosses)
            {
                unlock[bosses] = ModeUI.activeColor;
                if (!Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().objectiveStat[bosses])
                {
                    Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().remainingStat++;
                    Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().overallMaxStat++;
                    Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().objectiveStat[bosses] = true;
                }
                multiplier -= scaling[bosses];
            }
            if (Week || ModContent.GetInstance<ModeToggle>().dayCount > 6)
            {
                unlock[week] = ModeUI.activeColor;
                multiplier -= scaling[week];
                if (!Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().objectiveStat[week])
                {
                    Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().remainingStat++;
                    Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().overallMaxStat++;
                    Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().objectiveStat[week] = true;
                }
                Week = true;
            }
            if (Crafting || ModContent.GetInstance<ModeTile>().tileProgress)
            {
                unlock[crafting] = ModeUI.activeColor;
                multiplier -= scaling[crafting];
                if (!Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().objectiveStat[crafting])
                {
                    Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().remainingStat++;
                    Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().overallMaxStat++;
                    Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().objectiveStat[crafting] = true;
                }
                Crafting = true;
            }
            if (DownedMagno || ModContent.GetInstance<ArchaeaWorld>().downedMagno)
            {
                unlock[downedMagno] = ModeUI.activeColor;
                if (!Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().objectiveStat[downedMagno])
                {
                    Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().remainingStat++;
                    Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().overallMaxStat++;
                    Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().objectiveStat[downedMagno] = true;
                }
                multiplier -= scaling[downedMagno];
                DownedMagno = true;
            }
            multiplier = (float)Math.Round(Math.Abs(multiplier - 2f), 2);
            return multiplier;
        }
        public float ModeChecksDamageScale(bool mode)
        {
            float multiplier = 1f;
            if (!mode)
                return multiplier;
            multiplier -= 0.5f / 2;
            Start = true;
            multiplier -= scaling[start];
            foreach (Player player in Main.player)
            {
                if (player != null && player.active)
                {
                    if (Health || player.statLifeMax >= ArchaeaMode.LifeCrystal(20))
                    {
                        multiplier -= scaling[health];
                    }
                    if (Mana || player.statManaMax >= 40 || player.statManaMax2 >= 40)
                    {
                        multiplier -= scaling[mana];
                    }
                    if (Bottom || player.position.Y > Main.bottomWorld * 0.75f)
                    {
                        multiplier -= scaling[bottom];
                    }
                    break;
                }
            }
            int count = 0;
            for (int i = 0; i < Main.townNPCCanSpawn.Length; i++)
            {
                if (NPCs || Main.townNPCCanSpawn[i] && count++ > 4)
                {
                    multiplier -= scaling[npcs] / 2f;
                    break;
                }
            }
            if (Week || ModContent.GetInstance<ModeToggle>().dayCount > 6)
            {
                multiplier -= scaling[week];
            }
            if (Crafting || ModContent.GetInstance<ModeTile>().tileProgress)          
            {
                multiplier -= scaling[crafting];
            }
            if (DownedMagno || ModContent.GetInstance<ArchaeaWorld>().downedMagno)
            {
                multiplier -= scaling[downedMagno] / 2f;
            }
            multiplier = (float)Math.Round(Math.Abs(multiplier - 2f), 2);
            return multiplier;
        }
}
    public class ModeToggle : ModSystem
    {
        public bool loading = true;
        public override void OnWorldLoad()
        {
            loading = true;
            /*
            if (timer != null)
                timer.Dispose();
            timer = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds);
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();*/

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                archaeaMode = false;
                healthScale = 1f;
                damageScale = 1f;
                dayCount = 0;
                totalTime = 0;
            }
        }

        private void LoadObjectivesMenu()
        {
            loading = false;
            if (ModContent.GetInstance<ArchaeaMode>() != null)
            {
                healthScale = ModContent.GetInstance<ArchaeaMode>().ModeChecksLifeScale(archaeaMode);
                damageScale = ModContent.GetInstance<ArchaeaMode>().ModeChecksDamageScale(archaeaMode);
            }
            if (ArchaeaPlayer.CheckHasTrait(TraitID.SUMMONER_MinionDmg, ClassID.Summoner, Main.myPlayer))
            {
                Main.player[Main.myPlayer].GetDamage(DamageClass.Summon) *= 1.20f;
            }
        }
        public override void PreSaveAndQuit()
        {
            Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().SetModeStats(false);
        }

        public bool notFirstJoin
        { 
            get { return ModContent.GetInstance<ArchaeaWorld>().notFirstJoin; }
            set { ModContent.GetInstance<ArchaeaWorld>().notFirstJoin = value; }
        }
        public bool archaeaMode;
        public bool progress;
        public float healthScale;
        public float damageScale;
        public float dayCount;
        public float totalTime;
        private Timer timer;

        
        public override void SaveWorldData(TagCompound tag)/* Edit tag parameter rather than returning new TagCompound */
        {                                       
            //tag.Add("Init", notFirstJoin);
            tag.Add("ArchaeaMode", archaeaMode);
            tag.Add("HealthScale", healthScale);
            tag.Add("DamageScale", damageScale);
            tag.Add("DayCount", dayCount);
            tag.Add("TotalTime", totalTime);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            //notFirstJoin = tag.GetBool("Init");
            if (tag.ContainsKey("ArchaeaMode"))
            {
                archaeaMode = tag.GetBool("ArchaeaMode");
                healthScale = tag.GetFloat("HealthScale");
                damageScale = tag.GetFloat("DamageScale");
                dayCount = tag.GetFloat("DayCount");
                totalTime = tag.GetFloat("TotalTime");
            }
            //if (!notFirstJoin)
            //{
            //    archaeaMode = false;
            //}
        }
        public void SetArchaeaMode(bool flag)
        {
            if (archaeaMode != flag)
            { 
                archaeaMode = flag;
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
        private Button followerButton;
        public override void PostUpdateEverything()
        {
            totalTime += Main.frameRate / 60f;
            dayCount = totalTime / (float)Main.dayLength;
            if (Main.dedServ)
                return;
            if (!init)
            {
                objectiveButton = new Button("Mode Status",    new Rectangle(20, 284, 10 * 11, 24));
                followerButton  = new Button("Stop followers", new Rectangle(20, 312, 10 * 12, 24));
                init = true;
            }
            if (objectiveButton.LeftClick() && Main.playerInventory)
            { 
                progress = !progress;
                SoundEngine.PlaySound(SoundID.MenuTick, Main.LocalPlayer.Center);
            }
            if (followerButton.LeftClick() && Main.playerInventory)
            {
                Main.projectile
                    .Where(t => t.active && t.owner == Main.LocalPlayer.whoAmI && t.type == ModContent.ProjectileType<Follower>())
                    .ToList()
                    .ForEach(t => t.active = false);
                SoundEngine.PlaySound(SoundID.MenuTick, Main.LocalPlayer.Center);
            }
            if (loading)
            {
                LoadObjectivesMenu();
            }
        }
        public override bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
        {
            switch (messageType)
            {
                case MessageID.PlayerInfo:
                case MessageID.PlayerSpawn:
                case MessageID.WorldData:
                    healthScale = ModContent.GetInstance<ArchaeaMode>().ModeChecksLifeScale(archaeaMode);
                    damageScale = ModContent.GetInstance<ArchaeaMode>().ModeChecksDamageScale(archaeaMode);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetHandler.Send(Packet.ArchaeaMode, 256, -1, 0, healthScale, damageScale, 0, archaeaMode, dayCount, totalTime);
                    break;
            }
            return base.HijackGetData(ref messageType, ref reader, playerNumber);
        }
        public override void PostDrawTiles()
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (!Main.hardMode)
            { 
                Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().DarkenedVision();
            }
            if (Main.playerInventory)
            {
                
                if (progress)
                {
                    Rectangle panel = new Rectangle(306 - 160, 300, 180, 100);
                    Utils.DrawInvBG(sb, panel, Color.DodgerBlue * 0.33f);
                    //sb.Draw(TextureAssets.MagicPixel.Value, panel, Color.DodgerBlue * 0.33f);
                    sb.DrawString(FontAssets.MouseText.Value, "Life scale: " + healthScale, new Vector2(panel.Left + 4, panel.Top + 4), Color.White);
                    sb.DrawString(FontAssets.MouseText.Value, "Damage scale: " + damageScale, new Vector2(panel.Left + 4, panel.Top + 24), Color.White);
                    sb.DrawString(FontAssets.MouseText.Value, "Day: " + Math.Round(dayCount + 1, 0), new Vector2(panel.Left + 4, panel.Top + 44), Color.White);
                    sb.DrawString(FontAssets.MouseText.Value, "World time: " + Math.Round(totalTime / 60d / 60d, 1), new Vector2(panel.Left + 4, panel.Top + 64), Color.White);
                }
                if (archaeaMode)
                    objectiveButton.Draw();
                followerButton.Draw();
            }
            sb.End();
        }
    }
    public class ModeNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC npc)
        {
            if (ModContent.GetInstance<ModeToggle>().archaeaMode)
            {
                int lifeMax = ArchaeaMode.ModeScaling(ArchaeaMode.StatWho.NPC, ArchaeaMode.Stat.Life, npc.lifeMax, ModContent.GetInstance<ModeToggle>().healthScale, npc.defense, DamageClass.Default);
                npc.lifeMax = lifeMax;
                npc.life = lifeMax;
                npc.netUpdate = true;
            }
        }

        //  TODO: check scaling
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (ModContent.GetInstance<ModeToggle>().archaeaMode)
            {
                modifiers.FinalDamage *= ArchaeaMode.ModeScaling(ArchaeaMode.StatWho.None, ArchaeaMode.Stat.Damage, item.damage, ModContent.GetInstance<ModeToggle>().damageScale, npc.defense, item.DamageType);
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (ModContent.GetInstance<ModeToggle>().archaeaMode)
            {
                modifiers.FinalDamage *= ArchaeaMode.ModeScaling(ArchaeaMode.StatWho.None, ArchaeaMode.Stat.Damage, projectile.damage, ModContent.GetInstance<ModeToggle>().damageScale, npc.defense, projectile.DamageType);
            }
        }
        public override void OnKill(NPC npc)
        {
            if (npc.boss)
            {
                ModContent.GetInstance<ArchaeaMode>().Bosses = true;
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

