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
using TheFightingIrish.Projectiles;

namespace TheFightingIrish
{
    public class EnemyManager : DrawableGameComponent
    {
        public static List<Enemy> Enemies = new List<Enemy>();

        SpriteBatch spriteBatch;

        float spawnTimer = 1.0f;
        float timer = 0;

        Texture2D enemyTexture;
        Texture2D targetTex;
        Random rand = new Random();
        float enemySpeed = 0.9f;

        int enemyCount = 0;

        SpriteFont font;

        public EnemyManager(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            game.Components.Add(this);
            this.spriteBatch = spriteBatch;

            enemyTexture = game.Content.Load<Texture2D>(@"Textures/Sprites/Enemy");
            targetTex = game.Content.Load<Texture2D>(@"Textures/HUD/Target");
            font = game.Content.Load<SpriteFont>(@"Fonts/Debug");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > spawnTimer)
            {
                timer = 0;

                AddEnemy(new Vector2(Camera2D.ScreenRect.X - 100, rand.Next(100, 600)));
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Draw(spriteBatch);
            }

            base.Draw(gameTime);
        }

        public void AddEnemy(Vector2 position)
        {
            Enemies.Add(new Enemy(enemyTexture, position, enemyTexture.Width, Color.White, 
                0, 1, 0.9f, 0, enemySpeed, enemyCount, targetTex, font));

            enemyCount++;
        }

        public static bool CheckCollision(Projectile p)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].BoundingRect.Intersects(p.BoundingRect))
                {
                    if (p.Type != ProjectileType.Missile)
                    {
                        Enemies.RemoveAt(i);
                        return true;
                    }
                    else //Ensure that missiles only hit their target and not other enemies 
                    {                 
                        if (Enemies[i].Index == ((Missile)p).MissileTargetIndex)
                        {
                            Enemies.RemoveAt(i);
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
