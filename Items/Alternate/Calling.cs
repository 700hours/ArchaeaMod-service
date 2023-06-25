using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace ArchaeaMod.Items.Alternate
{
    public class Calling : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Staff of Calling");
            Tooltip.SetDefault("Conjures a minion");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 0;
            Item.value = 3500;
            Item.useTime = 60;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Green;
        }
        private int count;
        private int minions;
        private int buffType
        {
            get { return (ModContent.BuffType<Minion.CallMinionBuff>()); }
        }
        [CloneByReference]
        private Projectile minion;
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
        {
            minions = count + player.numMinions;
            if (minions == player.maxMinions || player.HasBuff(buffType))
            {
                if (minion != null)
                    minion.active = false;
                minion = Projectile.NewProjectileDirect(Projectile.GetSource_None(), player.position - new Vector2(0, player.height), Vector2.Zero, ModContent.ProjectileType<Minion>(), 0, 0f, player.whoAmI);
            }
            if (!player.HasBuff(buffType))
            {
                player.AddBuff(buffType, 36000);
                minion = Projectile.NewProjectileDirect(Projectile.GetSource_None(), player.position - new Vector2(0, player.height), Vector2.Zero, ModContent.ProjectileType<Minion>(), 0, 0f, player.whoAmI);
                count = player.ownedProjectileCounts[minion.type];
            }
            return true;
        }
        public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/r_Catcher").Value;
            sb.Draw(tex, position, frame, Color.Firebrick * 0.67f, 0f, origin, 1f, SpriteEffects.None, 0f);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = Mod.Assets.Request<Texture2D>("Items/r_Catcher").Value;
            sb.Draw(tex, Item.position - Main.screenPosition, null, Color.Firebrick * 0.67f, 0f, new Vector2(tex.Width / 2, tex.Height / 2), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }

    public class Minion : ModProjectile
    {
        private int time
        {
            get { return (int)Projectile.localAI[0]; }
            set { Projectile.localAI[0] = value; }
        }
        private int ai = -1;
        private int coolDown;
        public const int
            Reset = -2,
            Start = -1,
            Move = 0,
            Active = 1,
            Attack = 2;
        private float range = 300f;
        private float distance;
        private float maxDistance = 180f;
        private float angle;
        public float moveSpeed = 1.5f;
        public Vector2 moveTo;
        public Player owner
        {
            get { return Main.player[Projectile.owner]; }
        }
        public Target follow;
        public Target target;
        public Target[] targets;
        private Draw draw;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slowing Minion");
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.damage = 0;
            Projectile.knockBack = 0f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }
        public override bool PreAI()
        {
            if (!owner.HasBuff(ModContent.BuffType<CallMinionBuff>()))
            {
                owner.numMinions -= owner.ownedProjectileCounts[Projectile.type];
                Projectile.active = false;
                return false;
            }
            if (targets == null)
                return true;
            switch (ai)
            {
                case Reset:
                    ai = Reset;
                    if ((distance -= 5f) == 0f)
                    {
                        ai = Move;
                        goto case Move;
                    }
                    break;
                case Start:
                    draw = new Draw();
                    Projectile.Center = Main.MouseWorld;
                    moveTo = Projectile.Center;
                    ai = Move;
                    goto case Move;
                case Move:
                    if (coolDown > 0)
                        coolDown--;
                    if (coolDown == 0)
                    {
                        angle = 0f;
                        if (LeftClick())
                            moveTo = Main.MouseWorld;
                        if (!Projectile.Hitbox.Contains(moveTo.ToPoint()))
                        {
                            Projectile.velocity += NPCs.ArchaeaNPC.AngleToSpeed(NPCs.ArchaeaNPC.AngleTo(Projectile.Center, moveTo), moveSpeed);
                            distance = 0f;
                        }
                        if (target != null && time-- < 0)
                        {
                            time = 0;
                            goto case Active;
                        }
                    }
                    else
                    {
                        moveTo = NPCs.ArchaeaNPC.AngleBased(owner.Center, angle += Draw.radian * 5f, 135f);
                        Projectile.Center += NPCs.ArchaeaNPC.AngleToSpeed(NPCs.ArchaeaNPC.AngleTo(Projectile.Center, moveTo), moveSpeed);
                        if (coolDown > 0)
                            coolDown--;
                    }
                    NPCs.ArchaeaNPC.VelocityClamp(Projectile, -5f, 5f);
                    break;
                case Active:
                    ai = Active;
                    Projectile.velocity = Vector2.Zero;
                    if (RightClick())
                    {
                        coolDown = time;
                        goto case Reset;
                    }
                    if (time++ < 300)
                        goto case Attack;
                    break;
                case Attack:
                    if (coolDown++ > 180)
                        goto case Reset;
                    if (target != null)
                    {
                        if (distance < maxDistance)
                            distance += 10f;
                        Color color = Color.DodgerBlue;
                        Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
                    }
                    foreach (Target t in targets)
                        if (t.npc.Distance(Projectile.Center) < distance)
                            t.AttackEffect(Target.Frozen);
                    break;

            }
            return true;
        }

        private int type = -2;
        private Vector2 move;
        public override void AI()
        {
            if (ArchaeaItem.Elapsed(60))
            {
                targets = Target.GetTargets(Projectile, range).Where(t => t != null).ToArray();
                target = Target.GetClosest(owner, targets);
            }
            if (targets == null)
                return;
            if (RightClick())
            {
                foreach (Target tg in targets.Where(t => t != null))
                    if (tg.npc.Hitbox.Contains(Main.MouseWorld.ToPoint()))
                        follow = tg;
            }
            if (follow == null)
                return;
            if (!follow.npc.active || follow.npc.Distance(owner.Center) > Main.screenWidth / 2f)
                follow = null;
            if (follow != null && ai != Attack)
            {
                moveTo = follow.npc.Center;
                if (!Projectile.Hitbox.Contains(moveTo.ToPoint()))
                    Projectile.velocity += NPCs.ArchaeaNPC.AngleToSpeed(NPCs.ArchaeaNPC.AngleTo(Projectile.Center, moveTo), moveSpeed);
            }
            NPCs.ArchaeaNPC.VelocityClamp(Projectile, -5f, 5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = Mod.Assets.Request<Texture2D>("Projectiles/CatcherMinion").Value;
            sb.Draw(tex, Projectile.position - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1.1f, SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            if (distance != 0f)
            {
                for (float r = 0f; r < distance; r += Draw.radians(distance))
                {
                    Vector2 c = NPCs.ArchaeaNPC.AngleBased(Projectile.Center, r, distance) - Main.screenPosition;
                    Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)c.X, (int)c.Y, 1, 1), Color.DodgerBlue * 0.50f);
                }
            }
        }
        public override bool PreKill(int timeLeft)
        {
            timeLeft = 36000;
            return false;
        }

        protected bool LeftClick()
        {
            return Main.mouseLeftRelease && Main.mouseLeft;
        }
        protected bool RightClick()
        {
            return Main.mouseRightRelease && Main.mouseRight;
        }

        public class CallMinionBuff : ModBuff
        {
            public override string Texture => "ArchaeaMod/Gores/Null";
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Minion Helper");
            }
            public override void ModifyBuffTip(ref string tip, ref int rare)
            {
                tip = "It eats ice";
            }
            public override bool PreDraw(SpriteBatch sb, int buffIndex, ref BuffDrawParams drawParams)
            {
                Texture2D tex = Mod.Assets.Request<Texture2D>("Buffs/mercury").Value;
                sb.Draw(tex, drawParams.Position, drawParams.SourceRectangle, Color.Firebrick * 0.67f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                return false;
            }
        }
    }
}
