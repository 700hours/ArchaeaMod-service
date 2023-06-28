using ArchaeaMod.Jobs.Projectiles;
using ArchaeaMod.NPCs.Town;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static IL.Terraria.WorldBuilding.Searches;
using static System.Formats.Asn1.AsnWriter;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ArchaeaMod.Jobs.Buffs
{
    internal class Zombie : ModBuff
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Incognito");
            Main.buffNoSave[Type] = false;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip = "\"Groan\"";
        }
        public const int MaxTime = 7200;
        int npcIndex;
        NPC mask => Main.npc[npcIndex];
        public override bool RightClick(int buffIndex)
        {
            if (mask.active)
            {
                mask.life = -1;
                mask.checkDead();
            }
            return true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] == MaxTime)
            {
                npcIndex = NPC.NewNPC(NPC.GetSource_None(), (int)player.position.X, (int)player.position.Y, Main.rand.Next(new[] { NPCID.Zombie, NPCID.ZombieDoctor, NPCID.ZombieElf, NPCID.ZombieElfBeard, NPCID.ZombieEskimo, NPCID.ZombieMerman, NPCID.ZombieMushroom, NPCID.ZombiePixie, NPCID.ZombieRaincoat, NPCID.ZombieSuperman, NPCID.ZombieSweater }));
                Main.npc[npcIndex].friendly = true;
                int projType = Projectile.NewProjectile(Projectile.GetSource_None(), player.position, Vector2.Zero, ModContent.ProjectileType<fake_npc>(), 0, 0f, player.whoAmI, index, 0);
                fake_npc.SetFollowType(Main.projectile[projType], FollowID.Replace);
            }
            player.moveSpeed /= 2f;
            player.statDefense = 0;
            player.immune = true;
            player.immuneAlpha = 0;
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active) continue;
                if (npc.target == player.whoAmI)
                {
                    npc.target = 255;
                }
                else continue;
            }
            if (!mask.active)
            {
                player.DelBuff(buffIndex--);
            }
        }
        /*	public static Texture2D
                armorLeg = Main.Assets.Request[Config.goreID["Zombie Legs"]],
                armorBody = Main.goreTexture[Config.goreID["Zombie Body"]],
                armorHead = Main.goreTexture[Config.goreID["Zombie Head"]],
                armorArm = Main.goreTexture[Config.goreID["null"]],
                armorHands = Main.goreTexture[Config.goreID["null"]],
                armorHands2 = Main.goreTexture[Config.goreID["null"]],
                oldArmorLegTexture, 
                oldArmorBodyTexture, 
                oldArmorHeadTexture, 
                oldArmorArmTexture, 
                oldPlayerHandsTexture, 
                oldPlayerHands2Texture;
            public void EffectsStart(Player player,int buffIndex,int buffType,int buffTime) 
            {
                if(player.armor[0].stack <= 0) player.armor[0].SetDefaults("Copper Helmet");
                if(player.armor[1].stack <= 0) player.armor[1].SetDefaults("Copper Chainmail");
                if(player.armor[2].stack <= 0) player.armor[2].SetDefaults("Copper Greaves");
                if(Main.armorLegTexture[player.legs] != null) oldArmorLegTexture = Main.armorLegTexture[player.legs];
                if(Main.armorBodyTexture[player.body] != null) oldArmorBodyTexture = Main.armorBodyTexture[player.body];
                if(Main.armorArmTexture[player.body] != null) oldArmorArmTexture = Main.armorArmTexture[player.body];
                if(Main.playerHandsTexture != null) oldPlayerHandsTexture = Main.playerHandsTexture;
                if(Main.playerHands2Texture != null) oldPlayerHands2Texture = Main.playerHands2Texture;
                if(Main.armorHeadTexture[player.head] != null) oldArmorHeadTexture = Main.armorHeadTexture[player.head];
            }
            public void Effects(Player player,int buffIndex,int buffType,int buffTime) 
            {
                if(player.armor[0].stack <= 0) player.armor[0].SetDefaults("Copper Helmet");
                if(player.armor[1].stack <= 0) player.armor[1].SetDefaults("Copper Chainmail");
                if(player.armor[2].stack <= 0) player.armor[2].SetDefaults("Copper Greaves");
                Main.armorHeadTexture[player.head] = armorHead;
                Main.armorBodyTexture[player.body] = armorBody;
                Main.armorArmTexture[player.body] = armorArm;
                Main.playerHandsTexture = armorHands;
                Main.playerHands2Texture = armorHands2;
                Main.armorLegTexture[player.legs] = armorLeg;
                player.moveSpeed = 0.5f;
                player.statDefense = 0;
                player.controlUseItem = false;
                player.controlJump = false;
                player.immune = true;
                player.immuneAlpha = 0;
            }
            public void EffectsEnd(Player player,int buffIndex,int buffType,int buffTime)
            {
                if(oldArmorHeadTexture != null) Main.armorHeadTexture[player.head] = oldArmorHeadTexture;
                if(oldArmorBodyTexture != null) Main.armorBodyTexture[player.body] = oldArmorBodyTexture;
                if(oldArmorArmTexture != null) Main.armorArmTexture[player.body] = oldArmorArmTexture;
                if(oldPlayerHandsTexture != null) Main.playerHandsTexture = oldPlayerHandsTexture;
                if(oldPlayerHands2Texture != null) Main.playerHands2Texture = oldPlayerHands2Texture;
                if(oldArmorLegTexture != null) Main.armorLegTexture[player.legs] = oldArmorLegTexture;
            }				  */
    }
}