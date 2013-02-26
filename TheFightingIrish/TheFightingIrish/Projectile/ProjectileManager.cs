using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace TheFightingIrish.Projectiles
{
    public delegate void CannonFired();

    public class ProjectileManager : DrawableGameComponent
    {
        public static List<int> enemyIndexes = new List<int>();
        static List<Projectile> projectiles = new List<Projectile>();

        static float drawLayer = 0.9f;

        static Texture2D minigunTex, cannonTex, mineTex;
        static Texture2D rocketTex, bombTex;
        static float miniGunSpeed = 8;
        static float cannonSpeed = 12;
        static float mineSpeed = 0;
        static float rocketSpeed = 400;

        static float miniGunFireLimit = 0.1f;
        static float cannonFireLimit = 0.3f;
        static float mineFireLimit = 0.8f;

        static float miniGunTimer = 0;
        static float cannonFireTimer = 0;
        static float mineFireTimer = 0;

        SpriteBatch spriteBatch;

        public static event CannonFired CannonJustFired;

        public ProjectileManager(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            game.Components.Add(this);

            this.spriteBatch = spriteBatch;
            Load(game.Content);
        }

        public void Load(ContentManager content)
        {
            minigunTex = content.Load<Texture2D>(@"Textures/Sprites/MinigunTexture");
            cannonTex = content.Load<Texture2D>(@"Textures/Sprites/CannonProjectile");
            mineTex = content.Load<Texture2D>(@"Textures/Sprites/mineTexture");
            rocketTex = content.Load<Texture2D>(@"Textures/Sprites/Rocket");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update(gameTime);

                if (!Camera2D.ScreenRect.Contains((int)projectiles[i].Position.X, (int)projectiles[i].Position.Y))
                {
                    projectiles[i].IsActive = false;
                }

                if (!projectiles[i].IsActive || EnemyManager.CheckCollision(projectiles[i]))
                {
                    projectiles.RemoveAt(i);
                }
            }

            cannonFireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            miniGunTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            mineFireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(spriteBatch);
            }

            base.Draw(gameTime);
        }

        public static void AddMinigunProjectile(Vector2 position, Vector2 direction, ProjectileOwner owner)
        {
            if (miniGunTimer > miniGunFireLimit)
            {
                projectiles.Add(new MiniGun(minigunTex, position, minigunTex.Width, Color.White, 0.0f,
                    1.0f, drawLayer, 0, ProjectileType.Minigun, direction, miniGunSpeed + Player.PlayerSpeed, owner));

                miniGunTimer = 0;
            }
        }

        public static void AddCannonProjectile(Vector2 position, Vector2 direction, ProjectileOwner owner)
        {
            if (cannonFireTimer > cannonFireLimit)
            {
                projectiles.Add(new Cannon(cannonTex, position, cannonTex.Width, Color.White, 0.0f,
                    1.0f, drawLayer, 0, ProjectileType.Cannon, direction, cannonSpeed + Player.PlayerSpeed, owner));

                if (CannonJustFired != null)
                    CannonJustFired();

                cannonFireTimer = 0;
            }
        }

        public static void AddMineProjectile(Vector2 position, Vector2 direction, ProjectileOwner owner)
        {
            if (mineFireTimer > mineFireLimit)
            {
                projectiles.Add(new Mine(mineTex, position, mineTex.Width, Color.White, 0.0f,
                    1.0f, drawLayer, 0, ProjectileType.Mines, direction, mineSpeed, owner));

                mineFireTimer = 0;
            }
        }

        public static void LaunchMissiles(Vector2 position, ProjectileOwner owner)
        {
            foreach(int i in enemyIndexes)
            {
                Missile r = new Missile(rocketTex, position, rocketTex.Width, Color.White,
                    0.0f, 1.0f, drawLayer, 0, i, ProjectileType.Missile, new Vector2(1, 0), rocketSpeed, owner);
                projectiles.Add(r);
            }

            enemyIndexes.Clear();
        }

        bool HasCollided(Missile r)
        {
            for (int i = 0; i < EnemyManager.Enemies.Count; i++)
            {
                if (r.MissileTargetIndex == EnemyManager.Enemies[i].Index)
                {
                    if (r.BoundingRect.Intersects(EnemyManager.Enemies[i].BoundingRect))
                        return true;
                }
            }

            return false;
        }
    }
}
