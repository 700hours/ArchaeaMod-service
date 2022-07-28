using System.IO;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

using ArchaeaMod.Entities;
using Terraria.ID;

namespace ArchaeaMod
{
    public class ArchaeaMain : Mod
    {
        public static Mod getMod => ModLoader.GetMod("ArchaeaMod");
        
        public static string magnoHead = "ArchaeaMod/Gores/magno_head";
        public static string skyHead = "ArchaeaMod/Gores/sky_head";
        //public static ModHotKey[] macro = new ModHotKey[5];
        public override void Load()
        {
            AddBossHeadTexture(magnoHead, ModNPCID.MagnoliacHead);
            AddBossHeadTexture(skyHead, ModNPCID.SkyBoss);
            if (!Main.dedServ)
            {
                MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/The_Undying_Flare"), ModContent.ItemType<Items.Tiles.mbox_magno_boss>(), ModContent.TileType<Tiles.music_boxes>(), 0);
                MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/Magno_Biome"), ModContent.ItemType<Items.Tiles.mbox_magno_1>(), ModContent.TileType<Tiles.music_boxes>(), 36);
                MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/Dark_and_Evil_with_a_hint_of_Magma"), ModContent.ItemType<Items.Tiles.mbox_magno_2>(), ModContent.TileType<Tiles.music_boxes_alt>(), 36);
            }
            //for (int i = 0; i < macro.Length; i++)
            //{
            //    macro[i] = RegisterHotKey($"Macro {i + 1}", "");
            //}
        }
        //public void SetModInfo(out string name, ref ModProperties properties)
        //{
        //    name = "Archaea Mod";
        //    properties.Autoload = true;
        //    properties.AutoloadBackgrounds = true;
        //    properties.AutoloadGores = true;
        //    properties.AutoloadSounds = true;
        //}
        
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetHandler.Receive(reader);
        }
    }
    public class SkyFortPortalBiome : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override bool IsBiomeActive(Player player)
        {
            return Main.SceneMetrics.GetTileCount((ushort)ModContent.TileType<Tiles.sky_portal>()) > 0;
        }
    }
    public class SkyFortBiome : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override bool IsBiomeActive(Player player)
        {
            return Main.SceneMetrics.GetTileCount(ArchaeaWorld.skyBrick) >= 80;
        }
    }
    public class MagnoBiome : ModBiome
    {
        private bool swapTracks;
        private bool triggerSwap;
        public override string MapBackground => "Backgrounds/MapBGMagno";
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetModUndergroundBackgroundStyle(Backgrounds.bg_style.Style);
        public override int Music => UpdateMusic();
        public override bool IsBiomeActive(Player player)
        {
            return Main.SceneMetrics.GetTileCount(ArchaeaWorld.magnoStone) >= 80;
        }
        public int UpdateMusic()
        {
            if ((int)Main.time == (int)Main.dayLength / 2)
                triggerSwap = true;
            if (triggerSwap)
            {
                swapTracks = !swapTracks;
                triggerSwap = false;
            }
            return swapTracks ? MusicLoader.GetMusicSlot(this.Mod, "Sounds/Music/Dark_and_Evil_with_a_hint_of_Magma") : MusicLoader.GetMusicSlot(this.Mod, "Sounds/Music/Magno_Biome");
        }
    }
    public class NetHandler
    {
        private static Mod mod
        {
            get { return ArchaeaMain.getMod; }
        }
        public static void Send(byte type, int toWho = -1, int fromWho = -1, int i = 0, float f = 0f, float f2 = 0f, int i2 = 0, bool b = false, float f3 = 0f, float f4 = 0f, float f5 = 0f)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write(type);
            packet.Write(i);
            packet.Write(f);
            packet.Write(f2);
            packet.Write(i2);
            packet.Write(b);
            packet.Write(f3);
            packet.Write(f4);
            packet.Write(f5);
            packet.Send(toWho, fromWho);
        }
        public static void Receive(BinaryReader reader)
        {
            if (reader.PeekChar() == -1)
                return;
            int n = 0, n2 = 0;
            byte type = reader.ReadByte();
            int t = reader.ReadInt32();
            float f = reader.ReadSingle();
            float f2 = reader.ReadSingle();
            int i = reader.ReadInt32();
            bool b = reader.ReadBoolean();
            float f3 = reader.ReadSingle();
            float f4 = reader.ReadSingle();;
            float f5 = reader.ReadSingle();
            switch (type)
            {
                case Packet.WorldTime:
                    Main.dayTime = b;
                    Main.time = f;
                    NetMessage.SendData(7, -1, -1, null);
                    break;
                case Packet.SpawnNPC:
                    if (f5 != 0f)
                    {
                        n = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)f4, (int)f5, t, 0);
                        Main.npc[n].whoAmI = n;
                        Main.npc[n].lifeMax = (int)f;
                        Main.npc[n].life = (int)f;
                        Main.npc[n].defense = (int)f2;
                        Main.npc[n].damage = i;
                        Main.npc[n].knockBackResist = f3;
                        NetMessage.SendData(23, -1, -1, null, n);
                        return;
                    }
                    else if (!b)
                    {
                        n = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)f, (int)f2, t);
                        Main.npc[n].whoAmI = n;
                        NetMessage.SendData(23, -1, -1, null, n);
                    }
                    else NPC.SpawnOnPlayer(i, t);
                    break;
                case Packet.SpawnItem:
                    Main.item[t].whoAmI = t;
                    NetMessage.SendData(21, -1, -1, null, t);
                    break;
                case Packet.TeleportPlayer:
                    if (Main.netMode == 2)
                        Send(Packet.TeleportPlayer, t, -1, t, f, f2);
                    else if (t == Main.LocalPlayer.whoAmI)
                    {
                        Main.player[t].Teleport(new Vector2(f, f2));
                    }
                    break;
                case Packet.StrikeNPC:
                    Main.npc[t].StrikeNPC(i, f, 0);
                    NetMessage.SendData(28, -1, -1, null, t);
                    break;
                case Packet.ArchaeaMode:
                    ModContent.GetInstance<Mode.ModeToggle>().archaeaMode = b;
                    if (Main.netMode == NetmodeID.Server)
                        Send(Packet.ArchaeaMode, b: b);
                    break;
                case Packet.SyncClass:
                    break;
                case Packet.SyncInput:
                    if (Main.netMode == 2)
                        Send(Packet.SyncInput, t, -1);
                    else
                    {
                        Mode.ModeToggle modWorld = ModContent.GetInstance<Mode.ModeToggle>();
                        modWorld.progress = !modWorld.progress;
                    }
                    break;
                case Packet.SyncEntity:
                    if (Main.netMode == 2)
                        Send(Packet.SyncEntity, -1, -1, t, f, f2, i, b, f3);
                    else 
                    {
                        if (ArchaeaEntity.entity[t] != null)
                        {
                            ArchaeaEntity.entity[t].active = b;
                            ArchaeaEntity.entity[t].owner = (int)f3;
                            if (f != 0f)
                                ArchaeaEntity.entity[t].rotation = f;
                        }
                    }
                    break;
                case Packet.Debug:
                    if (Main.netMode == 2)
                        Send(Packet.Debug, t, -1, t, f);
                    else
                    {
                        if (t == Main.LocalPlayer.whoAmI)
                        {
                            var modPlayer = Main.player[t].GetModPlayer<ArchaeaPlayer>();
                            switch ((int)f)
                            {
                                case 0:
                                    modPlayer.debugMenu = !modPlayer.debugMenu;
                                    break;
                                case 1:
                                    modPlayer.spawnMenu = !modPlayer.spawnMenu;
                                    break;
                            }
                        }
                    }
                    break;
                case Packet.TileExplode:
                    if (f < f2)
                        ArchaeaMod.Merged.Tiles.m_ore.TileExplode(t, i);
                    break;
                case Packet.DownedMagno:
                    ModContent.GetInstance<ArchaeaWorld>().downedMagno = true;
                    break;
                case Packet.ModeScaling:
                    ModContent.GetInstance<Mode.ModeToggle>().damageScale = f;
                    ModContent.GetInstance<Mode.ModeToggle>().healthScale = f2;
                    if (Main.netMode == NetmodeID.Server)
                        NetHandler.Send(Packet.ModeScaling, f: f, f2: f2);
                    break;
                case Packet.TileProgress:
                    ModContent.GetInstance<Mode.ModeTile>().tileProgress = b;
                    if (Main.netMode == NetmodeID.Server)
                        NetHandler.Send(Packet.TileProgress, b: b);
                    break;
            }
        }
    }
    public class Packet
    {
        public const byte
            WorldTime = 1,
            SpawnNPC = 2,
            SpawnItem = 3,
            TeleportPlayer = 4,
            StrikeNPC = 5,
            ArchaeaMode = 6,
            SyncClass = 7,
            SyncInput = 8,
            SyncEntity = 9,
            Debug = 10,
            TileExplode = 11,
            DownedMagno = 12,
            ModOptions = 13,
            ModeScaling = 14,
            TileProgress = 15;
    }
}
