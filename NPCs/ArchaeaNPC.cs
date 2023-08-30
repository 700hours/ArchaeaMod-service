using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArchaeaMod
{
    public class ModNPCID
    {
        protected static Mod getMod
        {
            get { return ModLoader.GetMod("ArchaeaMod"); }
        }
        public static int ItchySlime
        {
            get { return ModContent.NPCType<NPCs.Slime_Itchy>(); }
        }
        public static int MercurialSlime
        {
            get { return ModContent.NPCType<NPCs.Slime_Mercurial>(); }
        }
        public static int Mimic
        {
            get { return ModContent.NPCType<NPCs.Mimic>(); }
        }
        public static int Fanatic
        {
            get { return ModContent.NPCType<NPCs.Fanatic>(); }
        }
        public static int Hatchling
        {
            get { return ModContent.NPCType<NPCs.Hatchling_head>(); }
        }
        public static int Observer
        {
            get { return ModContent.NPCType<NPCs.Sky_1>(); }
        }
        //public static int Marauder
        //{
        //    get { return ModContent.NPCType<NPCs.Sky_2>(); }
        //}
        public static int MagnoliacHead
        {
            get { return ModContent.NPCType<NPCs.Bosses.Magnoliac_head>(); }
        }
        public static int MagnoliacBody
        {
            get { return ModContent.NPCType<NPCs.Bosses.Magnoliac_body>(); }
        }
        public static int MagnoliacTail
        {
            get { return ModContent.NPCType<NPCs.Bosses.Magnoliac_tail>(); }
        }
        public static int SkyBoss
        {
            get { return ModContent.NPCType<NPCs.Bosses.Sky_boss>(); }
        }
        public static int Gargoyle
        {
            get { return ModContent.NPCType<NPCs.Sky_3>(); }
        }
        public static int MechanicMinion
        {
            get { return ModContent.NPCType<NPCs.Town.MechanicMinion>(); }
        }
        public static int FollowerMenu
        {
            get { return ModContent.NPCType<NPCs.Town.FollowerMenu>(); }
        }
        public static bool Follower(int type)
        {
            return type != ModContent.NPCType<NPCs.Town.FollowerMenu>() && 
                type != ModContent.NPCType<NPCs.Town.MechanicMinion>(); 
        }
    }
}

namespace ArchaeaMod.NPCs
{
    public enum Pattern
    {
        JustSpawned,
        Idle,
        Active,
        Attack,
        Teleport,
        FadeIn,
        FadeOut
    }
    public class PatternID
    {
        public const int
            JustSpawned = 0,
            Idle = 1,
            Active = 2,
            Attack = 3,
            Teleport = 4,
            FadeIn = 5,
            FadeOut = 6;
    }
    public class _GlobalNPC : GlobalNPC
    {
        public override bool CheckActive(NPC npc)
        {
            if (npc.TypeName.Contains("Sky"))
                return true;
            return base.CheckActive(npc);
        }
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Painter)
            {
                shop.Add(new NPCShop.Entry(
                    ModContent.ItemType<Items.Tiles.m_biomepainting>(), 
                    new Condition(LocalizedText.Empty, delegate () { return !Main.dayTime; })));
            }
            else if (shop.NpcType == NPCID.Wizard)
            {
                shop.Add(new NPCShop.Entry(ModContent.ItemType<Items.Tiles.mbox_magno_1>()));
                shop.Add(new NPCShop.Entry(ModContent.ItemType<Items.Tiles.mbox_magno_2>()));
                shop.Add(new NPCShop.Entry(ModContent.ItemType<Items.Tiles.mbox_magno_boss>()));
            }
            else if (shop.NpcType == NPCID.Steampunker)
            {
                shop.Add(new NPCShop.Entry(ModContent.ItemType<Items.gray_solution>()));
            }
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            pool.Add(NPCID.BlazingWheel, spawnInfo.Player.GetModPlayer<ArchaeaPlayer>().Factory ? 0.2f : 0f);
        }
    }
    public class ArchaeaNPC
    {
        public static int defaultWidth = 800;
        public static int defaultHeight = 600;
        public static bool IsGenericNPC(NPC nPC)
        {
            if (!nPC.active) return false;
            if (nPC.life <= 0) return false;
            if (nPC.friendly) return false;
            if (nPC.dontTakeDamage) return false;
            if (nPC.boss) return false;
            return true;
        }
        public static void StrikeNPC(NPC npc, int damage, float knockback, int direction, bool crit)
        {
            NPC.HitInfo info = new NPC.HitInfo();
            info.Damage = damage;
            info.Knockback = knockback;
            info.HitDirection = direction;
            info.Crit = crit;
            npc.StrikeNPC(info, Main.netMode == 2);
        }
        public static void StrikeNetNPC(NPC npc, int damage, float knockback, int direction, int crit)
        {
            StrikeNPC(npc, damage, knockback, direction, crit == 1);
            if (Main.netMode == 1)
            {
                NetMessage.SendData(28, -1, -1, null, npc.whoAmI, damage, knockback, direction, crit);
            }
        }
        public static void HurtNetNPC(NPC npc, int damage, float knockback, int direction, int crit)
        {
            npc.life -= damage;
            if (Main.netMode == 1)
            {
                NetMessage.SendData(28, -1, -1, null, npc.whoAmI, damage, knockback, direction, crit);
            }
        }
        public static void AddBuffNetNPC(NPC npc, int buffType, int duration)
        {
            npc.AddBuff(buffType, duration, Main.netMode == 0);
            if (Main.netMode == 1)
            {
                NetMessage.SendData(53, -1, -1, null, npc.whoAmI, buffType, duration);
            }
        }
        public static void DrawChain(Texture2D tex, SpriteBatch sb, Vector2 start, Vector2 end, int len = 12)
        {
            for (int n = 0; n < start.Distance(end); n += len)
            {
                double f = 1f;
                if (n > start.Distance(end) - len)
                    f = (start.Distance(end) - n) / len;
                float angle = start.AngleTo(end);
                double cos = start.X + n * Math.Cos(angle);
                double sine = start.Y + n * Math.Sin(angle);
                float light = Lighting.Brightness((int)cos / 16, (int)sine / 16);
                Color lightColor = Color.White;
                Color _light = Color.White;
                _light.R = (byte)(lightColor.R * Math.Min(light, 1f));
                _light.G = (byte)(lightColor.G * Math.Min(light, 1f));
                _light.B = (byte)(lightColor.B * Math.Min(light, 1f));
                sb.Draw(tex,
                    new Vector2((float)cos, (float)sine) - Main.screenPosition,
                    new Rectangle(0, 0, len, (int)(len * f)), _light, angle - Draw.radian * 90f, Vector2.Zero,
                    1f, SpriteEffects.None, 0f);
            }
        }
        public static Rectangle defaultBounds(NPC npc)
        {
            return new Rectangle((int)npc.position.X - defaultWidth / 2, (int)npc.position.Y - defaultHeight / 2, defaultWidth, defaultHeight);
        }
        public static Rectangle defaultBounds(Player player)
        {
            return new Rectangle((int)player.position.X - defaultWidth / 2, (int)player.position.Y - defaultHeight / 2, defaultWidth, defaultHeight);
        }
        public static Vector2 FastMove(NPC npc)
        {
            return FindGround(npc, defaultBounds(npc));
        }
        public static Vector2 FastMove(Player player)
        {
            return FindGround(player, defaultBounds(player));
        }
        public static bool TargetBasedMove(NPC npc, Player target, bool playerRange = false)
        {
            int width = Main.screenWidth - 100;
            int height = Main.screenHeight - 100;
            Vector2 old = npc.position;
            Vector2 vector;
            if (target == null)
                return false;
            vector = FindGround(npc, new Rectangle((int)target.position.X - width / 2, (int)target.position.Y - height / 2, width, height));
            if (!ArchaeaWorld.Inbounds((int)vector.X / 16, (int)vector.Y / 16))
                return false;
            if (vector != Vector2.Zero)
                npc.position = vector;
            if (old != npc.position)
                return true;
            return false;
        }
        public static bool NoSolidTileCollision(Tile tile)
        {
            return (tile.HasTile && !Main.tileSolid[tile.TileType]) || !tile.HasTile;
        }
        public static Vector2 FindGround(NPC npc, Rectangle bounds)
        {
            var vector = FindEmptyRegion(npc, bounds);
            if (vector != Vector2.Zero)
            {
                int i = (int)vector.X / 16;
                int j = (int)(vector.Y + npc.height - 8) / 16;
                if (!ArchaeaWorld.Inbounds(i, j))
                    return Vector2.Zero;
                int max = npc.width / 16;
                int move = 0;
                Tile ground = Main.tile[i + (npc.width / 16 / 2), j + move];
                while (NoSolidTileCollision(ground))
                {
                    move++;
                    ground = Main.tile[i + (npc.width / 16 / 2), j + move];
                }
                vector.Y += move * 16 - 16;
                return vector;
            }
            else return Vector2.Zero;
        }
        public static Vector2 FindGround(Player player, Rectangle bounds)
        {
            var vector = FindEmptyRegion(player, bounds);
            for (int k = 0; k < 5; k++)
            {
                int i = (int)vector.X / 16;
                int j = (int)(vector.Y + player.height + 8) / 16;
                if (!ArchaeaWorld.Inbounds(i, j))
                    continue;
                int count = 0;
                int max = player.width / 16;
                for (int l = 0; l < player.width / 16; l++)
                {
                    Tile ground = Main.tile[i + l, j + 1];
                    if (ground.HasTile && Main.tileSolid[ground.TileType])
                        count++;
                }
                while (vector.Y + player.height < j * 16)
                    vector.Y++;
                if (Collision.SolidCollision(vector, player.width - 4, player.height - 4))
                    return Vector2.Zero;
                if (count == max)
                    return vector;
            }
            return Vector2.Zero;
        }
        public static Vector2 FindEmptyRegion(NPC npc, Rectangle check)
        {
            int tile = 16;
            int x = Main.rand.Next(check.X, check.Right);
            int y = Main.rand.Next(check.Y, check.Bottom);
            if (Main.netMode == 0)
            {
                x /= tile;
                y /= tile;
                for (int n = npc.height / 16; n >= 0; n--)
                    for (int m = 0; m < npc.width / 16; m++)
                        if (!NoSolidTileCollision(Main.tile[x + m, y + n]))
                            return Vector2.Zero;
                return new Vector2(x * tile, y * tile);
            }
            else
            {
                if (Collision.SolidTiles(x, x + npc.width, y, y + npc.height))
                    return Vector2.Zero;
            }
            return new Vector2(x, y);
        }
        public static Vector2 FindEmptyRegion(Player player, Rectangle check)
        {
            int x = Main.rand.Next(check.X, check.Right);
            int y = Main.rand.Next(check.Y, check.Bottom);
            int tile = 18;
            for (int n = player.height + tile; n >= 0; n--)
                for (int m = 0; m < player.width + tile; m++)
                {
                    int i = (x + m) / 16;
                    int j = (y + n) / 16;
                    if (Collision.SolidCollision(new Vector2(x + m, y + n), player.width, player.height))
                        return Vector2.Zero;
                    return new Vector2(x, y);
                }
            return Vector2.Zero;
        }
        public static Vector2 FindAny(NPC npc, Player target, bool findGround = true, int range = 400)
        {
            int x = 0, y = 0;
            x = Main.rand.Next((int)target.Center.X - range, (int)target.Center.X + range);
            y = Main.rand.Next((int)target.Center.Y - (int)(range * 0.67f), (int)target.Center.Y + (int)(range * 0.67f));
            x = (x - (x % 16)) / 16;
            y = (y - (y % 16)) / 16;
            if (!ArchaeaWorld.Inbounds(x, y))
                return Vector2.Zero;
            if (findGround)
            {
                if (!Main.tile[x, y + npc.height / 16 + 1].HasTile || !Main.tileSolid[Main.tile[x, y + npc.height / 16 + 1].TileType] || !Main.tileSolid[Main.tile[x + 1, y + npc.height / 16 + 1].TileType] || Main.tile[x, y + (npc.height - 4) / 16].HasTile)
                    return Vector2.Zero;
            }
            return new Vector2(x * 16, y * 16);
        }
        public static Vector2 FindAny(NPC npc, int range = 400)
        {
            int tries = 0;
            int x = 0, y = 0;
            x = Main.rand.Next((int)npc.Center.X - range, (int)npc.Center.X + range);
            y = Main.rand.Next((int)npc.Center.Y - range, (int)npc.Center.Y + range);
            x = (x - (x % 16));
            y = (y - (y % 16));
            return new Vector2(x, y);
        }
        public static Vector2 AllSolidFloors(Player target, int range = 400)
        {
            int x = (int)target.Center.X;
            int y = (int)target.Center.Y;
            int right = x + range;
            int bottom = y + range;
            List<Vector2> floor = new List<Vector2>();
            for(int i = x - range; i < right; i++)
            {
                for (int j = y - range; j < bottom; j++)
                {
                    int tile = 16;
                    if (!Collision.SolidTiles(i, i + tile, j, j + tile) && Collision.SolidTiles(i, i + tile, j + tile, j + tile * 2))
                        floor.Add(new Vector2(i, j));
                }
            }
            if (floor.Count() > 0)
                return floor[Main.rand.Next(floor.Count())];
            else return Vector2.Zero;
        }
        public static Vector2 AllSolidFloorsV2(Entity target, int range = 400)
        {
            int x = (int)(target.Center.X - range / 2);
            int y = (int)(target.Center.Y - range / 2);
            int right = x + range;
            int bottom = y + range;
            List<Vector2> floor = new List<Vector2>();
            for (int i = x; i < right; i++)
            {
                for (int j = y; j < bottom; j++)
                {
                    if (Collision.IsWorldPointSolid(new Vector2(i, j)))
                        floor.Add(new Vector2(i, j));
                }
            }
            if (floor.Count() > 0)
                return floor[Main.rand.Next(floor.Count())];
            else return Vector2.Zero;
        }
        public static bool WithinRange(Vector2 position, Rectangle range)
        {
            return range.Contains(position.ToPoint());
        }
        protected static Rectangle Range(Vector2 position, int width, int height)
        {
            return new Rectangle((int)position.X - width / 2, (int)position.Y - width / 2, width, height);
        }
        public static Player FindClosest(NPC npc, bool unlimited = false, float range = 300f)
        {
            int[] indices = new int[Main.player.Length];
            if (!unlimited)
            {
                foreach (Player target in Main.player)
                    if (target.active)
                        if (npc.Distance(target.position) < range)
                            return target;
            }
            else
            {
                int count = 0;
                for (int i = 0; i < Main.player.Length; i++)
                    if (Main.player[i].active)
                        indices[count] = Main.player[i].whoAmI;
                float[] distance = new float[indices.Length];
                for (int k = 0; k < indices.Length; k++)
                    distance[k] = Vector2.Distance(Main.player[k].position, npc.position);
                return Main.player[indices[distance.ToList().IndexOf(distance.Min())]];
            }
            return Main.player[Main.myPlayer];
        }
        public static NPC FindClosestNPC(Player player, bool unlimited = false, float range = 300f)
        {
            return Main.npc.FirstOrDefault(t => Vector2.Distance(player.Center, t.Center) < range);
        }
        public static NPC FindClosestNPC(Projectile projectile, float range = 300f)
        {
            return Main.npc.FirstOrDefault(t => Vector2.Distance(projectile.Center, t.Center) < range);
        }
        public static NPC[] FindCloseNPCs(Projectile projectile)
        {
            return Main.npc.OrderBy(t => t.position.Distance(projectile.Center)).ToArray();
        }

        public static Vector2 AngleToSpeed(float angle, float amount = 2f)
        {
            float cos = (float)(amount * Math.Cos(angle));
            float sine = (float)(amount * Math.Sin(angle));
            return new Vector2(cos, sine);
        }
        public static Vector2 AngleBased(Vector2 position, float angle, float radius)
        {
            float cos = position.X + (float)(radius * Math.Cos(angle));
            float sine = position.Y + (float)(radius * Math.Sin(angle));
            return new Vector2(cos, sine);
        }
        public static Vector2 AngleBased(float angle, float radius)
        {
            float cos = (float)(radius * Math.Cos(angle));
            float sine = (float)(radius * Math.Sin(angle));
            return new Vector2(cos, sine);
        }
        public static float RandAngle()
        {
            return Main.rand.NextFloat((float)(Math.PI * 2d));
        }
        public static float AngleTo(NPC from, Player to)
        {
            return (float)Math.Atan2(to.position.Y - from.position.Y, to.position.X - from.position.X);
        }
        public static float AngleTo(Vector2 from, Vector2 to)
        {
            return (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
        }
        public static Dust[] DustSpread(Vector2 v, int width = 1, int height = 1, int DustType = 6, int total = 10, float scale = 1f, Color color = default(Color), bool noGravity = false, float spreadSpeed = 8f)
        {
            Dust[] dusts = new Dust[total];
            for (int k = 0; k < total; k++)
            {
                Vector2 speed = ArchaeaNPC.AngleToSpeed(ArchaeaNPC.RandAngle(), spreadSpeed);
                dusts[k] = Dust.NewDustDirect(v + speed, width, height, DustType, speed.X, speed.Y, 0, color, scale);
                dusts[k].noGravity = noGravity;
            }
            return dusts;
        }
        public static bool OnHurt(int life, int oldLife, out int newLife)
        {
            if (life < oldLife)
            {
                newLife = life;
                return true;
            }
            newLife = life;
            return false;
        }
        public static void RotateIncrement(bool direction, ref float from, float to, float speed, out float result)
        {
            if (!direction)
            {
                if (from > to * -1)
                    from -= speed;
                if (from < to * -1)
                    from -= speed;
            }
            else
            {
                if (from > to)
                    from -= speed;
                if (from < to)
                    from += speed;
            }
            result = from;
        }
        public static void SlowDown(ref Vector2 velocity)
        {
            velocity.X = velocity.X > 0.1f ? velocity.X -= 0.05f : 0f;
            velocity.X = velocity.X < -0.1f ? velocity.X += 0.05f : 0f;
        }
        public static void SlowDown(ref Vector2 velocity, float rate = 0.05f)
        {
            if (velocity.X > 0.1f)
                velocity.X -= rate;
            if (velocity.X < -0.1f) 
                velocity.X += rate;
            if (velocity.Y > 0.1f)
                velocity.Y -= rate;
            if (velocity.Y < -0.1f)
                velocity.Y += rate;
        }
        public static void PositionToVel(NPC npc, Vector2 change, float speedX, float speedY, bool clamp = false, float min = -2.5f, float max = 2.5f, bool wobble = false, double degree = 0f)
        {
            float cos = wobble ? (float)(0.05f * Math.Cos(degree)) : 0f;
            float sine = wobble ? (float)(0.05f * Math.Sin(degree)) : 0f;
            if (clamp)
                VelocityClamp(npc, min, max);
            if (npc.position.X < change.X)
                npc.velocity.X += speedX + cos;
            if (npc.position.X > change.X)
                npc.velocity.X -= speedX + cos;
            if (npc.position.Y < change.Y)
                npc.velocity.Y += speedY + sine;
            if (npc.position.Y > change.Y)
                npc.velocity.Y -= speedY + sine;
        }
        public static void VelocityClamp(NPC npc, float min, float max)
        {
            Vector2 _min = new Vector2(min, min);
            Vector2 _max = new Vector2(max, max);
            Vector2.Clamp(ref npc.velocity, ref _min, ref _max, out npc.velocity);
        }
        public static void VelocityClamp(Projectile proj, float min, float max)
        {
            Vector2 _min = new Vector2(min, min);
            Vector2 _max = new Vector2(max, max);
            Vector2.Clamp(ref proj.velocity, ref _min, ref _max, out proj.velocity);
        }
        public static void VelocityClamp(ref Vector2 velocity, float min, float max)
        {
            Vector2 _min = new Vector2(min, min);
            Vector2 _max = new Vector2(max, max);
            Vector2.Clamp(ref velocity, ref _min, ref _max, out velocity);
        }
        public static void VelClampX(NPC npc, float min, float max)
        {
            if (npc.velocity.X < min)
                npc.velocity.X = min;
            if (npc.velocity.X > max)
                npc.velocity.X = max;
        }
        public static void VelClampY(NPC npc, float min, float max)
        {
            if (npc.velocity.Y < min)
                npc.velocity.Y = min;
            if (npc.velocity.Y > max)
                npc.velocity.Y = max;
        }
        public static bool IsNotOldPosition(NPC npc)
        {
            return npc.position.X < npc.oldPosition.X || npc.position.X > npc.oldPosition.X || npc.position.Y < npc.oldPosition.Y || npc.position.Y > npc.oldPosition.Y;
        }
        public static bool IsNotOldPosition(Projectile proj)
        {
            return proj.position.X < proj.oldPosition.X || proj.position.X > proj.oldPosition.X || proj.position.Y < proj.oldPosition.Y || proj.position.Y > proj.oldPosition.Y;
        }

        protected static bool SolidGround(Tile[] tiles)
        {
            int count = 0;
            foreach (Tile ground in tiles)
                if (!NotActiveOrSolid(ground))
                {
                    count++;
                    if (count == tiles.Length)
                        return true;
                }
            return false;
        }
        protected static bool NotActiveOrSolid(int i, int j)
        {
            return (!Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType]) || (Main.tile[i, j].HasTile && !Main.tileSolid[Main.tile[i, j].TileType]);
        }
        protected static bool NotActiveOrSolid(Tile tile)
        {
            return (!tile.HasTile && Main.tileSolid[tile.TileType]) || (tile.HasTile && !Main.tileSolid[tile.TileType]);
        }
        

        #region out of view
        /*
        int add = 30;
        int count = 0;
        int max = 3;
        List<Vector2> vectors = new List<Vector2>();
        Rectangle screen = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
        while (count < max)
        {
            check.Inflate(add, add);
            if (check.Width > screen.Width + 30)
                break;
            count++;
        }
        for (int i = check.Left; i < check.Right; i++)
            for (int j = check.Left; j < check.Right; i++)
                if (!WithinRange(new Vector2(i, j), screen) && ArchaeaWorld.Inbounds(i, j))
                    vectors.Add(new Vector2(i, j));
        if (vectors.Count > 0)
            return vectors.ToArray();
        return new Vector2[] { Vector2.Zero };*/
        #endregion
        #region depracated
        /*if (!ArchaeaWorld.Inbounds(x, y))
        {
            npc.active = false;
            return Vector2.Zero;
        }
        int count = 0;
        int max = npc.width / 16 * (npc.height / 16);
        for (int l = 0; l < npc.height; l++)
            for (int k = 0; k < npc.width; k++)
            {
                int i = (x + k) / 16;
                int j = (y + l - npc.height) / 16;
                if (!ArchaeaWorld.Inbounds(i, j))
                    continue;
                Tile tile = Main.tile[i, j];
                if (NotActiveOrSolid(tile))
                    count++;
                else
                {
                    count = 0;
                    break;
                }
                if (k == 0 && l == npc.height - 1)
                    for (int m = 0; m < npc.width / 16; m++)
                    {
                        Tile ground = Main.tile[i + m, j + 1];
                        if (ground.active() && Main.tileSolid[ground.type])
                            count++;
                    }
                if (count == max + npc.width / 16)
                    return new Vector2(x, y);
            }
        return new Vector2(rangeOut, rangeOut);*/
        #endregion
        #region Depracated SpawnOnGround
        /*
        Tile[] ground = new Tile[npc.width / 16];
        Vector2 spawn = FindNewPosition(npc, bounds);
        int i = (int)(spawn.X / 16);
        int j = (int)(spawn.Y + npc.height) / 16;
        int count = 0;
        for (int k = 0; k < ground.Length; k++)
        {
            ground[k] = Main.tile[i + k, j + 1];
            if (ground[k].active() && Main.tileSolid[ground[k].type])
            {
                count++;
                if (count == ground.Length)
                    return spawn;
            }
        }
        return Vector2.Zero;*/
        #endregion
    }
}
